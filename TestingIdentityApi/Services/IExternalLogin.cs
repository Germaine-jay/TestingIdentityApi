namespace TestingIdentityApi.Services
{
    public interface IExternalLogin
    {
        Task<object> MicrosoftAuth(string credentials);
        Task<object> TiktokAuth(string credentials);
        Task<object> GetMicrosoftAccountUserInfoAsync(string accessToken);
    }
}
