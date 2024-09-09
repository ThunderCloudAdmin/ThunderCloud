using FastEndpoints;
using Shared.Services.Interfaces;

namespace Shared.API.Directories;

public class ListDirectoriesEndpoint : EndpointWithoutRequest<ListDirectoriesResponse>
{
    private readonly IDirectoryService _directoryService;

    public ListDirectoriesEndpoint(IDirectoryService directoryService)
    {
        _directoryService = directoryService;
    }

    public override void Configure()
    {
        Get("/directories");
        AllowAnonymous(); // Adjust authentication as necessary
    }

    public override async Task HandleAsync(CancellationToken ct)
    {
        //var directories = await _directoryService.GetUserDirectoriesAsync(UserId);
        //await SendAsync(new ListDirectoriesResponse { Directories = directories });
    }
}

public class ListDirectoriesResponse
{
    public List<DirectoryDto> Directories { get; set; }
}

public class DirectoryDto
{
    public Guid DirectoryId { get; set; }
    public string DirectoryName { get; set; }
}