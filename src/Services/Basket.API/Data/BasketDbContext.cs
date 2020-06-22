using ApiLibrary.Core.Entities;
using Basket.API.Models;
using Microsoft.EntityFrameworkCore;

namespace Basket.API.Data
{

    public class BasketDbContext : BaseDbContext
    {

        public BasketDbContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<Models.Basket> Baskets { get; set; }

        public DbSet<Booking> Bookings { get; set; }

    }
}