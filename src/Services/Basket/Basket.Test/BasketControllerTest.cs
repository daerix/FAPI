using Basket.API.Controllers;
using Basket.API.Enums;
using Basket.API.Models;
using Basket.Test.Mocks;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Xunit;

namespace Basket.Test
{
    public class BasketControllerTest
    {
        //private MockDbContext _db = await MockDbContext.GetDbContext();
        private BasketsController controller;

        public BasketControllerTest()
        {
        }

        [Fact]
        public async Task Should_Create_Basket_If_It_Does_Not_Exists()
        {
            using (var context = await MockDbContext.GetDbContext())
            {
                controller = new BasketsController(context);
                var basketMock = new Basket.API.Models.Basket
                {
                    User = 10
                };
                var actionResult = await controller.PostItemAsync(basketMock);
                var returnedObject = (actionResult as ObjectResult).Value as Basket.API.Models.Basket;
                Assert.Equal(201, (actionResult as ObjectResult).StatusCode);
                Assert.Equal(4, returnedObject.Id);
                Assert.Equal(10, returnedObject.User);
                Assert.Equal(BasketStates.PENDING, returnedObject.State);
            }
        }

        [Fact]
        public async Task Should_Not_Create_Basket_If_It_Does_Exists()
        {
            using (var context = await MockDbContext.GetDbContext())
            {
                controller = new BasketsController(context);
                var basketMock = new Basket.API.Models.Basket
                {
                    User = 3
                };
                var actionResult = await controller.PostItemAsync(basketMock) as ObjectResult;
                Assert.Equal((int)HttpStatusCode.BadRequest, (actionResult as ObjectResult).StatusCode);
            }

        }

        [Fact]
        public async Task Should_Update_Basket()
        {
            using (var context = await MockDbContext.GetDbContext())
            {
                controller = new BasketsController(context);
                var basketMock = context.Find<Basket.API.Models.Basket>(1);
                basketMock.State = BasketStates.SENT;
                var actionResult = await controller.PutItemAsync(basketMock, 1) as NoContentResult;

                Assert.Equal((int)HttpStatusCode.NoContent, actionResult.StatusCode);
            }

        }

        [Fact]
        public async Task Should_Update_Bookings_When_Basket_Validated()
        {
            using (var context = await MockDbContext.GetDbContext())
            {
                controller = new BasketsController(context);
                var basketMock = context.Find<Basket.API.Models.Basket>(2);
                basketMock.State = BasketStates.VALIDATED;
                var actionResult = await controller.PutItemAsync(basketMock, 2);
                var basketBookings = context.Set<Booking>().Where(x => x.BasketID == basketMock.Id && x.DeletedAt == null).ToList();
                Assert.Empty(basketBookings);
            }

        }

        [Fact]
        public async Task Should_Return_NotFound_If_Basket_Doesnt_Exists()
        {
            using (var context = await MockDbContext.GetDbContext())
            {
                controller = new BasketsController(context);
                var actionResult = await controller.DeleteItemAsync(15);
                Assert.Equal((int)HttpStatusCode.NotFound, (actionResult as NotFoundResult).StatusCode);
            }

        }

        [Fact]
        public async Task Should_Delete_Basket_And_Related_Bookings()
        {
            using (var context = await MockDbContext.GetDbContext())
            {
                controller = new BasketsController(context);
                var actionResult = await controller.DeleteItemAsync(2);
                var basketBookings = context.Set<Booking>().Where(x => x.BasketID == 2 && x.DeletedAt == null).ToList();
                Assert.Equal((int)HttpStatusCode.NoContent, (actionResult as NoContentResult).StatusCode);
                Assert.Empty(basketBookings);
            }

        }
    }
}
