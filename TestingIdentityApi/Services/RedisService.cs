using StackExchange.Redis;

namespace TestingIdentityApi.Services
{
    public class RedisService : IRedisService
    {
        //private readonly ConnectionMultiplexer _redis;
        private readonly StackExchange.Redis.IConnectionMultiplexer _connectionMultiplexer;
        private readonly StackExchange.Redis.IDatabase _redis;
        private readonly StackExchange.Redis.IServer _server;

        public RedisService(IConnectionMultiplexer redis)
        {
            //_redis = ConnectionMultiplexer.Connect("localhost:6379");
            _connectionMultiplexer = redis;
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
            try
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
            catch (Exception ex)
            {
                throw new InvalidOperationException(userId, ex);
            }
        }


        public async Task<bool> SaveCartAsync(Cart cart, CartItem? cartItem)
        {
            try
            {
                var total = 0;
                var key = $"cart:{cart.Id}";

                if(cartItem != null)
                {
                    cart.CartItems.Add(cartItem);
                    total = (cartItem.Quantity * cartItem.Price);
                    cart.Total = total;
                }

                var cartString = System.Text.Json.JsonSerializer.Serialize(cart);
                var a = await _redis.StringSetAsync(key, cartString, TimeSpan.FromDays(365));

                return a;
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException(ex.Message);
            }
        }


        public async Task<bool> ClearFromCache(string userId)
        {
            var key = $"cart:{userId}";
            if (userId != null)
                return false;

            await _redis.KeyDeleteAsync(key);
            return true;

        }


        public async Task<bool> RemoveCartItemAsync(string productId, string userId)
        {
            var cart = await GetCartAsync(userId)
                ?? throw new Exception("user not found");

            var cartItemToRemove = cart.CartItems.Where(c => c.ProductId.ToString() == productId).FirstOrDefault();
            if (cartItemToRemove != null)
            {
                cart.CartItems.Remove(cartItemToRemove);
                await SaveCartAsync(cart, null);
                return true;
            }
            return false;   
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
