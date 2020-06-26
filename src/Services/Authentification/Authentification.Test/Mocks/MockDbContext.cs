using Authentification.API.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;

namespace Authentification.Test.Mocks
{
    class MockDbContext : UserDbContext
    {
        public MockDbContext(DbContextOptions options) : base(options)
        {
        }

        public async static Task<MockDbContext> GetDbContext(bool withData = true)
        {
            var options = new DbContextOptionsBuilder().UseInMemoryDatabase(Guid.NewGuid().ToString()).Options;
            var db = new MockDbContext(options);

            if (withData)
            {
                db.Users.Add(new UserMock { Id = 1, FirstName = "FirstNameTest", LastName = "LastNameTest", Mail = "MailTest@icloud.com", Password = "PasswordTest123" });
                db.Users.Add(new UserMock { Id = 2, FirstName = "FirstNameTest", LastName = "LastNameTest", Mail = "MailTest2@icloud.com", Password = "PasswordTest123" });
                db.Users.Add(new UserMock { Id = 3, FirstName = "FirstNameTest", LastName = "LastNameTest", Mail = "MailTest3@icloud.com", Password = "PasswordTest123" });
                await db.SaveChangesAsync();
            }

            return db;
        }
    }
}
