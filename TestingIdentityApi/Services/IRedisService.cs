namespace TestingIdentityApi.Services
{
    public interface IRedisService
    {
        Task<bool> SaveCartAsync(Cart cart, CartItem? cartItem);
        Task<Cart> GetCartAsync(string userId);

        Task<bool> ClearFromCache(string key);

        Task<bool> RemoveCartItemAsync(string productId, string userId);
    }
}
