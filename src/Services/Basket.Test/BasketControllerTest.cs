using Basket.API.Controllers;
using Basket.Test.Mocks;
using Basket.Test.Mocks.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Xunit;

namespace Basket.Test
{
    public class BasketControllerTest
    {
        private MockDbContext _db = MockDbContext.GetDbContext();

        [Fact(DisplayName = "test")]
        public async Task Get_basket_success()
        {
            var controller = new BasketsController(_db);

            var actionResult = await controller.GetItemsAsync(new Dictionary<string, string>());
            Assert.Equal((int)HttpStatusCode.OK, (actionResult as ObjectResult).StatusCode);
        }
    }
}
