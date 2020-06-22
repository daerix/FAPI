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
            param.Add("Password", "Password123");
            param.Add("Mail", "itiome@icloud.com");
            var ok = base.GetItemsAsync(param);
            var test = _db.Users.Where(x => x.Mail == "itiome@icloud.com" && x.Password == "Password123");
            return ok;
        }
        public  Task<ActionResult> GetUser([FromQuery] Dictionary<string, string> param)
        {
            param.Add("Password", "Password123");
            var ok = 
            var ok = base.GetItemsAsync(param);
            return ok;
        }

       /* [HttpPost]
        public async Task<IActionResult> Login(User _userData)
        {

            if (_userData != null && _userData.Email != null && _userData.Password != null)
            {
                var param = new Dictionary<string, string>();
                param.Add("Password", "Password123");
                var user = await GetItemsAsync(param);

                if (user != null)
                {
                    //create claims details based on the user information
                    var claims = new[] {
                    new Claim(JwtRegisteredClaimNames.Sub, _configuration["Jwt:Subject"]),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                    new Claim(JwtRegisteredClaimNames.Iat, DateTime.UtcNow.ToString()),
                    new Claim("ID", user. .ToString()),
                    new Claim("FirstName", user.FirstName),
                    new Claim("LastName", user.LastName),
                    new Claim("UserName", user.UserName),
                    new Claim("Email", user.Email)
                   };

                    var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));

                    var signIn = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

                    var token = new JwtSecurityToken(_configuration["Jwt:Issuer"], _configuration["Jwt:Audience"], claims, expires: DateTime.UtcNow.AddDays(1), signingCredentials: signIn);

                    return Ok(new JwtSecurityTokenHandler().WriteToken(token));
                }
                else
                {
                    return BadRequest("Invalid credentials");
                }
            }
            else
            {
                return BadRequest();
            }
        } */

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
