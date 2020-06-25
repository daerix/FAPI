using ApiLibrary.Core.Controllers;
using Catalog.API.Data;
using Catalog.API.Models;
using Microsoft.AspNetCore.Mvc;


namespace Catalog.API.Controllers
{

    [ApiVersion("1")]
    public class ActorsController : BaseController<Actor, int, CatalogDbContext>
    {
        public ActorsController(CatalogDbContext db) : base(db)
        {
        }

    }
}
