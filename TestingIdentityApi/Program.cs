using Amazon.ECS.Model;
using Microsoft.AspNetCore.HttpOverrides;
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

builder.Services.AddCors(options =>
{
    options.AddPolicy("CorsPolicy", builder =>
    builder.AllowAnyOrigin()
    .AllowAnyMethod()
    .AllowAnyHeader());
});


builder.Services.AddAuthentication()
  .AddMicrosoftAccount(option=>
  {
      //IConfigurationSection microsoftAuthNSection = builder.Configuration.GetSection("Authentication:Microsoft");
      option.ClientSecret = "XSC8Q~lQWoLkM7NbfLYifVym59QbZZuKdbdMhbzw";
      option.ClientId = "171bc807-00c0-4dc1-af87-af462a27c4f1";
      option.CallbackPath = "/api/ExternalLogin/microsoft-callback";
     
      //option.AuthorizationEndpoint = "https://login.microsoftonline.com/common/oauth2/v2.0/authorize";
  } 
    )
  .AddGoogle(option=>
  {
      //IConfigurationSection microsoftAuthNSection = builder.Configuration.GetSection("Authentication:Microsoft");
      option.ClientSecret = "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";
      option.ClientId = "286600713169-1frq8hfoq4hsl1qm760f9v17u1bvh9fe.apps.googleusercontent.com";
      //option.CallbackPath = "/https://localhost:7085/api/ExternalLogin/microsoft-callback";
     
      //option.AuthorizationEndpoint = "https://login.microsoftonline.com/common/oauth2/v2.0/authorize";
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
app.UseForwardedHeaders();
app.UseRouting();
app.UseAuthorization();
app.UseCors();
app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
app.MapControllers();

app.Run();
