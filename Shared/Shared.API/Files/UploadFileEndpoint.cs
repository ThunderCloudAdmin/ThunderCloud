using FastEndpoints;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Shared.Services.Interfaces;

namespace Shared.API.Files;

public class UploadFileEndpoint : Endpoint<UploadFileRequest, UploadFileResponse>
{
    private readonly IFileService _fileService;
    private readonly ILogger _logger;

    public UploadFileEndpoint(IFileService fileService, ILogger<UploadFileEndpoint> _logger)
    {
        _fileService = fileService;
        this._logger = _logger;
    }

    public override void Configure()
    {
        Post("/files/upload");
        AllowAnonymous(); // Adjust authentication as necessary
    }

    public override async Task HandleAsync(UploadFileRequest req, CancellationToken ct)
    {
        //Steps to do:
        //read the file as stream
        //encrypt it with AES public key
        //upload file + generated public key to the server

        var file = req.File;



        // Implementation for file upload (including encryption)
        //var fileId = await _fileService.UploadFileAsync(req);
        await SendAsync(new UploadFileResponse { FileId = Guid.NewGuid() });
    }
}

public class UploadFileRequest
{
    public IFormFile File { get; set; }
    public string FileName { get; set; }
}

public class UploadFileResponse
{
    public Guid FileId { get; set; }
}