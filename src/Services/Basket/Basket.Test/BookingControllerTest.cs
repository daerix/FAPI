using Basket.API.Controllers;
using Basket.API.Models;
using Basket.Test.Mocks;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Basket.Test
{
    public class BookingControllerTest
    {
        //private MockDbContext _db = MockDbContext.GetDbContext();
        //private BookingsController controller;

        //public BookingControllerTest()
        //{
        //    controller = new BookingsController(_db);
        //}

        //[Fact]
        //public async Task Should_Not_Create_If_Basket_Does_Not_Exists()
        //{
        //    var bookingMock = new Booking()
        //    {
        //        BasketID = 4
        //    };
        //    var actionResult = await controller.PostItemAsync(bookingMock);
        //    Assert.Equal((int)HttpStatusCode.BadRequest, (actionResult as ObjectResult).StatusCode);

        //}

        //[Fact]
        //public async Task Should_Not_Create_If_Booking_Already_Exists()
        //{
        //    var dateNow = DateTime.Now;
        //    var booking = _db.Set<Booking>().FirstOrDefault(x => x.Id == 1);
        //    booking.BookingDate = dateNow;
        //    _db.Update(booking);
        //    await _db.SaveChangesAsync();
        //    var bookingMock = new Booking
        //    {
        //        Id = 1,
        //        BookingDate = dateNow,
        //        ProductID = 1
        //    };
        //    var actionResult = await controller.PostItemAsync(bookingMock);
        //    Assert.Equal((int)HttpStatusCode.BadRequest, (actionResult as ObjectResult).StatusCode);

        //}

        //[Fact]
        //public async Task Should_Create_Booking()
        //{
        //    var bookingMock = new Booking
        //    {
        //        Id = 4,
        //        ProductID = 3,
        //        BasketID = 1
        //    };
        //    var actionResult = await controller.PostItemAsync(bookingMock);
        //    Assert.Equal((int)HttpStatusCode.Created, (actionResult as ObjectResult).StatusCode);

        //}
    }
}
