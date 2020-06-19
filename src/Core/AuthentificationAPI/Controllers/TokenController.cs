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
        public override int AcceptRange { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

       public TokenController(UserDbContext context ) : base(context)
        {

        }

        public override Task<ActionResult<User>> GetItemByIdAsync([FromRoute] int id)
        {
            return base.GetItemByIdAsync(id);
        }

        public override Task<ActionResult<IEnumerable<User>>> GetItemsAsync([FromQuery] QueryParams param)
        {
            return base.GetItemsAsync(param);
        }

        public override Task<ActionResult> PostProduct([FromBody] User item)
        {
            return base.PostProduct(item);
        }

        public override Task<ActionResult> PutProduct([FromBody] User item, [FromRoute] int id)
        {
            return base.PutProduct(item, id);
        }

        public override Task<ActionResult> DeleteItemAsync([FromRoute] int id)
        {
            return base.DeleteItemAsync(id);
        }
    }
}
