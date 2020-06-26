using ApiLibrary.Core.Controllers;
using ApiLibrary.Core.Models;
using Catalog.API.Data;
using Catalog.API.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Catalog.API.Controllers
{
    [ApiVersion("1")]
    public class ActorsController : BaseController<Actor, int, CatalogDbContext>
    {
        public ActorsController(CatalogDbContext db) : base(db)
        {
        }

        [Authorize]
        public override Task<ActionResult> DeleteItemAsync([FromRoute] int id)
        {
            return base.DeleteItemAsync(id);
        }

        public override Task<ActionResult<Actor>> GetItemByIdAsync([FromRoute] int id, [FromQuery] bool deepFetch = false)
        {
            return base.GetItemByIdAsync(id, deepFetch);
        }

        public override Task<ActionResult<IEnumerable<Actor>>> GetItemsAsync([FromQuery] QueryParams param)
        {
            return base.GetItemsAsync(param);
        }

        [Authorize]
        public override Task<ActionResult> PostItemAsync([FromBody] Actor item)
        {
            return base.PostItemAsync(item);
        }

        [Authorize]
        public override Task<ActionResult> PutItemAsync([FromBody] Actor item, [FromRoute] int id)
        {
            return base.PutItemAsync(item, id);
        }
    }
}
