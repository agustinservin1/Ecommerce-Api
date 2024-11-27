using Application.Interfaces;
using Application.Models.Request;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;

namespace Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : Controller
    {
        private readonly IAuthenticationServiceApi _authenticateService;
        public AuthenticationController(IAuthenticationServiceApi authenticateService)
        {
            _authenticateService = authenticateService;
        }

        [HttpPost]
        public IActionResult AuthenticateUser([FromBody] CredentialRequest request)
        {
            string token = _authenticateService.AuthenticateCredentials(request);

            if (token is not null)
            {
                return (Ok(token));
            }

            return Unauthorized();
        }
    }
}