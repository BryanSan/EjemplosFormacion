using EjemplosFormacion.WebApi.Authentication.OAuthBearerToken.Models;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Data.Entity;

namespace EjemplosFormacion.WebApi.Authentication.OAuthBearerToken
{
    public class TestAuthContext : IdentityDbContext<IdentityUser>
    {
        public TestAuthContext() : base("TestAuthContextConnection")
        {
            // Configura un Database Initializer cuando la aplicacion inicia
            // Puedes usar los Built-In Database Initializer para recrear la base de datos siempre, o recrearla cuando tu modelo cambia, o crearla si no existe
            // O puedes crear un Custom Database Initializer como es en este caso para ademas de recrear la base de datos cuando el modelo cambia o crearla si no existe
            // Tambien añadir registros por default al recrearla, usa el metodo Seed para hacer la logica de los registros default
            Database.SetInitializer<TestAuthContext>(new AuthContextDropCreateDatabaseIfModelChanges());
        }

        public DbSet<TestClientApp> ClientApps { get; set; }
        public DbSet<TestRefreshToken> RefreshTokens { get; set; }
    }
}