using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ApiLibrary.Core.Entities;
using Catalog.API.Data;
using Catalog.API.Models;
using ApiLibrary.Core.Controllers;
using ApiLibrary.Core.Attributes;

namespace Catalog.API.Controllers
{

    [ApiVersion("1")]
    public class GenresController : BaseController<Genre, int, CatalogDbContext>
    {

        public GenresController(CatalogDbContext context) : base(context)
        {
        }

       

    }
}
