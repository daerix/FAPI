using ApiLibrary.Core.Entities;
using Authentification.API.Models;
using Microsoft.EntityFrameworkCore;

namespace Authentification.API.Data
{
    public class UserDbContext : BaseDbContext
    {
        public UserDbContext(DbContextOptions options) : base(options)
        {
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>();
            base.OnModelCreating(modelBuilder);
        }

        public DbSet<User> Users { get; set; }
    }
}
