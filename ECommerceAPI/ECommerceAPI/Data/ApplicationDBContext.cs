using ECommerceAPI.Data.Entities;
using Microsoft.EntityFrameworkCore;
using System;

namespace ECommerceAPI.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<Product> Products { get; set; }
        public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<User>()
                        .HasIndex(e => e.UserName)
                        .IsUnique();

            modelBuilder.Entity<User>()
                       .HasIndex(e => e.EmailAddress)
                       .IsUnique();
        }

    }
}
