using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using ApiLibrary.Core.Controllers;
using ApiLibrary.Core.Entity;
using ApiLibrary.Core.Extensions;
using Catalog.API.Data;
using Catalog.API.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;


namespace Catalog.API.Controllers
{
    
    [ApiVersion("1")]
    public class ProductsController : BaseController<Product, int, CatalogDbContext>
    {
        public override int AcceptRange { get; set; } = 25;

        public ProductsController(CatalogDbContext db) : base(db)
        {
        }

        










        /*[HttpGet]
        [ProducesResponseType(typeof(IEnumerable<Product>), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ApiVersion("1", Deprecated = true)]
        [ApiVersion("2")]
        public async Task<ActionResult> GetProducts(ApiVersion version)
        {
            IEnumerable<Product> result = null;
            if(version.MajorVersion == 2)
            {
                result = new List<Product>();
            }
            return Ok(result);
        }*/

        /* [HttpGet]
         [ProducesResponseType(typeof(IEnumerable<Product>), (int)HttpStatusCode.OK)]
         [ProducesResponseType((int)HttpStatusCode.BadRequest)]
         [ProducesResponseType((int)HttpStatusCode.NotFound)]
         [ApiVersion("2")]
         public async Task<ActionResult> GetProducts_V2()
         {
             return Ok(null);
         }*/
    }
}
