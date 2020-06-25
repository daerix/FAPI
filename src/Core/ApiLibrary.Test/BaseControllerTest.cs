using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using Xunit;
using System.Linq;
using ApiLibrary.Test.Mocks.Models;
using ApiLibrary.Test.Mocks;
using ApiLibrary.Core.Exceptions;


namespace ApiLibrary.Test
{
    public class BaseControllerTest
    {
        private static BaseDbContextMock _db = BaseDbContextMock.GetDbContext();

        [Fact]
        public async Task get_models()
        {
            var controller = new BaseControllerMock(_db);

            var actionResult = await controller.GetItemsAsync(new Dictionary<string, string> { });
            var result = ((IEnumerable<ModelTest>)(actionResult.Result as ObjectResult).Value);

            Assert.Equal((int)System.Net.HttpStatusCode.OK, (actionResult.Result as ObjectResult).StatusCode);
            Assert.Equal(_db.Models.ToList().Count(), result.Count());
        }

        [Fact]
        public async Task get_models_with_filter()
        {
            var controller = new BaseControllerMock(_db);

            var actionResult = await controller.GetItemsAsync(new Dictionary<string, string> { { "String", "String1" } });
            var result = ((IEnumerable<ModelTest>)(actionResult.Result as ObjectResult).Value);

            Assert.Equal((int)System.Net.HttpStatusCode.OK, (actionResult.Result as ObjectResult).StatusCode);
            Assert.Equal(_db.Models.Where(x => x.String == "String1").ToList().Count(), result.Count());
        }

        [Fact]
        public async Task get_models_with_search()
        {
            var controller = new BaseControllerMock(_db);

            var actionResult = await controller.GetItemsAsync(new Dictionary<string, string> { { "String", "String*" } });
            var result = ((IEnumerable<ModelTest>)(actionResult.Result as ObjectResult).Value);

            Assert.Equal((int)System.Net.HttpStatusCode.OK, (actionResult.Result as ObjectResult).StatusCode);
            Assert.Equal(_db.Models.Where(x => x.String.Contains("String")).ToList().Count(), result.Count());
        }

        [Fact]
        public async Task get_models_with_fork()
        {
            var controller = new BaseControllerMock(_db);
            var actionResult = await controller.GetItemsAsync(new Dictionary<string, string> { { "Integer", "[2,3]" } });
            var result = ((IEnumerable<ModelTest>)(actionResult.Result as ObjectResult).Value);

            Assert.Equal((int)System.Net.HttpStatusCode.OK, (actionResult.Result as ObjectResult).StatusCode);
            Assert.Equal(_db.Models.Where(x => x.Integer >= 2 && x.Integer <= 3).ToList().Count(), result.Count());
        }

        [Fact]
        public async Task get_models_with_sort()
        {
            var controller = new BaseControllerMock(_db);
            var actionResult = await controller.GetItemsAsync(new Dictionary<string, string> { { "Sort", "String" } });
            var result = ((IEnumerable<ModelTest>)(actionResult.Result as ObjectResult).Value);

            Assert.Equal((int)System.Net.HttpStatusCode.OK, (actionResult.Result as ObjectResult).StatusCode);
            Assert.True(result.SequenceEqual(_db.Models.OrderBy(x => x.String).ToList()));
        }

        [Fact]
        public async Task get_models_with_fields()
        {
            var controller = new BaseControllerMock(_db);
            var actionResult = await controller.GetItemsAsync(new Dictionary<string, string> { { "Field", "String,Integer" } });
            var result = ((IEnumerable<object>)(actionResult.Result as ObjectResult).Value);

            Assert.Equal((int)System.Net.HttpStatusCode.OK, (actionResult.Result as ObjectResult).StatusCode);

            foreach (dynamic data in result)
            {
                var dataDictionary = (IDictionary<string, object>)data;

                Assert.True(dataDictionary.ContainsKey("String"));
                Assert.True(dataDictionary.ContainsKey("Integer"));
                Assert.False(dataDictionary.ContainsKey("Date"));
                Assert.False(dataDictionary.ContainsKey("Id"));
                Assert.False(dataDictionary.ContainsKey("Double"));
                Assert.False(dataDictionary.ContainsKey("Decimal"));
            }
        }

        /*

        [Fact]
        public async Task get_models_with_range()
        {
            var controller = new BaseControllerMock(_db);
            var actionResult = await controller.GetItemsAsync(new Dictionary<string, string> { { "Range", "2-3" } });
            var result = ((IEnumerable<object>)(actionResult.Result as ObjectResult).Value);

            Assert.Equal((int)System.Net.HttpStatusCode.PartialContent, (actionResult.Result as ObjectResult).StatusCode);
            Assert.Equal(2, result.Count());
        }
        */

        [Fact]
        public async Task get_models_with_range_throw_conversion_exception()
        {
            var controller = new BaseControllerMock(_db);

            var actionResult = await controller.GetItemsAsync(new Dictionary<string, string> { { "Range", "toto-tata" } });

            Assert.Equal((int)System.Net.HttpStatusCode.BadRequest, (actionResult.Result as ObjectResult).StatusCode);
        }


