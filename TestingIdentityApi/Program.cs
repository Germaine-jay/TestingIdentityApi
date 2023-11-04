using Amazon.ECS.Model;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using TestingIdentityApi.Extension;
using TestingIdentityApi.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRedisCache(builder.Configuration);
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddHostedService<ConsumeScopedServiceHostedService>();
builder.Services.AddScoped<IScopedProcessingService, ScopedProcessingService>();
builder.Services.AddScoped<IExternalLogin, ExternalLogin>();
builder.Services.AddHttpClient();

builder.Services.AddAuthentication()
  .AddMicrosoftAccount(option=>
  {
      //IConfigurationSection microsoftAuthNSection = builder.Configuration.GetSection("Authentication:Microsoft");
      option.ClientSecret = "XSC8Q~lQWoLkM7NbfLYifVym59QbZZuKdbdMhbzw";
      option.ClientId = "171bc807-00c0-4dc1-af87-af462a27c4f1";
      option.CallbackPath = "http://localhost:4200";
      option.AuthorizationEndpoint = "https://login.microsoftonline.com/common/oauth2/v2.0/authorize";
  }
    )
.AddOAuth("TikTok", options =>
{
    options.ClientId = "YOUR_CLIENT_ID";
    options.ClientSecret = "YOUR_CLIENT_SECRET";
    options.CallbackPath = "/signin-tiktok";
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment() || app.Environment.IsProduction() || app.Environment.IsStaging())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
