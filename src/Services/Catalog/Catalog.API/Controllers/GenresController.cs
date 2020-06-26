using System.Collections.Generic;
using System.Threading.Tasks;
using ApiLibrary.Core.Controllers;
using ApiLibrary.Core.Models;
using Catalog.API.Data;
using Catalog.API.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Catalog.API.Controllers
{

    [ApiVersion("1")]
    public class GenresController : BaseController<Genre, int, CatalogDbContext>
    {

        public GenresController(CatalogDbContext context) : base(context)
        {
        }

        [Authorize]
        public override Task<ActionResult> DeleteItemAsync([FromRoute] int id)
        {
            return base.DeleteItemAsync(id);
        }

        public override Task<ActionResult<Genre>> GetItemByIdAsync([FromRoute] int id, [FromQuery] bool deepFetch = false)
        {
            return base.GetItemByIdAsync(id, deepFetch);
        }

        public override Task<ActionResult<IEnumerable<Genre>>> GetItemsAsync([FromQuery] QueryParams param)
        {
            return base.GetItemsAsync(param);
        }

        [Authorize]
        public override Task<ActionResult> PostItemAsync([FromBody] Genre item)
        {
            return base.PostItemAsync(item);
        }

        [Authorize]
        public override Task<ActionResult> PutItemAsync([FromBody] Genre item, [FromRoute] int id)
        {
            return base.PutItemAsync(item, id);
        }
    }
}
