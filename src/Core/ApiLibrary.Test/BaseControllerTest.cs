using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Mvc;
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
        private static DbContextMock _db = DbContextMock.GetDbContext();

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
        public async Task get_models_with_Filter()
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
            var result = ((IEnumerable<ModelTest>)(actionResult.Result as ObjectResult).Value);

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

        [Fact]
        public async Task get_models_with_range()
        {
            var controller = new BaseControllerMock(_db);
            var actionResult = await controller.GetItemsAsync(new Dictionary<string, string> { { "Range", "2-3" } });
            var result = ((IEnumerable<ModelTest>)(actionResult.Result as ObjectResult).Value);

            Assert.Equal((int)System.Net.HttpStatusCode.PartialContent, (actionResult.Result as ObjectResult).StatusCode);
            Assert.Equal(2, result.Count());
        }

        [Fact]
        public async Task get_models_with_range_throw_conversion_exception()
        {
            var controller = new BaseControllerMock(_db);

            var actionResult = await controller.GetItemsAsync(new Dictionary<string, string> { { "Range", "toto-tata" } });

            Assert.Equal((int)System.Net.HttpStatusCode.BadRequest, (actionResult.Result as ObjectResult).StatusCode);
        }

        /*

      [Fact]
      public async Task get_models_with_range_throw_range_exception()
      {
          var controller = new BaseControllerMock(_db);
          ActionResult<IEnumerable<ModelTest>> actionResult;
          await Assert.ThrowsAsync<RangeException>(async () =>
               {
                   actionResult = await controller.GetItemsAsync(new Dictionary<string, string> { { "Range", "0-26" } });
               }
          );
          Assert.Equal((int)System.Net.HttpStatusCode.BadRequest, (actionResult.Result as ObjectResult).StatusCode);
      }

      [Fact]
      public async Task get_models_with_fork_throw_fork_exception()
      {
          var controller = new BaseControllerMock(_db);
          ActionResult<IEnumerable<ModelTest>> actionResult;
          await Assert.ThrowsAsync<ForkException>(async () =>
          {
              actionResult = await controller.GetItemsAsync(new Dictionary<string, string> { { "String", "[toto,tata]" } });
          }
          );
          Assert.Equal((int)System.Net.HttpStatusCode.BadRequest, (actionResult.Result as ObjectResult).StatusCode);
      }


      [Fact]
      public async Task get_models_with_range_throw_search_exception()
      {
          var controller = new BaseControllerMock(_db);
          ActionResult<IEnumerable<ModelTest>> actionResult;
          await Assert.ThrowsAsync<SearchException>(async () =>
          {
              actionResult = await controller.GetItemsAsync(new Dictionary<string, string> { { "Integer", "1" } });
          }
          );
          Assert.Equal((int)System.Net.HttpStatusCode.BadRequest, (actionResult.Result as ObjectResult).StatusCode);
      }

      [Fact]
      public async Task get_models_with_range_throw_search_exception()
      {
          var controller = new BaseControllerMock(_db);
          ActionResult<IEnumerable<ModelTest>> actionResult;
          await Assert.ThrowsAsync<SearchException>(async () =>
          {
              actionResult = await controller.GetItemsAsync(new Dictionary<string, string> { { "Integer", "1" } });
          }
          );
          Assert.Equal((int)System.Net.HttpStatusCode.BadRequest, (actionResult.Result as ObjectResult).StatusCode);
      }

      [Fact]
      public async Task get_models_with_range_throw_search_exception()
      {
          var controller = new BaseControllerMock(_db);
          var actionResult = await controller.GetItemsAsync(new Dictionary<string, string> { { "Range", "toto-tata" } });

          Assert.Equal((int)System.Net.HttpStatusCode.BadRequest, (actionResult.Result as ObjectResult).StatusCode);

      }
      */
    }
}
