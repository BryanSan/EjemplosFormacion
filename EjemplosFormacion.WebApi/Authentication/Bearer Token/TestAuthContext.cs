using Microsoft.AspNet.Identity.EntityFramework;

namespace EjemplosFormacion.WebApi.Authentication.BearerToken
{
    public class TestAuthContext : IdentityDbContext<IdentityUser>
    {
        public TestAuthContext() : base("TestAuthContext")
        {

        }
    }
}