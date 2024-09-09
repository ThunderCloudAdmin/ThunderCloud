using FastEndpoints;
using Shared.Services.Interfaces;

namespace Shared.API.Directories
{
    public class MoveFileToDirectoryEndpoint : Endpoint<MoveFileToDirectoryRequest, MoveFileToDirectoryResponse>
    {
        private readonly IDirectoryService _directoryService;

        public MoveFileToDirectoryEndpoint(IDirectoryService directoryService)
        {
            _directoryService = directoryService;
        }

        public override void Configure()
        {
            Post("/directories/movefile");
            AllowAnonymous(); // Adjust authentication as necessary
        }

        public override async Task HandleAsync(MoveFileToDirectoryRequest req, CancellationToken ct)
        {
            await _directoryService.MoveFileToDirectoryAsync(req.FileId, req.DirectoryId);
            await SendOkAsync();
        }
    }

    public class MoveFileToDirectoryRequest
    {
        public Guid FileId { get; set; }
        public Guid DirectoryId { get; set; }
    }

    public class MoveFileToDirectoryResponse { }
}