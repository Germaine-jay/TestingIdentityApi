
using StackExchange.Redis;
using System.Security.Authentication;
using TestingIdentityApi.Services;

namespace TestingIdentityApi.Extension
{
    public static class ServiceExtension
    {
        public static void AddRedisCache(this IServiceCollection services, IConfiguration config)
        {

            ConfigurationOptions configurationOptions = new ConfigurationOptions();
            configurationOptions.SslProtocols = SslProtocols.Tls12;
            //configurationOptions.SyncTimeout = 30000;
            configurationOptions.Ssl = true;
            configurationOptions.Password = config["RedisConfig:Password"];
            configurationOptions.AbortOnConnectFail = false;
            configurationOptions.EndPoints.Add(config["RedisConfig:Host"], 6379);

            services.AddStackExchangeRedisCache(options =>
            {
                options.Configuration = configurationOptions.ToString();
                options.InstanceName = config["RedisConfig:InstanceId"];
            });

            services.AddSingleton<IConnectionMultiplexer>((x) =>
            {
                var connectionMultiplexer = ConnectionMultiplexer.Connect(new ConfigurationOptions
                {
                    Password = configurationOptions.Password,
                    EndPoints = { configurationOptions.EndPoints[0] },
                    AbortOnConnectFail = false,
                    AllowAdmin = false,
                    ClientName = config["RedisConfig:InstanceId"]
                });
                return connectionMultiplexer;
            });
            services.AddTransient<IRedisService, RedisService>();
        }
    }
}
