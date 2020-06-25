﻿using Basket.API.Data;
using Basket.API.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
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

        public async static Task<MockDbContext> GetDbContext(bool withData = true)
        {
            var  options = new DbContextOptionsBuilder().UseInMemoryDatabase(Guid.NewGuid().ToString()).Options;
            var db = new MockDbContext(options);

            if (withData)
            {
                db.Baskets.Add(new Basket.API.Models.Basket { Id = 1, State = API.Enums.BasketStates.PENDING, CreatedAt = DateTime.Now, User = 1 });
                db.Baskets.Add(new Basket.API.Models.Basket { Id = 2, State = API.Enums.BasketStates.PENDING, CreatedAt = DateTime.Now, User = 2 });
                db.Baskets.Add(new Basket.API.Models.Basket { Id = 3, State = API.Enums.BasketStates.PENDING, CreatedAt = DateTime.Now, User = 3 });

                db.Bookings.Add(new Booking { Id = 1, BasketID = 2, Price = 90, ProductID = 1});
                db.Bookings.Add(new Booking { Id = 2, BasketID = 2, Price = 40, ProductID = 2 });
                db.Bookings.Add(new Booking { Id = 3, BasketID = 1, Price = 90, ProductID = 3 });
                await db.SaveChangesAsync();
            }

            return db;
        }
    }
}
