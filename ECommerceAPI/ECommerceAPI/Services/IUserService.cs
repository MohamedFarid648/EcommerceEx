
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using ECommerceAPI.Data;
using ECommerceAPI.Data.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace ECommerceAPI.Services
{
    public interface IUserService
    {
        User Authenticate(string username, string password);
        User RegisterUser(string username, string password, string emailAddress);

    }

    public class UserService : IUserService
    {
        private readonly IConfiguration _configuration;
        private readonly ApplicationDbContext _dbContext;

        public UserService(IConfiguration configuration, ApplicationDbContext dbContext)
        {
            _configuration = configuration;
            _dbContext = dbContext;
        }


        public User Authenticate(string username, string password)
        {
                var user = _dbContext.Users.SingleOrDefault(x => x.UserName == username && x.Password == password);

                if (user == null)
                    return null;
                //user.lastLoginTime = DateTime.UtcNow;
               //_dbContext.SaveChanges();
                user.Token = CreateUserToken(user.Id);
                user.Password = null; // Remove password from response

            return user;
            }

        public User RegisterUser(string username, string password , string emailAddress)
        {
            if (_dbContext.Users.Any(x => x.UserName == username))
                return null;//BadRequest("Username is already taken.");

            // Validate password length
          /*  if (password.Length < _configuration.GetValue<int>("AuthSettings:MinPasswordLength"))
            {

                return null;// BadRequest("Password length should be at least " + _configuration.GetValue<int>("AuthSettings:MinPasswordLength") + " characters.");

            }*/

            // Create a new user
            var user = new User
            {
                UserName = username,
                Password = password,
                EmailAddress = emailAddress
            };
            user.Token = CreateUserToken(user.Id);

            // Save the user to the database
            _dbContext.Users.Add(user);
            _dbContext.SaveChanges();

            // Return the registered user without the password
            user.Password = null;

            return user;
        }

        public string CreateUserToken(long Id)
        {


            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(_configuration["Jwt:Secret"]);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, Id.ToString())
                }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
    }
