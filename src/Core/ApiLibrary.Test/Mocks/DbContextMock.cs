using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using ApiLibrary.Test.Mocks.Models;
using ApiLibrary.Core.Entities;

namespace ApiLibrary.Test.Mocks
{
    public class DbContextMock : BaseDbContext
    {
        public DbContextMock(DbContextOptions options) : base(options)
        {
        }

         public DbSet<ModelTest> Models { get; set; }
        public DbSet<ParentModelTest> ParentModels { get; set; }

        public static DbContextMock GetDbContext(bool withData = true)
        {
            var options = new DbContextOptionsBuilder().UseInMemoryDatabase("dbtest").Options;
            var db = new DbContextMock(options);

            if (withData)
            {
                db.ParentModels.Add(new ParentModelTest { Id = 1 });
                db.Models.Add(new ModelTest { String = "String1", Integer = 1, Double = 1.1, Decimal = 1.1M, Date = new DateTime(1,1,1), ParentModelId = 1 });
                db.Models.Add(new ModelTest { String = "String2", Integer = 2, Double = 2.2, Decimal = 2.2M, Date = new DateTime(2, 2, 2), ParentModelId = 1 });
                db.Models.Add(new ModelTest { String = "String3", Integer = 3, Double = 3.3, Decimal = 3.3M, Date = new DateTime(3, 3, 3), ParentModelId = 1 });
                db.Models.Add(new ModelTest { String = "String4", Integer = 1, Double = 1.1, Decimal = 1.1M, Date = new DateTime(1, 1, 1), ParentModelId = 1 });
                db.Models.Add(new ModelTest { String = "String5", Integer = 2, Double = 2.2, Decimal = 2.2M, Date = new DateTime(2, 2, 2), ParentModelId = 1 });
                db.Models.Add(new ModelTest { String = "StringTest6", Integer = 3, Double = 3.3, Decimal = 3.3M, Date = new DateTime(3, 3, 3), ParentModelId = 1 });

                db.SaveChanges();
            }
            return db;
        }
    }

}
