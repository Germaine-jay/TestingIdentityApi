using Newtonsoft.Json;
using StackExchange.Redis;

namespace TestingIdentityApi.Services
{
    public class RedisService : IRedisService
    {
        //private readonly ConnectionMultiplexer _redis;
        private readonly StackExchange.Redis.IDatabase _redis;
        private readonly StackExchange.Redis.IServer _server;

        public RedisService(IConnectionMultiplexer redis)
        {
            //_redis = ConnectionMultiplexer.Connect("localhost:6379");
            _redis = redis.GetDatabase();
            _server = redis.GetServer(redis.GetEndPoints()[0]);
        }


        /*public async Task SaveEntity(Product product)
        {
            string stringifiedJson = JsonConvert.SerializeObject(product);
            await _redis.StringSetAsync(key, stringifiedJson, absoluteExpireTime, When.Always).ConfigureAwait(true);

            if (product != null)
                await _redis.SetAddAsync(cacheKeySets.Value.ToString(), key);


            // Serialize the entity to a JSON string.
            var json = JsonConvert.SerializeObject(product);

            // Set the entity in Redis with a key-value pair.
            //db.StringSet($"product:{product.Id}", json);
        }*/


        public async Task<Cart> GetCartAsync(string userId)
        {
            var key = $"cart:{userId}";
            var cartString = await _redis.StringGetAsync(key);
            if (cartString.IsNull)
            {
                return new Cart();
            }

            var cart = System.Text.Json.JsonSerializer.Deserialize<Cart>(cartString);
            return cart;
        }

        public async Task SaveCartAsync(Cart cart)
        {
            var key = $"cart:{cart.Id}";
            var cartString = System.Text.Json.JsonSerializer.Serialize(cart);
            await _redis.StringSetAsync(key, cartString, TimeSpan.FromDays(365));
        }


        public async Task ClearFromCache(string userId)
        {
            var key = $"cart:{userId}";
            await _redis.KeyDeleteAsync(key);
        }


        public async Task RemoveCartItemAsync(string productId,string userId)
        {
            var cart = await GetCartAsync(userId);
            var cartItemToRemove = cart.CartItems.Where(c => c.ProductId.ToString() == productId).FirstOrDefault();
            if (cartItemToRemove != null)
            {
                cart.CartItems.Remove(cartItemToRemove);
            }

            await SaveCartAsync(cart);
        }
    }

    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
    }

    public class Cart
    {
        public Guid Id { get; set; }
        public int Total { get; set; }
        public List<CartItem> CartItems { get; set; } = new List<CartItem>();
    }

    public class CartDto
    {
        public Guid Id { get; set; }
        public int Total { get; set; }
        public List<CartItem> CartItems { get; set; } = new List<CartItem>();
    }

    public class CartItem
    {
        public Guid ProductId { get; set; }
        public int Quantity { get; set; }
        public int Price { get; set; }
    }
}