        [Fact]
        public async Task get_models_with_range_throw_range_exception()
        {
            var controller = new BaseControllerMock(_db);
            var actionResult = await controller.GetItemsAsync(new Dictionary<string, string> { { "Range", "0-26" } });

            var badRequestResult = Assert.IsType<BadRequestObjectResult>(actionResult.Result);
            Assert.IsType<RangeException>(badRequestResult.Value);
           
        }

        [Fact]
        public async Task get_models_with_fork_throw_fork_exception()
        {
            var controller = new BaseControllerMock(_db);
            var actionResult = await controller.GetItemsAsync(new Dictionary<string, string> { { "String", "[toto,tata]" } });
           
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(actionResult.Result);
            Assert.IsType<ForkException>(badRequestResult.Value);

        }

        [Fact]
        public async Task get_models_with_search_throw_search_exception()
        {
            var controller = new BaseControllerMock(_db);
            var actionResult = await controller.GetItemsAsync(new Dictionary<string, string> { { "Integer", "1*" } });

            var badRequestResult = Assert.IsType<BadRequestObjectResult>(actionResult.Result);
            Assert.IsType<SearchException>(badRequestResult.Value);
        }

        [Fact]
        public async Task get_model_without_mapping()
        {
            var controller = new BaseControllerMock(_db);
            var actionResult = await controller.GetItemByIdAsync(1);

            var result = ((ModelTest)(actionResult.Result as ObjectResult).Value);
            Assert.Equal((int)System.Net.HttpStatusCode.OK, (actionResult.Result as ObjectResult).StatusCode);
            Assert.Equal(_db.Models.Where(x => x.Id == 1).First(), result);
        }

        [Fact]
        public async Task get_model_with_mapping()
        {
            var controller = new BaseControllerMock(_db);
            var actionResult = await controller.GetItemByIdAsync(1, true);

            var result = ((ModelTest)(actionResult.Result as ObjectResult).Value);

            Assert.Equal((int)System.Net.HttpStatusCode.OK, (actionResult.Result as ObjectResult).StatusCode);
            Assert.Equal(_db.Models.Where(x => x.Id == 1).Include(x => x.Parent).First(), result);
        }

        [Fact]
        public async Task get_model_throw_not_found()
        {
            var controller = new BaseControllerMock(_db);
            var actionResult = await controller.GetItemByIdAsync(8);

            Assert.IsType<NotFoundResult>(actionResult.Result);
        }

        [Fact]
        public async Task post_model()
        {
            var db = BaseDbContextMock.GetDbContext();
            var controller = new BaseControllerMock(db);

            var item = new ModelTest() { String = "TestPost", Integer = 1 };
            var actionResult = await controller.PostItemAsync(item);
            var ent = await db.Models.Where(x => x.String == "TestPost").FirstAsync();

            Assert.IsType<CreatedResult>(actionResult);
            Assert.NotNull(ent);
        }

        [Fact]
        public async Task post_model_throw_bad_request()
        {
            var db = BaseDbContextMock.GetDbContext();
            var controller = new BaseControllerMock(db);
            controller.ModelState.AddModelError("String", "Required");
            controller.ModelState.AddModelError("Integer", "Required");

            var item = new ModelTest();
            var actionResult = await controller.PostItemAsync(item);

            Assert.IsType<BadRequestObjectResult>(actionResult);
            Assert.Equal((int)System.Net.HttpStatusCode.BadRequest, (actionResult as ObjectResult).StatusCode);
            Assert.NotNull((actionResult as ObjectResult).Value);
        }

        [Fact]
        public async Task put_model()
        {
            var db = BaseDbContextMock.GetDbContext();
            var controller = new BaseControllerMock(db);

            var item = await db.Models.FindAsync(1);
            item.ParentModelId = 2;
            var actionResult = await controller.PutItemAsync(item, item.Id);

            Assert.IsType<NoContentResult>(actionResult);
            Assert.Equal(await db.Models.FindAsync(1), item);
        }

        [Fact]
        public async Task put_model_throw_bad_request()
        {
            var db = BaseDbContextMock.GetDbContext();
            var controller = new BaseControllerMock(db);

            var item = db.Models.Find(1);
            var actionResult = await controller.PutItemAsync(item, 3);

            Assert.IsType<BadRequestResult>(actionResult);
        }

        [Fact]
        public async Task delete_model()
        {
            var db = BaseDbContextMock.GetDbContext();
            var controller = new BaseControllerMock(db);

            var actionResult = await controller.DeleteItemAsync(6);
            var ent = await db.Models.Where(x => x.Id == 6).FirstAsync();

            Assert.IsType<OkResult>(actionResult);
            Assert.NotNull(ent.DeletedAt);
        }

        [Fact]
        public async Task delete_model_throw_not_found()
        {
            var db = BaseDbContextMock.GetDbContext();
            var controller = new BaseControllerMock(db);

            var actionResult = await controller.DeleteItemAsync(8);

            Assert.IsType<NotFoundResult>(actionResult);
        }

    }
}
