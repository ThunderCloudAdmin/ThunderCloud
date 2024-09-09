using MassTransit;
using Microsoft.Extensions.Logging;
using Shared.Models;
using Shared.Services.Publishers.Interfaces;

namespace Shared.Services.Publishers;

public class FileUploadService : IFileUploadService
{
    private readonly IBus bus;
    private readonly ILogger<FileUploadService> logger;

    public FileUploadService(IBus bus, ILogger<FileUploadService> logger)
    {
        this.bus = bus ?? throw new ArgumentNullException(nameof(bus));
        this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task UploadFileAsync(EncryptedBundle encryptedBundle)
    {
        logger.LogInformation("Publishing message at {className}", nameof(FileUploadService));
        await bus.Publish(encryptedBundle);
    }
}
