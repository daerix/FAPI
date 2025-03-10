﻿using ApiLibrary.Core.Controllers;
using Basket.API.Data;
using Basket.API.Models;
using Mailjet.Client;
using Mailjet.Client.Resources;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;
using Newtonsoft.Json.Linq;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Threading.Tasks;
using System.Timers;

namespace Basket.API.Controllers
{
    [Authorize]
    [ApiVersion("1")]
    public class BasketsController : BaseController<Models.Basket, int, BasketDbContext>
    {
        private Timer aTimer;

        public BasketsController(BasketDbContext db) : base(db)
        {
            //SetTimer();
        }

        [Authorize]
        public override async Task<ActionResult> PostItemAsync([FromBody] Models.Basket basket)
        {
            try
            {

                if (ModelState.IsValid || basket.User == 0)
                {
                    basket.State = Enums.BasketStates.PENDING;
                    var basketAlreadyExists = _db.Set<Models.Basket>().Where(x => x.User == basket.User && x.DeletedAt == null).ToList();
                    if (basketAlreadyExists.Count() == 0)
                    {
                        return await base.PostItemAsync(basket);
                    }
                    return BadRequest(ModelState);
                }
                return BadRequest(ModelState);
            }
            catch (Exception e)
            {
                return BadRequest(e);
            }
        }
        [Authorize]
        public override async Task<ActionResult> PutItemAsync([FromBody] Models.Basket basket, [FromRoute] int id)
        {
            if (id != basket.Id)
            {
                return BadRequest();
            }

            if (ModelState.IsValid)
            {
                _db.Update<Models.Basket>(basket);
                if (basket.State == Enums.BasketStates.VALIDATED)
                {
                    var price = (decimal)0;
                    var bookingToDelete = _db.Set<Booking>().Where(x => x.BasketID == basket.Id).ToList();
                    foreach (var booking in bookingToDelete)
                    {
                        price += (decimal)booking.Price;
                        _db.Remove(booking);
                    }
                    _db.Remove(basket);
                    var token = GetToken();
                    var email = GetClaim(token, "Mail");
                    var lastname = GetClaim(token, "LastName");
                    var firstname = GetClaim(token, "FirstName");
                    SendValidationMailAsync(basket, !string.IsNullOrEmpty(email) ? email : "virgilesassano@gmail.com", !string.IsNullOrEmpty(lastname) ? lastname : "Mouloud", !string.IsNullOrEmpty(firstname) ? firstname : "Mouloud", price);
                }

                await _db.SaveChangesAsync();
                return NoContent();
            }
            else
            {
                return BadRequest(ModelState);
            }
        }
        [Authorize]
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

        private void CleanData(Object source, ElapsedEventArgs e)
        {
            //_db.Database.OpenConnection();
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

        public string GetClaim(string token, string claimType)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            if (!string.IsNullOrEmpty(token))
            {
                var securityToken = tokenHandler.ReadToken(token) as JwtSecurityToken;

                var stringClaimValue = securityToken.Claims.First(claim => claim.Type == claimType).Value;
                return stringClaimValue;
            }
            return null;
        }

        public string GetToken()
        {
            var token = "";
            if (Request != null)
            {
                token = (string)Request.Headers[HeaderNames.Authorization] as string;
            }
            token = token.Replace("Bearer ", "");
            return token;
        }

        private async void SendValidationMailAsync(Models.Basket basket, string userEmail, string firstName, string lastName, decimal totalPrice)
        {
            MailjetClient client = new MailjetClient("4f32aface96993dabdc99274cac8f363", "d4349dcf478531808acefc8d681f3ee5")
            {
                Version = Mailjet.Client.ApiVersion.V3_1,
            };
            MailjetRequest request = new MailjetRequest
            {
                Resource = Send.Resource,
            }.Property(Send.Messages, new JArray {
                new JObject {
                 {"From", new JObject {
                  {"Email", "virgilesassano@gmail.com"},
                  {"Name", "FAPI"}
                  }},
                 {"To", new JArray {
                  new JObject {
                   {"Email",userEmail},
                   {"Name", firstName + " " + lastName }
                   }
                  }},
                 {"TemplateID", 1516707},
                 {"TemplateLanguage", true},
                 {"Subject", "FAPI Validated Command"},
                 {"Variables", new JObject {
                  {"nom", "SASSANO\"]][[data: prénom:\"Virgile\"]]</ title >< !--[if !mso]>< !---->< meta http - equiv = \"X-UA-Compatible\" content = \"IE=edge\" >< !--< ![endif]-- >< meta http - equiv = \"Content-Type\" content = \"text/html; charset=UTF-8\" >< meta name = \"viewport\" content = \"width=device-width,initial-scale=1\" >< style type = \"text/css"},
{"firstname", firstName},
{"total_price", totalPrice},
{"order_date", basket.DeletedAt},
{"order_id", basket.Id}
                  }
}
                 }
                });

            MailjetResponse response = await client.PostAsync(request);
        }
    }
}