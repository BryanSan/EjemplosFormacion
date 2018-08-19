using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Threading.Tasks;

namespace EjemplosFormacion.WebApi.Authentication.BearerToken
{
    public class TestAuthRepository : IDisposable
    {
        private readonly TestAuthContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        public TestAuthRepository()
        {
            _context = new TestAuthContext();
            _userManager = new UserManager<IdentityUser>(new UserStore<IdentityUser>(_context));
        }

        public async Task<IdentityResult> RegisterUser(TestUserModel userModel)
        {
            IdentityUser user = new IdentityUser
            {
                UserName = userModel.UserName
            };

            var result = await _userManager.CreateAsync(user, userModel.Password);

            return result;
        }

        public async Task<IdentityUser> FindUser(string userName, string password)
        {
            IdentityUser user = await _userManager.FindAsync(userName, password);

            return user;
        }

        public void Dispose()
        {
            _context.Dispose();
            _userManager.Dispose();
        }
    }
}