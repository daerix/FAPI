using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using ApiLibrary.Core.Controllers;
using ApiLibrary.Core.Models;
using AuthentificationAPI.Data;
using AuthentificationAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace AuthentificationAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TokenController : BaseController<User, int, UserDbContext>
    {

       public TokenController(UserDbContext context ) : base(context)
        {
        }

        public override Task<ActionResult> DeleteItemAsync([FromRoute] int id)
        {
            return base.DeleteItemAsync(id);
        }

        public override Task<ActionResult> GetItemByIdAsync([FromRoute] object id)
        {
            return base.GetItemByIdAsync(id);
        }

        public override Task<ActionResult> GetItemsAsync([FromQuery] Dictionary<string, string> param)
        {
            return base.GetItemsAsync(param);
        }

        public override Task<ActionResult> PostItemAsync([FromBody] User item)
        {
            return base.PostItemAsync(item);
        }

        public override Task<ActionResult> PutItemAsync([FromBody] User item, [FromRoute] int id)
        {
            return base.PutItemAsync(item, id);
        }
    }
}
