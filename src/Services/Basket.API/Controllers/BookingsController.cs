using ApiLibrary.Core.Controllers;
using Basket.API.Data;
using Basket.API.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Basket.API.Controllers
{
    [ApiVersion("1")]
    public class BookingsController : BaseController<Booking, int, BasketDbContext>
    {

        public BookingsController(BasketDbContext db) : base(db)
        {
        }


        public override async Task<ActionResult> PostItemAsync([FromBody] Booking book)
        {
            using (var transaction = _db.Database.BeginTransaction())
            {
                try
                {
                    if (ModelState.IsValid)
                    {

                        var basketExists = _db.Set<Models.Basket>().FirstOrDefault(x => x.Id == book.BasketID && x.DeletedAt == null);
                        var bookingAlreadyExists = _db.Set<Booking>().FirstOrDefault(x => x.ProductID == book.ProductID && x.BookingDate == book.BookingDate);
                        if (basketExists != null && bookingAlreadyExists == null)
                        {
                            var returnedValue = await base.PostItemAsync(book);
                            transaction.Commit();
                            return returnedValue;
                        }
                        return BadRequest(ModelState);
                    }
                    return BadRequest(ModelState);

                }
                catch
                {
                    return BadRequest();
                }
            }
        }
    }
}
