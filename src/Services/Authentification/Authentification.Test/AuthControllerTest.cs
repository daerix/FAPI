using Authentification.API.Controllers;
using Authentification.API.Models;
using Authentification.Test.Mocks;
using System;
using System.Data.Entity.Core.Objects;
using System.Net;
using System.Threading.Tasks;
using Xunit;

namespace Authentification.Test
{
    public class AuthControllerTest
    {

        private AuthController controller;
        [Fact]
        public async Task Should_Not_Create_If_Basket_Does_Not_Exists()
        {
            /*using (var context = await MockDbContext.GetDbContext())
            {
                controller = new AuthController(context);
                var bookingMock = new User()
                {
                    Id = 4
                };
                var actionResult = await controller.PostItemAsync(bookingMock);
                Assert.Equal((int)HttpStatusCode.BadRequest, (actionResult as ObjectResult).StatusCode);
            }*/
        }
    }
}
