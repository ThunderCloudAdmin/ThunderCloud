using Microsoft.Extensions.Options;
using Shared.Services.Configurations;
using ThunderClient.WebApp.Services.Interfaces;

namespace ThunderClient.WebApp.Services;

/// <summary>
/// Represents a background service that manages file system watchers.
/// </summary>
public class FileWatcherBackgroundService : BackgroundService
{
    private readonly IFileWatcherService _fileWatcherService;
    private readonly StorageConfiguration _configuration;

    // <summary>
    /// Initializes a new instance of the <see cref="FileWatcherBackgroundService"/> class.
    /// </summary>
    /// <param name="fileWatcherService">The <see cref="FileWatcherService"/> instance used to manage file system watchers.</param>
    public FileWatcherBackgroundService(IFileWatcherService fileWatcherService, IOptionsMonitor<StorageConfiguration> configuration)
    {
        _fileWatcherService = fileWatcherService;
        _configuration = configuration.CurrentValue ?? throw new ArgumentNullException(nameof(configuration));

        Directory.CreateDirectory(_configuration.Root);

        _fileWatcherService.AddWatcher(_configuration.Root);
    }

    /// <summary>
    /// Executes the background service's main task. This method is called when the service starts.
    /// </summary>
    /// <param name="stoppingToken">A <see cref="CancellationToken"/> that signals the service to stop.</param>
    /// <returns>A <see cref="Task"/> that represents the asynchronous operation.</returns>
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        // The service will remain alive for the application's lifetime.
        while (!stoppingToken.IsCancellationRequested)
        {
            // The FileSystemWatcher operates on events, so no active polling is needed.
            // We use a delay to keep the background service active.
            await Task.Delay(Timeout.Infinite, stoppingToken); // Keep the service alive.
        }
    }

    /// <summary>
    /// Performs application-defined tasks associated with stopping the background service.
    /// This method is called when the application is shutting down.
    /// </summary>
    /// <param name="stoppingToken">A <see cref="CancellationToken"/> that signals the service to stop.</param>
    /// <returns>A <see cref="Task"/> that represents the asynchronous operation.</returns>
    public override async Task StopAsync(CancellationToken stoppingToken)
    {
        // Dispose of all watchers when the service is stopping
        _fileWatcherService.Dispose();

        await base.StopAsync(stoppingToken);
    }
}