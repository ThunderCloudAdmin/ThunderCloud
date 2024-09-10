using HealthChecks.UI.Client;
using MassTransit;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Shared.Models;
using Shared.Services;
using Shared.Services.Configurations;
using Shared.Services.Interfaces;
using Shared.Services.Publishers;
using Shared.Services.Publishers.Interfaces;
using ThunderClient.WebApp.Components;
using ThunderClient.WebApp.Services;
using ThunderClient.WebApp.Services.Interfaces;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.Sources.Clear();

// Add configuration from environment variables first

// Add configuration from appsettings.json and appsettings.{Environment}.json
builder.Configuration.AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
builder.Configuration.AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", optional: true, reloadOnChange: true);

builder.Configuration.AddEnvironmentVariables();

//builder.Services
//    .AddFastEndpoints()
//   .SwaggerDocument(o =>
//   {
//       o.DocumentSettings = s =>
//       {
//           s.Title = "My API";
//           s.Version = "v1";
//       };
//   })
//    .AddAuthenticationJwtBearer(key => key.SigningKey = "")
//    .AddAuthorization()
//    .AddSwaggerGen();

//builder.Services.AddAutoMapper(Assembly.GetExecutingAssembly());

builder.Services.AddLogging(builder =>
{
    builder.AddDebug()
    .AddJsonConsole()
    .AddConsole();
});

builder.Services.AddOptions<StorageConfiguration>().Bind(builder.Configuration.GetSection("Storage")).ValidateOnStart();
builder.Services.AddOptions<RabbitMqConfiguration>().Bind(builder.Configuration.GetSection("RabbitMq")).ValidateOnStart();

RabbitMqConfiguration? rabbitMqSettings = builder.Configuration.GetRequiredSection("RabbitMq").Get<RabbitMqConfiguration>();

builder.Services.AddMassTransit(x =>
{
    // Add consumers here, e.g., x.AddConsumer<YourConsumer>();

    x.UsingRabbitMq((context, cfg) =>
    {
        cfg.Host(rabbitMqSettings.Host, rabbitMqSettings.VirtualHost, h =>
            {
                h.Username(rabbitMqSettings.Username);
                h.Password(rabbitMqSettings.Password);
            });
        cfg.UseMessageRetry(retry => retry.Interval(5, TimeSpan.FromSeconds(5)));
        cfg.UseInMemoryOutbox();
        cfg.ConfigureEndpoints(context);

        var exchangeName = "ThunderCloud";

        //cfg.MessageTopology.SetEntityNameFormatter(new CustomEntityNameFormatter("ThunderCloud"));

        cfg.ReceiveEndpoint($"ThunderServer.API_{nameof(IFileUploadService)}", e =>
        {
            e.Bind(rabbitMqSettings.ExchangeName, s =>
            {
                s.RoutingKey = "ThunderCloud_RK";
                //s.ExchangeType = "direct";
            });
        });
    });
});

builder.Services
    .AddHealthChecks()
    .AddRabbitMQ(
        rabbitConnectionString: $"amqp://{rabbitMqSettings.Username}:{rabbitMqSettings.Password}@{rabbitMqSettings.Host}",
        name: "rabbitmq",
        failureStatus: HealthStatus.Unhealthy,
        tags: ["ready"]);

builder.Services.AddSingleton<IRSAKeyService, RSAKeyService>();

builder.Services.AddSingleton<IEncryptionService<AesKey>, AesEncryptionService>();
builder.Services.AddSingleton<IEncryptionService<TwoFishKey>, TwoFishEncryptionService>();
builder.Services.AddSingleton<ITwoFishEncryptionService, TwoFishEncryptionService>();
builder.Services.AddSingleton<IAesEncryptionService, AesEncryptionService>();
builder.Services.AddSingleton<IFileEncryptorService, FileEncryptorService>();
builder.Services.AddSingleton<IFileUploadService, FileUploadService>();

builder.Services.AddSingleton<IFileWatcherService, FileWatcherService>();

builder.Services.AddAuthentication();
builder.Services.AddAuthorization();

builder.Services.AddHostedService<FileWatcherBackgroundService>();

builder.Services.AddBlazorBootstrap();

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

var app = builder.Build();

//app.UseAuthentication()
//    .UseAuthorization()
//    .UseFastEndpoints()
//   .UseSwaggerGen();

app.MapHealthChecks(
    "/health",
    new HealthCheckOptions
    {
        ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
    });

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    //app.UseSwagger();
    //app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseAntiforgery();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();
