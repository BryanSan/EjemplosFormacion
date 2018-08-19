using EjemplosFormacion.WebApi.Authentication.BearerToken;
using EjemplosFormacion.WebApi.Authentication.BearerToken.Models;
using Microsoft.AspNet.Identity;
using System.Threading.Tasks;
using System.Web.Http;

namespace EjemplosFormacion.WebApi.Controllers.TestAuthentication
{
    public class TestBearerTokenController : ApiController
    {

        private readonly TestAuthRepository _repo;

        public TestBearerTokenController()
        {
            _repo = new TestAuthRepository();
        }

        public async Task<IHttpActionResult> RegisterUser(TestUserModel userModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            IdentityResult result = await _repo.RegisterUser(userModel);

            IHttpActionResult errorResult = GetErrorResult(result);

            if (errorResult != null)
            {
                return errorResult;
            }

            return Ok();
        }

        [Authorize]
        public IHttpActionResult TestAuthenticate()
        {
            return Ok();
        }

        public async Task<IHttpActionResult> DeleteRefreshToken(string tokenId)
        {
            var result = await _repo.RemoveRefreshToken(tokenId);
            if (result)
            {
                return Ok();
            }

            return BadRequest("Token Id does not exist");
        }

        private IHttpActionResult GetErrorResult(IdentityResult result)
        {
            if (result == null)
            {
                return InternalServerError();
            }

            if (!result.Succeeded)
            {
                if (result.Errors != null)
                {
                    foreach (string error in result.Errors)
                    {
                        ModelState.AddModelError("", error);
                    }
                }

                if (ModelState.IsValid)
                {
                    // No ModelState errors are available to send, so just return an empty BadRequest.
                    return BadRequest();
                }

                return BadRequest(ModelState);
            }

            return null;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _repo.Dispose();
            }

            base.Dispose(disposing);
        }
    }
}
