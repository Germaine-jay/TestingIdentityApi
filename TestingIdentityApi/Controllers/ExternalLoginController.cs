using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.MicrosoftAccount;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using TestingIdentityApi.Services;

namespace TestingIdentityApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ExternalLoginController : Controller
    {
    private readonly IExternalLogin _externalLoginService;

        public ExternalLoginController(IExternalLogin externalLoginService)
        {
            _externalLoginService = externalLoginService;
        }

        [HttpPost("microsoft-login")]
        public async Task<IActionResult> MicrosoftAuth(string credential)
        {
            var res = _externalLoginService.MicrosoftAuth(credential);

            return Ok(res);
        }

        [HttpPost("microsoft-login2")]
        public async Task<IActionResult> Microsoft2Auth(string credential)
        {
            var res = _externalLoginService.GetMicrosoftAccountUserInfoAsync(credential);

            return Ok(res);
        }


        [EnableCors("CorsPolicy")]
        [HttpGet("microsoft")]
        public IActionResult MicrosoftLogin()
        {
            var properties = new AuthenticationProperties
            {
                RedirectUri = "/api/ExternalLogin/microsoft-callback",
            };
            return Challenge(properties, MicrosoftAccountDefaults.AuthenticationScheme);
        }


        //[EnableCors("CorsPolicy")]
        [HttpPost("microsoft-callback")]
        public async Task<IActionResult> MicrosoftCallback()
        {
            var result = await HttpContext.AuthenticateAsync(MicrosoftAccountDefaults.AuthenticationScheme);
            var principal = HttpContext.User;
            // You can access user information from the `principal` object.
            // For example, principal.FindFirst(ClaimTypes.Email).Value
            if (result.Succeeded)
            {

            // Add your logic here, such as creating a user in your system or issuing a JWT token.

                // Successful authentication - you can access user information from result.Principal
                var user = result.Principal;

                // Redirect or respond as needed
                return Ok($"Successfully authenticated. Principal == {principal}\n\n result == {result}");
            }
            return BadRequest(result);
        }



        [HttpPost("tiktok-login")]
        public IActionResult TiktokAuth(string credential)
        {
            var res = _externalLoginService.TiktokAuth(credential);

            return Ok(res);
        }
    }
}
