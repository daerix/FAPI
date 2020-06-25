using ApiLibrary.Test.Mocks;
using ApiLibrary.Test.Mocks.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace ApiLibrary.Test
{
    public class BaseDbContextTest
    {
        public DateTime _today = DateTime.Now;

        [Fact]
        public async Task create_element_check_createAt_value()
        {
            var db = BaseDbContextMock.GetDbContext();

            db.Add(new ModelTest
            {
                String = "String11",
                Integer = 1,
                Double = 1.1,
                Decimal = 1.1M,
                Date = new DateTime(1, 1, 1),
                ParentModelId = 5
            });

            db.SaveChanges();

            var item = await db.Models.Where(x => x.String == "String11").FirstAsync();

            Assert.Equal(_today.Date, item.CreatedAt.Date);
            Assert.NotNull(item.UpdatedAt);
            Assert.Equal(_today.Date, item.UpdatedAt?.Date);
            Assert.Null(item.DeletedAt);

        }

        [Fact]
        public async Task update_element_check_updateAt_value()
        {
            var db = BaseDbContextMock.GetDbContext();

            var item = await db.Models.Where(x => x.String == "String1").FirstAsync();
            var oldUpdatedDate = item.UpdatedAt;

            db.Update<ModelTest>(item);
            await db.SaveChangesAsync();

            Assert.NotNull(item.UpdatedAt);
            Assert.NotEqual(oldUpdatedDate, item.UpdatedAt);
            Assert.Equal(_today.Date, item.UpdatedAt?.Date);
            Assert.Null(item.DeletedAt);

        }

        [Fact]
        public async Task delete_element_check_updateAt_value()
        {
            var db = BaseDbContextMock.GetDbContext();
            var item = await db.Models.Where(x => x.String == "String1").FirstAsync();

            db.Remove<ModelTest>(item);
            await db.SaveChangesAsync();

            Assert.NotNull(item.UpdatedAt);
            Assert.Equal(_today.Date, item.UpdatedAt?.Date);
            Assert.NotNull(item.DeletedAt);
            Assert.Equal(_today.Date, item.DeletedAt?.Date);
        }
    }
}
