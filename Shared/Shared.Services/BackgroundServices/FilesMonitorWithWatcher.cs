using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Shared.Services.Configurations;
using Shared.Services.Interfaces;
using Shared.Services.Publishers.Interfaces;

namespace Shared.Services.BackgroundServices;

public class FilesMonitorWithWatcher : BackgroundService
{
    private readonly StorageConfiguration _configuration;
    private readonly ILogger<FilesMonitorWithWatcher> _logger;
    private readonly IFileEncryptorService fileEncryptorService;
    private readonly IFileUploadService fileUploadService;

    public FilesMonitorWithWatcher(
        IOptionsMonitor<StorageConfiguration> configuration,
        ILogger<FilesMonitorWithWatcher> logger,
        IFileEncryptorService fileEncryptorService,
        IFileUploadService fileUploadService)
    {
        _configuration = configuration.CurrentValue ?? throw new ArgumentNullException(nameof(configuration));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        this.fileEncryptorService = fileEncryptorService ?? throw new ArgumentNullException(nameof(fileEncryptorService));
        this.fileUploadService = fileUploadService ?? throw new ArgumentNullException(nameof(fileUploadService));

        Directory.CreateDirectory(_configuration.Root);
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            using var watcher = new FileSystemWatcher($"{_configuration.Root}")
            {
                InternalBufferSize = 64 * 1024, //to avoid missed events when a lot of changes happen inside a folder
                IncludeSubdirectories = true,
                EnableRaisingEvents = true,
                NotifyFilter = NotifyFilters.Attributes
                                 | NotifyFilters.CreationTime
                                 | NotifyFilters.DirectoryName
                                 | NotifyFilters.FileName
                                 | NotifyFilters.LastAccess
                                 | NotifyFilters.LastWrite
                                 //| NotifyFilters.Security
                                 | NotifyFilters.Size
            };

            watcher.Changed += OnChanged;
            watcher.Created += OnCreated;
            watcher.Deleted += OnDeleted;
            watcher.Renamed += OnRenamed;
            watcher.Error += OnError;

            await Task.Delay(TimeSpan.FromSeconds(1), stoppingToken);
        }
    }

    private void OnChanged(object sender, FileSystemEventArgs e)
    {
        switch (e.ChangeType)
        {
            case WatcherChangeTypes.Deleted:
                OnDeleted(sender, e);
                break;
            case WatcherChangeTypes.Created:
                OnCreated(sender, e);
                break;
            default:
                ChangedMonitor(sender, e);
                break;
        }
    }

    private void ChangedMonitor(object sender, FileSystemEventArgs e)
    {//detect whether its a directory or file
        if (Directory.Exists(e.FullPath))
        {
            _logger.LogDebug("directory state changed to: {changeType} for: {fileName} at {filePath}", e.ChangeType, e.Name, e.FullPath);
        }
        else
        {
            _logger.LogDebug("File state changed to: {changeType} for: {fileName} at {filePath}", e.ChangeType, e.Name, e.FullPath);
        }
    }

    private void OnCreated(object sender, FileSystemEventArgs e)
    {

        var createdFile = File.ReadAllBytes(e.FullPath);

        if (createdFile.Length > 0)
        {
            var bundledFile = fileEncryptorService.GenerateEncryptedFile(File.ReadAllBytes(e.FullPath));

            fileUploadService.UploadFileAsync(bundledFile);
        }
        _logger.LogDebug("Created file: {fileName} at {filePath}", e.Name, e.FullPath);
    }

    private void OnDeleted(object sender, FileSystemEventArgs e)
    {
        if (!Directory.Exists(e.FullPath) && !File.Exists(e.FullPath))
        {
            _logger.LogDebug("Deleted directory: {fileName} at {filePath}", e.Name, e.FullPath);
        }
        else if (File.Exists(e.FullPath))
        {
            _logger.LogDebug("Deleted file: {fileName} at {filePath}", e.Name, e.FullPath);
        }
    }

    private void OnRenamed(object sender, RenamedEventArgs e)
    {
        _logger.LogDebug("Renamed file: from old: {oldFilePath} - new: {newFilePath}", e.OldFullPath, e.FullPath);
    }

    private void OnError(object sender, ErrorEventArgs e)
    {
        _logger.LogError("Error occured while performing operaiton on file: {message}", e.GetException());
    }
}
