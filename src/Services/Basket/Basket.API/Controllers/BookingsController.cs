using ApiLibrary.Core.Controllers;
using Basket.API.Data;
using Basket.API.Models;
using Microsoft.AspNetCore.Mvc;
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
            try
            {
                if (ModelState.IsValid)
                {

                    var basketExists = _db.Set<Models.Basket>().FirstOrDefault(x => x.Id == book.BasketID && x.DeletedAt == null);
                    var bookingAlreadyExists = _db.Set<Booking>().FirstOrDefault(x => x.ProductID == book.ProductID && x.BookingDate == book.BookingDate && x.DeletedAt == null);
                    if (basketExists != null && bookingAlreadyExists == null)
                    {
                        var createdBooking = _db.Set<Booking>().Add(book);
                        await _db.SaveChangesAsync();
                        return Created("", createdBooking.Entity);
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
