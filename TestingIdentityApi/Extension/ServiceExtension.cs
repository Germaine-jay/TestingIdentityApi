
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
            configurationOptions.EndPoints.Add(config["RedisConfig:Host"], 16360);
           

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


                //var connectionMultiplexer = ConnectionMultiplexer.Connect("127.0.0.1");
                //var connectionMultiplexer = ConnectionMultiplexer.Connect("redis://default:vKd9XJqVeuNTH4MY4C0xfe2UjNQW3M91@redis-16360.c251.east-us-mz.azure.cloud.redislabs.com:16360");
                return connectionMultiplexer;
            });
            services.AddTransient<IRedisService, RedisService>();
        }
    }
}
