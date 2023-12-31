
using System;
using System.Reflection;
using System.Text;
using ECommerceAPI.Data.Entities;
using ECommerceAPI.Data;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using ECommerceAPI.Services;

namespace ECommerceAPI
{
    public class Startup
    {
        readonly string MyAllowSpecificOrigins = "_myAllowSpecificOrigins";

        public Startup(IConfiguration configuration, IWebHostEnvironment env)
        {
            Configuration = configuration;
            _env = env;
        }

        public IConfiguration Configuration { get; }

        private IWebHostEnvironment _env { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {

            var mySqlConnectionString = Configuration.GetConnectionString("DefaultConnection");
            var migrationsAssembly = typeof(Program).GetTypeInfo().Assembly.GetName().Name;

            services.AddCors(options =>
            {
                options.AddPolicy(name: MyAllowSpecificOrigins,
                                    builder =>
                                    {
                                        builder
                                        .AllowAnyOrigin()
                                        .AllowAnyHeader()
                                        .AllowAnyMethod();
                                    });
            });

            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseMySql(mySqlConnectionString, ServerVersion.AutoDetect(mySqlConnectionString), b => b.MigrationsAssembly(migrationsAssembly))
            );





            // Adding Authentication  
            // Add authentication
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = "ISSUERmhjly",
                        ValidAudience = "Audgrtrg",
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("4578965248"))
                    };
                });




            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IImageService, ImageService>();


            services.AddSwaggerGen(options =>
            {
                options.CustomSchemaIds(type => type.ToString().Replace("ECommerceAPI.Data.Entities.", ""));

                options.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "Version 1.0.0",
                    Title = "ECommerceAPI APIs",
                    Description = "ECommerceAPI APIs"
                });

                // To Enable authorization using Swagger (JWT)    
                options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.OAuth2,
                    Scheme = "Bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Description = "Enter 'Bearer' [space] and then your valid token in the text input below.\r\n\r\nExample: \"Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9\"",
                    Flows = new OpenApiOAuthFlows
                    {
                        Password = new OpenApiOAuthFlow
                        {
                            TokenUrl = new Uri("/api/auth/swagger/login", UriKind.Relative)
                        }
                    }
                });

                options.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        Array.Empty<string>()
                    }
                });
            });
            services.AddControllers();
            services.AddTransient(s => s.GetService<IHttpContextAccessor>().HttpContext.User);

          
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment() || env.IsStaging())
            {
                app.UseDeveloperExceptionPage();
            }

           
            app.UseSwagger();

            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "ECommerceAPI v1");
                c.OAuthClientId("null");
                c.OAuthClientSecret("null");
            });

            app.UseCors(MyAllowSpecificOrigins);

            app.UseStaticFiles();

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthentication();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });


        }

    }
}
