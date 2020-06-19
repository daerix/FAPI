using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Catalog.API.Data;
using Catalog.API.Models;
using ApiLibrary.Core.Controllers;
using ApiLibrary.Core.Attributes;

namespace Catalog.API.Controllers
{

    [ApiVersion("1")]
    //[MaxPagination(10)]
    public class GenresController : BaseController<Category, int, CatalogDbContext>
    {
        public override int AcceptRange { get; set; } = 10;

        public GenresController(CatalogDbContext context) : base(context)
        {
        }

       

    }
}
