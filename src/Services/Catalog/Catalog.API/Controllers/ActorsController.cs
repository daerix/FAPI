using ApiLibrary.Core.Controllers;
using Catalog.API.Data;
using Catalog.API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;


namespace Catalog.API.Controllers
{
    [Authorize]
    [ApiVersion("1")]
    public class ActorsController : BaseController<Actor, int, CatalogDbContext>
    {
        public ActorsController(CatalogDbContext db) : base(db)
        {
        }

    }
}
