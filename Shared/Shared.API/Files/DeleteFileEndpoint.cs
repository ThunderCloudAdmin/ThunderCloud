using FastEndpoints;
using Microsoft.Extensions.Logging;
using Shared.Services.Interfaces;

namespace Shared.API.Files;

public class DeleteFileEndpoint : Endpoint<DeleteFileRequest, DeleteFileResponse>
{
    private readonly IFileService _fileService;
    private readonly ILogger<DeleteFileEndpoint> _logger;

    public DeleteFileEndpoint(IFileService fileService, ILogger<DeleteFileEndpoint> logger)
    {
        _fileService = fileService ?? throw new ArgumentNullException(nameof(fileService));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public override void Configure()
    {
        Delete("/files/{FileId}");
        AllowAnonymous(); // Adjust authentication as necessary
    }

    public override async Task HandleAsync(DeleteFileRequest req, CancellationToken ct)
    {
        await _fileService.DeleteFileAsync(req.FileId);
        await SendOkAsync(ct);
    }
}

public class DeleteFileRequest
{
    public Guid FileId { get; set; }
}

public class DeleteFileResponse { }