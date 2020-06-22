using ApiLibrary.Core.Controllers;
using Basket.API.Data;
using Basket.API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;

namespace Basket.API.Controllers
{
    [ApiVersion("1")]
    public class BasketsController : BaseController<Models.Basket, int, BasketDbContext>
    {
        private System.Timers.Timer aTimer;

        public BasketsController(BasketDbContext db) : base(db)
        {
            SetTimer();
        }

        public override async Task<ActionResult> PostItemAsync([FromBody] Models.Basket basket)
        {
            try
            {
                if (ModelState.IsValid || basket.User == 0)
                {
                    basket.State = Enums.BasketStates.PENDING;
                    var basketAlreadyExists = _db.Set<Models.Basket>().Where(x => x.User == basket.User && x.DeletedAt == null);
                    if (basketAlreadyExists.Count() == 0)
                    {
                        return await base.PostItemAsync(basket);
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

        public override async Task<ActionResult> PutItemAsync([FromBody] Models.Basket basket, [FromRoute] int id)
        {
            if (id.Equals(basket.Id))
            {
                return BadRequest();
            }

            if (ModelState.IsValid)
            {
                _db.Set<Models.Basket>().Update(basket);
                if (basket.State == Enums.BasketStates.VALIDATED)
                {
                    var bookingToDelete = _db.Set<Booking>().Where(x => x.BasketID == basket.Id).ToList();
                    foreach (var booking in bookingToDelete)
                    {
                        _db.Remove(booking);
                    }
                    _db.Remove(basket);
                }
                await _db.SaveChangesAsync();
                return NoContent();
            }
            else
            {
                return BadRequest(ModelState);
            }
        }

        public override async Task<ActionResult> DeleteItemAsync([FromRoute] int id)
        {
            var basket = await _db.Baskets.FindAsync(id);
            if (basket == null)
            {
                return NotFound();
            }
            _db.Remove(basket);
            var linkedBooking = _db.Set<Booking>().Where(x => x.BasketID == id && x.DeletedAt == null).ToList();
            foreach (var booking in linkedBooking)
            {
                _db.Remove(booking);
            }
            await _db.SaveChangesAsync();
            return NoContent();
        }


        private void SetTimer()
        {
            aTimer = new System.Timers.Timer(10000);
            aTimer.Elapsed += CleanData;
            aTimer.Enabled = true;
        }

        private async void CleanData(Object source, ElapsedEventArgs e)
        {
            //aTimer.Stop();
            //var dateTimeNow = DateTime.Now;
            //var dateTimeMinusElapsedTime = dateTimeNow.AddMinutes(-1);
            //var basketToDelete = _db.Set<Models.Basket>().Where(x => x.DeletedAt == null && x.UpdatedAt < dateTimeMinusElapsedTime).ToList();
            //foreach (var basket in basketToDelete)
            //{
            //    await DeleteItemAsync(basket.Id);
            //}
            //aTimer.Start();
        }
    }
}