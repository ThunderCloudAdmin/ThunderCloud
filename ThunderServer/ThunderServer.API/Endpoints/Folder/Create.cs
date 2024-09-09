using FastEndpoints;
using Shared.Models;
using Swashbuckle.AspNetCore.Annotations;
using ThunderServer.API.Services.Interfaces;
using static ThunderServer.API.Endpoints.Folder.Create;

namespace ThunderServer.API.Endpoints.Folder;

public class Create : Endpoint<CreateFolderRequest>
{
	private readonly IThunderFileService _thunderFileService;
	private readonly ILogger<Create> _logger;

	public Create(IThunderFileService thunderFileService, ILogger<Create> logger)
	{
		_thunderFileService = thunderFileService ?? throw new ArgumentNullException(nameof(thunderFileService));
		_logger = logger ?? throw new ArgumentNullException(nameof(logger));
	}

	public override void Configure()
	{
		Post($"/{nameof(ThunderFolder)}/Create");
		AllowAnonymous();
	}

	[SwaggerOperation(
		Summary = "Creates a new folder",
		Description = "Creates a new folder",
		OperationId = "Folder_Create",
		Tags = [$"{nameof(ThunderFolder)}Endpoint"])]
	public override async Task<IResult> HandleAsync(CreateFolderRequest request, CancellationToken cancellationToken = default)
	{
		await this._thunderFileService.Create(request.ParentFolderId, request.FolderToCreateName);		

		return Results.Created();
	}

	public sealed record CreateFolderRequest
	{
        public Guid? ParentFolderId { get; set; }

        public string FolderToCreateName { get; set; }
    }
}
