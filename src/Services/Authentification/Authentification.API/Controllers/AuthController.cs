using System;
using System.Collections.Generic;
using System.Data.Entity.SqlServer.Utilities;
using System.Data.Entity.Utilities;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using ApiLibrary.Core.Controllers;
using ApiLibrary.Core.Models;
using Authentification.API.Data;
using Authentification.API.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace Authentification.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : BaseController<User, int, UserDbContext>
    {
        public IConfiguration _configuration;
        public AuthController(IConfiguration config, UserDbContext context ) : base(context)
        {
            _configuration = config;
        }

        public override Task<ActionResult> DeleteItemAsync([FromRoute] int id)
        {
            return base.DeleteItemAsync(id);
        }

        public override async Task<ActionResult> GetItemByIdAsync([FromRoute] object id)
        {
            return await base.GetItemByIdAsync(id);
        }

        [Authorize]
        public override Task<ActionResult> GetItemsAsync([FromQuery] Dictionary<string, string> param)
        {
            return base.GetItemsAsync(param);
        }

        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login([FromQuery] LoginQueryParams login)
        {
            if (login.IsPassword && login.IsEmail)
            {

                var model = await _db.Users.Where(x => x.Mail == login.Email).FirstOrDefaultAsync();
                
                
                if (model != null)
                {
                    var claims = new[] {
                        new Claim(JwtRegisteredClaimNames.Sub, _configuration["Jwt:Subject"]),
                        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                        new Claim(JwtRegisteredClaimNames.Iat, DateTime.UtcNow.ToString()),
                        new Claim("ID", model.Id.ToString()),
                        new Claim("FirstName", model.FirstName),
                        new Claim("LastName", model.LastName),
                        new Claim("Mail", model.Mail)
                    };

                    var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));

                    var signIn = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

                    var token = new JwtSecurityToken(_configuration["Jwt:Issuer"], _configuration["Jwt:Audience"], claims, expires: DateTime.UtcNow.AddDays(1), signingCredentials: signIn);
                    var tokenString = new JwtSecurityTokenHandler().WriteToken(token);
                    var cookieOptions = new CookieOptions();
                    cookieOptions.Expires = DateTimeOffset.UtcNow.AddHours(4);
                    cookieOptions.Domain = Request.Host.Value;
                    cookieOptions.Path = "/";
                    HttpContext.Session.SetString("JWToken", tokenString);
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
        }
        public class LoginQueryParams
        {
            public string Email { get; set; }
            public string Password { get; set; }
            public bool IsEmail =>
                !string.IsNullOrWhiteSpace(Email);
            public bool IsPassword =>
                !string.IsNullOrWhiteSpace(Password);
        }
    }
}
