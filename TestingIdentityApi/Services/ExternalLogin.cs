using Microsoft.AspNetCore.Authentication.MicrosoftAccount;
using Microsoft.Identity.Client.Platforms.Features.DesktopOs.Kerberos;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RestSharp;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Net.Http.Headers;
using System.Security.Claims;

namespace TestingIdentityApi.Services
{
    public class ExternalLogin : IExternalLogin
    {
        private readonly string _graphApiEndpoint = "https://graph.microsoft.com/v1.0/me";
        private readonly HttpClient _httpClient;
        //private readonly ILogger<ExternalLoginController> _logger;
        private readonly IConfiguration _configuration;
        public ExternalLogin(IConfiguration configuration, HttpClient httpClient)
        {
            _configuration = configuration;
            _httpClient = httpClient;
        }

        public async Task<object> MicrosoftAuth(string credentials)
        {
            if (credentials != null)
            {
                var client = new RestClient("https://login.microsoftonline.com/commom/oauth2/v2.0/token");
                var request = new RestRequest(Method.POST);
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                request.AddParameter("grant_type", "authorization_code");
                request.AddParameter("code", credentials);
                request.AddParameter("redirect_url", "http://localcalhost:4200");
                request.AddParameter("client_id", _configuration["Authentication:Microsoft:clientId"]);
                request.AddParameter("client_secret", _configuration["Authentication:Microsoft:clientSecret"]);

                IRestResponse response = client.Execute<object>(request);
                var content = response.Content;
                var result = (JObject)JsonConvert.DeserializeObject<dynamic>(content);

                var client2 = new RestClient("https://graph.microsoft.com/v1.0/me");
                IRestClient restClient = client2.AddDefaultHeader("application/json", "Bearer" + result["access_token"]);
                var request2 = new RestRequest(Method.GET);

                var response2 = client2.Execute(request2);
                var content2 = response2.Content;

                var email = JsonConvert.DeserializeObject<dynamic>(content2);

                return email;
                }

                return null;
            }


        public async Task<object> GetMicrosoftAccountUserInfoAsync(string accessToken)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidIssuer = "https://login.microsoftonline.com/dcbfd47c-b216-407f-9185-b3ccee7a4c96/v2.0", // Replace with your Azure AD tenant URL
                ValidAudience = _configuration["Authentication:Microsoft:clientId"], // Replace with your client ID
                //IssuerSigningKeys = (IEnumerable<SecurityKey>)JwtSecurityTokenHandler.DefaultInboundClaimTypeMap
            };

            SecurityToken validatedToken;
            ClaimsPrincipal claimsPrincipal = tokenHandler.ValidateToken(accessToken, tokenValidationParameters, out validatedToken);

            var userId = claimsPrincipal.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var userEmail = claimsPrincipal.FindFirst(ClaimTypes.Email)?.Value;

            // You can extract other user information from the claims as needed

            // Return the user details as a response
            return new { UserId = userId, Email = userEmail };
        }



        public async Task<object> TiktokAuth(string credentials)
        {
            if (credentials != null)
            {
                var client = new RestClient("https://open-api.tiktok.com/oauth/access_token/");
                var request = new RestRequest(Method.POST);
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                request.AddParameter("grant_type", "authorization_code");
                request.AddParameter("code", credentials);
                request.AddParameter("redirect_url", "https://localcalhost:7085");
                request.AddParameter("client_key", _configuration["Authentication:Microsoft:clientkey"]);
                request.AddParameter("client_secret", _configuration["Authentication:Microsoft:clientSecret"]);

                IRestResponse response = client.Execute<object>(request);
                var content = response.Content;
                var result = (JObject)JsonConvert.DeserializeObject(content);

                var client2 = new RestClient("https://open-api.tiktok.com");
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                client2.AddDefaultHeader("Content-Type", "application/json");
                var request2 = new RestRequest("/user/info/", Method.POST);
                var requestData = new
                {
                    access_token = result["access_token"],
                    fields = new[] { "open_id", "union_id", "avatar_url" }
                };


                request2.AddJsonBody(requestData);

                var response2 = client2.Execute(request);
                var content2 = response2.Content;

                var email = (JObject)JsonConvert.DeserializeObject(content2);
                return email;
            }

            return null;
        }
    }


    public class UserProfile
    {
        public string Id { get; set; }
        public string DisplayName { get; set; }
        public string GivenName { get; set; }
        public string Surname { get; set; }
        public string Email { get; set; }
    }
}
