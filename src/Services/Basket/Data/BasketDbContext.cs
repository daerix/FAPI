using ApiLibrary.Core.Entity;
using Microsoft.EntityFrameworkCore;

namespace Basket.Data
{

    public class BasketDbContext : BaseDbContext
    {

        public BasketDbContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<Models.Basket> Baskets { get; set; }

    }
}