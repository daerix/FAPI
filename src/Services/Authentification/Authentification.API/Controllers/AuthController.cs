﻿using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity.SqlServer.Utilities;
using System.Data.Entity.Utilities;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using ApiLibrary.Core.Controllers;
using ApiLibrary.Core.Models;
using Authentification.API.Data;
using Authentification.API.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace Authentification.API.Controllers
{
    [ApiVersion("1")]
    public class AuthController : BaseController<User, int, UserDbContext>
    {
        public IConfiguration _configuration;

        public AuthController(IConfiguration config, UserDbContext db) : base(db)
        {
            _configuration = config;
        }

        public override Task<ActionResult> DeleteItemAsync([FromRoute] int id)
        {
            return base.DeleteItemAsync(id);
        }

        public override async Task<ActionResult<User>> GetItemByIdAsync([FromRoute] int id, [FromQuery] bool deepFetch = false)
        {
            return await base.GetItemByIdAsync(id);
        }

        public override Task<ActionResult> PostItemAsync([FromBody] User item)
        {
            return base.PostItemAsync(item);
        }

        [Authorize]
        public override Task<ActionResult<IEnumerable<User>>> GetItemsAsync([FromQuery] Dictionary<string, string> param)
        {
            return base.GetItemsAsync(param);
        }

        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login([FromBody] User user)
        {
            if (!string.IsNullOrWhiteSpace(user.Mail) && !string.IsNullOrWhiteSpace(user.Password))
            {

                var model = await _db.Users.Where(x => x.Mail == user.Mail).FirstOrDefaultAsync();
                
                
                if (model != null)
                {
                    if (model.Password != HashPassword(user.Password))
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
                        return Ok(new JwtSecurityTokenHandler().WriteToken(token));
                    }
                    return BadRequest("Invalid credentials");
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

        [HttpPost]
        [Route("logon")]
        public async Task<IActionResult> Logon([FromBody] User user)
        {
            if (!string.IsNullOrWhiteSpace(user.Mail) && !string.IsNullOrWhiteSpace(user.Password))
            {

                var model = await _db.Users.Where(x => x.Mail == user.Mail).FirstOrDefaultAsync();


                if (model == null)
                {
                    user.Password = HashPassword(user.Password);
                    var userCreated = (await PostItemAsync(user) as CreatedResult).Value as User;
                    if (userCreated != null)
                    {
                        var claims = new[] {
                        new Claim(JwtRegisteredClaimNames.Sub, _configuration["Jwt:Subject"]),
                        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                        new Claim(JwtRegisteredClaimNames.Iat, DateTime.UtcNow.ToString()),
                        new Claim("ID", userCreated.Id.ToString()),
                        new Claim("FirstName", userCreated.FirstName),
                        new Claim("LastName", userCreated.LastName),
                        new Claim("Mail", userCreated.Mail)
                        };

                        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));

                        var signIn = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

                        var token = new JwtSecurityToken(_configuration["Jwt:Issuer"], _configuration["Jwt:Audience"], claims, expires: DateTime.UtcNow.AddDays(1), signingCredentials: signIn);
                        var tokenString = new JwtSecurityTokenHandler().WriteToken(token);
                        return Ok(new JwtSecurityTokenHandler().WriteToken(token));
                    }
                    return BadRequest("Erreur");
                }
                else
                {
                    return BadRequest("Mail déjà utilisé");
                }
            }
            else
            {
                return BadRequest();
            }
        }

        public string HashPassword(string Password)
        {
            byte[] salt = new byte[128 / 8];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(salt);
            }
            string hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                password: Password,
                salt: salt,
                prf: KeyDerivationPrf.HMACSHA1,
                iterationCount: 10000,
                numBytesRequested: 256 / 8
            ));
            return hashed;
        }

        public class LogQueryParams
        {
            public string Email { get; set; }
            public string Password { get; set; }
            public string LastName { get; set; }
            public string FirstName { get; set; }
            public bool IsEmail =>
                !string.IsNullOrWhiteSpace(Email);
            public bool IsPassword =>
                !string.IsNullOrWhiteSpace(Password);
        }
    }
}
