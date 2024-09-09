using FastEndpoints;
using Shared.API.Files;

public class ListSharedFilesEndpoint : EndpointWithoutRequest<ListSharedFilesResponse>
{
    public override void Configure()
    {
        Get("/files/shared");
        AllowAnonymous(); // Adjust authentication as necessary
    }

    public override async Task HandleAsync(CancellationToken ct)
    {
        //var sharedFiles = await _fileService.GetSharedFilesForUserAsync(UserId);
        //await SendAsync(new ListSharedFilesResponse { Files = sharedFiles });
    }
}

public class ListSharedFilesResponse
{
    public List<FileDto> Files { get; set; }
}
