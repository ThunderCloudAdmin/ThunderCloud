using FastEndpoints;
using Microsoft.Extensions.Logging;
using Shared.Services.Interfaces;

namespace Shared.API.Files;

public class DownloadFileEndpoint : Endpoint<DownloadFileRequest, DownloadFileResponse>
{
    private readonly IFileService _fileService;
    private readonly ILogger<DownloadFileEndpoint> _logger;

    public DownloadFileEndpoint(IFileService fileService, ILogger<DownloadFileEndpoint> logger)
    {
        this._fileService = fileService ?? throw new ArgumentNullException(nameof(fileService));
        this._logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public override void Configure()
    {
        Get("/files/download/{FileId}");
        AllowAnonymous(); // Adjust authentication as necessary
    }

    public override async Task HandleAsync(DownloadFileRequest req, CancellationToken ct)
    {
        //var file = await _fileService.GetFileAsync(req.FileId);
        //await SendFileAsync(file.Stream, file.FileName);
    }
}

public class DownloadFileRequest
{
    public Guid FileId { get; set; }
}

public class DownloadFileResponse
{
    public Stream FileStream { get; set; }
    public string FileName { get; set; }
}