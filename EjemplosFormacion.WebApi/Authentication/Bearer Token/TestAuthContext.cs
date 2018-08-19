using EjemplosFormacion.WebApi.Authentication.BearerToken.Models;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Data.Entity;

namespace EjemplosFormacion.WebApi.Authentication.BearerToken
{
    public class TestAuthContext : IdentityDbContext<IdentityUser>
    {
        public TestAuthContext() : base("TestAuthContext")
        {

        }

        public DbSet<TestClientApp> ClientApps { get; set; }
        public DbSet<TestRefreshToken> RefreshTokens { get; set; }
    }
}