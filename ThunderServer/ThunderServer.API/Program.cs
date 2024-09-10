using FastEndpoints;
using FastEndpoints.Security;
using FastEndpoints.Swagger;
using HealthChecks.UI.Client;
using MassTransit;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Shared.Models;
using Shared.Services;
using Shared.Services.Configurations;
using Shared.Services.Interfaces;
using Shared.Services.Publishers;
using Shared.Services.Publishers.Interfaces;
using ThunderServer.API.Configurations;
using ThunderServer.API.Extensions;
using ThunderServer.API.Services;
using ThunderServer.API.Services.Interfaces;
using ThunderServer.Infrastructure;
using ThunderServer.Infrastructure.Repositories;
using ThunderServer.Infrastructure.Repositories.Interfaces;
using ThunderServer.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle

builder.Configuration.Sources.Clear();

// Add configuration from environment variables first

// Add configuration from appsettings.json and appsettings.{Environment}.json
builder.Configuration.AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
builder.Configuration.AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", optional: true, reloadOnChange: true);

builder.Configuration.AddEnvironmentVariables();

builder.Services
    .AddFastEndpoints()
   .SwaggerDocument(o =>
   {
       o.DocumentSettings = s =>
       {
           s.Title = "ThunderServer.API";
           s.Version = "v1";
       };
   })
    .AddAuthenticationJwtBearer(key => key.SigningKey = "")
    .AddAuthorization()
    .AddSwaggerGen();

builder.Services.AddDbContext<ThunderServerContext>(options =>
{
    options.UseNpgsql(builder.Configuration.GetConnectionString("PostgresConnection"));
});

builder.Services
    .AddIdentity<ThunderUser, IdentityRole<Guid>>()
    .AddEntityFrameworkStores<ThunderServerContext>()
    .AddDefaultTokenProviders();

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
    .AddNpgSql("PostgresConnection")
    .AddRabbitMQ(
        rabbitConnectionString: $"amqp://{rabbitMqSettings.Username}:{rabbitMqSettings.Password}@{rabbitMqSettings.Host}",
        name: "rabbitmq",
        failureStatus: HealthStatus.Unhealthy,
        tags: ["ready"]);

builder.Services.AddScoped<IThunderFileService, ThunderFileService>();
builder.Services.AddSingleton<IEmailSender<ThunderUser>, EmailSender>();

builder.Services.AddSingleton<IRSAKeyService, RSAKeyService>();

builder.Services.AddSingleton<IEncryptionService<AesKey>, AesEncryptionService>();
builder.Services.AddSingleton<IEncryptionService<TwoFishKey>, TwoFishEncryptionService>();
builder.Services.AddSingleton<ITwoFishEncryptionService, TwoFishEncryptionService>();
builder.Services.AddSingleton<IAesEncryptionService, AesEncryptionService>();
builder.Services.AddSingleton<IFileEncryptorService, FileEncryptorService>();
builder.Services.AddSingleton<IFileUploadService, FileUploadService>();

builder.Services.AddScoped<IRsaKeyEncryptionService, RsaKeyEncryptionService>();

builder.Services.AddScoped<IThunderFileRepository, ThunderFileRepository>();
builder.Services.AddScoped<IRsaKeyPairServerRepository, RsaKeyPairServerRepository>();
builder.Services.AddScoped<IArgon2KeyRepository, Argon2KeyRepository>();

//builder.Services.AddSingleton<IBackgroundTaskQueue, FileToUpdateTaskQueue>();

builder.Services.AddAuthentication();
builder.Services.AddAuthorization();

var app = builder.Build();

app.UseAuthentication()
    .UseAuthorization()
    .UseFastEndpoints()
   .UseSwaggerGen();

app.MapIdentityApiFilterable<ThunderUser>(new IdentityApiEndpointRouteBuilderOptions()
{
    ExcludeRegisterPost = true,
    ExcludeLoginPost = false,
    ExcludeRefreshPost = false,
    ExcludeConfirmEmailGet = false,
    ExcludeResendConfirmationEmailPost = false,
    ExcludeForgotPasswordPost = false,
    ExcludeResetPasswordPost = false,
    // setting ExcludeManageGroup to false will disable
    // 2FA and both Info Actions
    ExcludeManageGroup = false,
    Exclude2faPost = false,
    ExcludegInfoGet = false,
    ExcludeInfoPost = true,
});

app.MapHealthChecks(
    "/health",
    new HealthCheckOptions
    {
        ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
    });

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.Run();
