using ApiLibrary.Core.Controllers;
using Basket.Data;
using Microsoft.AspNetCore.Mvc;

namespace Basket.Controllers
{

    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    [ApiVersion("1")]
    public class BasketsController : BaseController<Models.Basket, int, BasketDbContext>
    {
        public override int AcceptRange { get; set; } = 25;
        public BasketsController(BasketDbContext db) : base(db)
        {
        }
    }
}