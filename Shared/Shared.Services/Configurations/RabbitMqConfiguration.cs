namespace Shared.Services.Configurations;

public class RabbitMqConfiguration
{
    public string Username { get; set; }
    public string Password { get; set; }
    public string Host { get; set; }
    public string VirtualHost { get; set; }
    public string ExchangeName { get; set; }
}
