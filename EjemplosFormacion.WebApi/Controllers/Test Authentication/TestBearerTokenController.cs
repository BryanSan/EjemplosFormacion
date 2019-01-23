using EjemplosFormacion.HelperClasess.CriptographyHelpers.Abstract;
using EjemplosFormacion.WebApi.Authentication.OAuthBearerToken;
using EjemplosFormacion.WebApi.Authentication.OAuthBearerToken.Models;
using Microsoft.AspNet.Identity;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web.Http;

namespace EjemplosFormacion.WebApi.Controllers.TestAuthentication
{
    public class TestBearerTokenController : ApiController
    {

        readonly TestAuthRepository _repo;
        readonly ITokenGeneratorWithSymmetricKey _tokenGeneratorWithSymmetricKey;
        readonly ITokenValidatorWithSymmetricKey _tokenValidatorWithSymmetricKey;

        public TestBearerTokenController(ITokenGeneratorWithSymmetricKey tokenGeneratorWithSymmetricKey, ITokenValidatorWithSymmetricKey tokenValidatorWithSymmetricKey)
        {
            _repo = new TestAuthRepository();
            _tokenGeneratorWithSymmetricKey = tokenGeneratorWithSymmetricKey;
            _tokenValidatorWithSymmetricKey = tokenValidatorWithSymmetricKey;
        }

        public async Task<IHttpActionResult> RegisterUser(TestUserModel userModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            IdentityResult result = await _repo.RegisterUser(userModel);
            if (result.Succeeded)
            {
                return Ok();
            }
            else
            {
                IHttpActionResult errorResult = GetErrorResult(result);
                return errorResult;
            }
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

        [HttpGet]
        public IHttpActionResult GetToken(string userName)
        {
            string token = _tokenGeneratorWithSymmetricKey.GenerateTokenJwt(new List<Claim> { new Claim(ClaimTypes.Name, userName) });
            return Ok(token);
        }

        [HttpGet]
        public IHttpActionResult ValidateToken(string token)
        {
            (bool isValid, ClaimsPrincipal claimsPrincipal) = _tokenValidatorWithSymmetricKey.ValidateToken(token);
            return Ok(claimsPrincipal);
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
