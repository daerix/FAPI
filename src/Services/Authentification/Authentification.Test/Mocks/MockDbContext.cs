using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Authentification.Test.Mocks
{
    class MockDbContext
    {
        public MockDbContext(DbContextOptions options) : base(options)
        {
        }

        public static MockDbContext GetDbContext(bool withData = true)
        {
            var options = new DbContextOptionsBuilder().UseInMemoryDatabase("dbtest").Options;
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
