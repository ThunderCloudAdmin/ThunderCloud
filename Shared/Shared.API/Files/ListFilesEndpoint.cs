using FastEndpoints;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Shared.Models;
using Shared.Services.Interfaces;
using System.Security.Claims;

namespace Shared.API.Files;

public class ListFilesEndpoint : EndpointWithoutRequest<ListFilesResponse>
{
    private readonly IFileService fileService;
    private readonly ILogger<ListFilesEndpoint> logger;
    private readonly IHttpContextAccessor httpContext;

    public ListFilesEndpoint(IFileService fileService, ILogger<ListFilesEndpoint> logger, IHttpContextAccessor httpContext)
    {
        this.fileService = fileService ?? throw new ArgumentNullException(nameof(fileService));
        this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
        this.httpContext = httpContext ?? throw new ArgumentNullException(nameof(httpContext));
    }

    public override void Configure()
    {
        Get("/files");
        AllowAnonymous(); // Adjust authentication as necessary
    }

    public override async Task HandleAsync(CancellationToken ct)
    {
        var files = await fileService.GetUserFilesAsync(httpContext.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier).Value);
        await SendAsync(new ListFilesResponse { Files = files.ToList() }, cancellation: ct);
    }
}

public class ListFilesResponse
{
    public List<ThunderFile> Files { get; set; }
}

public class FileDto
{
    public Guid FileId { get; set; }
    public string FileName { get; set; }
    public long FileSize { get; set; }
    public DateTime UploadDate { get; set; }
}