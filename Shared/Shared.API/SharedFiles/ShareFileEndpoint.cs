using FastEndpoints;
using Shared.Models;
using Shared.Services.Interfaces;

public class ShareFileEndpoint : Endpoint<ShareFileRequest, ShareFileResponse>
{
    private readonly IFileService _fileService;
    public ShareFileEndpoint(IFileService fileService)
    {
        this._fileService = fileService;
    }

    public override void Configure()
    {
        Post("/files/share");
        AllowAnonymous(); // Adjust authentication as necessary
    }

    public override async Task HandleAsync(ShareFileRequest req, CancellationToken ct)
    {
        await _fileService.ShareFileAsync(req.FileId, req.RecipientId, req.Permission);
        await SendOkAsync();
    }
}

public class ShareFileRequest
{
    public Guid FileId { get; set; }
    public Guid RecipientId { get; set; }
    public FilePermission Permission { get; set; }
}

public class ShareFileResponse { }
