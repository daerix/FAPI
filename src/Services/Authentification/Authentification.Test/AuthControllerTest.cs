using Authentification.API.Controllers;
using Authentification.API.Models;
using Authentification.Test.Mocks;
using Authentification.Test.Mocks.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Xunit;

namespace Authentification.Test
{
    public class AuthControllerTest
    {

        private AuthControllerMock controller;
        public IConfiguration _configuration;

        public AuthControllerTest()
        {
            var myConfiguration = new Dictionary<string, string> {
                {"Jwt:Key", "secretKeyButNeedToBeStronger"},
                {"Jwt:Issuer", "FAPIAuthentificationAPI"},
                {"Jwt:Audience", "FAPIAuthentificationPostmanClient"},
                {"Jwt:Subject", "FAPIAuthentificationAccessToken"}
            };

            _configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(myConfiguration)
            .Build();
        }

        [Fact]
        public async Task Should_Not_Create_If_Password_Null()
        {
            using (var context = await MockDbContext.GetDbContext())
            {
                controller = new AuthControllerMock(_configuration, context);
                var userMock = new Authentification.API.Models.User
                {
                    Mail = "MailTest@icloud.com"
                };
                var actionResult = await controller.Logon(userMock);
                Assert.IsType<BadRequestResult>(actionResult);
            }
        }

        [Fact]
        public async Task Should_Not_Create_If_Mail_Null()
        {
            using (var context = await MockDbContext.GetDbContext())
            {
                controller = new AuthControllerMock(_configuration, context);
                var userMock = new Authentification.API.Models.User
                {
                    Password = "Password"
                };
                var actionResult = await controller.Logon(userMock);
                Assert.IsType<BadRequestResult>(actionResult);
            }
        }

        [Fact]
        public async Task Should_Not_Create_If_User_Exist()
        {
            using (var context = await MockDbContext.GetDbContext())
            {
                controller = new AuthControllerMock(_configuration, context);
                var userMock = new Authentification.API.Models.User
                {
                    Mail = "MailTest@icloud.com",
                    Password = "Password"
                };
                var actionResult = await controller.Logon(userMock);
                Assert.IsType<BadRequestObjectResult>(actionResult);
            }
        }

        [Fact]
        public async Task Should_Create_User()
        {
            using (var context = await MockDbContext.GetDbContext())
            {
                controller = new AuthControllerMock(_configuration, context);
                var userMock = new Authentification.API.Models.User
                {
                    Mail = "new@icloud.com",
                    Password = "Password123", 
                };
                var actionResult = await controller.Logon(userMock);
                Assert.IsType<OkObjectResult>(actionResult);
            }
        }
    }
}
