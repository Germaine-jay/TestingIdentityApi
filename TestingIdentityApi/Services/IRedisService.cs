namespace TestingIdentityApi.Services
{
    public interface IRedisService
    {
        Task SaveCartAsync(Cart cart);
        Task<Cart> GetCartAsync(string userId);

        Task ClearFromCache(string key);

        Task RemoveCartItemAsync(string productId, string userId);
    }
}
