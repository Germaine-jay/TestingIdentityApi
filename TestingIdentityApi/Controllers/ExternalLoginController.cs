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


        [HttpPost("tiktok-login")]
        public IActionResult TiktokAuth(string credential)
        {
            var res = _externalLoginService.TiktokAuth(credential);

            return Ok(res);
        }
    }
}
