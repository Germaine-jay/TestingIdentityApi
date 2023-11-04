using Microsoft.AspNetCore.Mvc;
using TestingIdentityApi.Services;

namespace TestingIdentityApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RedisController : ControllerBase
    {
        private readonly IRedisService _redisService;
        public string Id;

        public RedisController(IRedisService redisService)
        {
            _redisService = redisService;
            Id = Guid.NewGuid().ToString();
        }


        [HttpGet]
        public async Task<IActionResult> GetCartAsync(string UserId)
        {
            //var userId = Guid.NewGuid().ToString();

            var res = await _redisService.GetCartAsync(UserId);
            if (res != null)
                return Ok(res);

            //var resp = await _redisService.GetCartAsync(UserId);
            return BadRequest(res);
        }


        [HttpPost]
        public async Task<IActionResult> AddItemToCartAsync([FromBody] CartItem cartItem)
        {
            var cart = await _redisService.GetCartAsync("00000000-0000-0000-0000-000000000000");
            var res = await _redisService.SaveCartAsync(cart, cartItem);

            if (res)
            {
                return Ok(cartItem);
            }

            return BadRequest(res);
        }


        [HttpPost("Delete")]
        public async Task<IActionResult> DeleteCartAsync(string userid)
        {
            var res = await _redisService.ClearFromCache(userid);
            if(res)
                return Ok(res);

            return BadRequest(res);
        }


        [HttpPost("remove")]
        public async Task<IActionResult> DeleteCartItemAsync(string productId, string userid)
        {
            var res = await _redisService.RemoveCartItemAsync(productId, userid);
            if (res)
            {
                return Ok(res);
            }

            return BadRequest(res);
        }
    }
}
