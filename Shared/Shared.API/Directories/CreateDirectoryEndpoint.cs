using FastEndpoints;
using Shared.Services.Interfaces;

namespace Shared.API.Directories;

public class CreateDirectoryEndpoint : Endpoint<CreateDirectoryRequest, CreateDirectoryResponse>
{
    private readonly IDirectoryService _directoryService;

    public CreateDirectoryEndpoint(IDirectoryService directoryService)
    {
        _directoryService = directoryService;
    }

    public override void Configure()
    {
        Post("/directories/create");
        AllowAnonymous(); // Adjust authentication as necessary
    }

    public override async Task HandleAsync(CreateDirectoryRequest req, CancellationToken ct)
    {
        var directoryId = await _directoryService.CreateDirectoryAsync(req.DirectoryName, req.ParentDirectoryId);
        await SendAsync(new CreateDirectoryResponse { DirectoryId = directoryId }, cancellation: ct);
    }
}

public class CreateDirectoryRequest
{
    public string DirectoryName { get; set; }
    public Guid? ParentDirectoryId { get; set; }
}

public class CreateDirectoryResponse
{
    public Guid DirectoryId { get; set; }
}