using Basket.API.Data;
using Basket.Test.Mocks.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Basket.Test.Mocks
{
    class MockDbContext : BasketDbContext
    {

        public MockDbContext(DbContextOptions options): base(options)
        {
        }

        public static MockDbContext GetDbContext(bool withData = true)
        {
            var  options = new DbContextOptionsBuilder().UseInMemoryDatabase("dbtest").Options;
            var db = new MockDbContext(options);

            if (withData)
            {
                db.Baskets.Add(new BasketMock { Id = 1 });
                db.Baskets.Add(new BasketMock { Id = 2 });
                db.Baskets.Add(new BasketMock { Id = 3 });
                db.SaveChangesAsync();
            }

            return db;
        }
    }
}
