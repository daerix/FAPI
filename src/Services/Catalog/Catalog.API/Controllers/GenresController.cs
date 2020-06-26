using ApiLibrary.Core.Controllers;
using Catalog.API.Data;
using Catalog.API.Models;
using Microsoft.AspNetCore.Mvc;

namespace Catalog.API.Controllers
{
    [Authorize]
    [ApiVersion("1")]
    public class GenresController : BaseController<Genre, int, CatalogDbContext>
    {

        public GenresController(CatalogDbContext context) : base(context)
        {
        }



    }
}
