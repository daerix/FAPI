using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using ApiLibrary.Core.Controllers;
using ApiLibrary.Core.Entities;
using ApiLibrary.Core.Extensions;
using Catalog.API.Data;
using Catalog.API.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;


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
