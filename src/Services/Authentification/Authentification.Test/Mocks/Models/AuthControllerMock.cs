
using Authentification.API.Models;
using Authentification.API.Controllers;
using Microsoft.Extensions.Configuration;

namespace Authentification.Test.Mocks.Models
{
    class AuthControllerMock : AuthController
    {
        public AuthControllerMock(IConfiguration config, MockDbContext db): base(config, db)
        {
        }
    }
}
