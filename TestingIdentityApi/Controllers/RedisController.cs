using Microsoft.AspNetCore.Mvc;
using System.Linq;
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
        public async Task<Cart> GetCartAsync(string UserId)
        {
            var userId = Guid.NewGuid().ToString();
            if (UserId == null)
            {
                return await _redisService.GetCartAsync(userId);
            }
            return await _redisService.GetCartAsync(UserId);
        }


        [HttpPost]
        public async Task AddItemToCartAsync([FromBody] CartItem cartItem)
        {
            var total = 0;
            var cart = await _redisService.GetCartAsync("00000000-0000-0000-0000-000000000000");
            
                cart.CartItems.Add(cartItem);
                await _redisService.SaveCartAsync(cart);
            Console.WriteLine(cart.Id);
            foreach(var item in cart.CartItems)
            {
                total += (item.Quantity * item.Price);
                cart.Total = total;
                await _redisService.SaveCartAsync(cart);
            }
                 BadRequest(cartItem.ProductId);
        }


        [HttpPost("Delete")]
        public async Task DeleteCartAsync(string userid)
        {
             await _redisService.ClearFromCache(userid);
        }


        [HttpPost("remove")]
        public async Task DeleteCartAsync(string productId, string userid)
        {
             await _redisService.RemoveCartItemAsync(productId, userid);
        }
    }
}
