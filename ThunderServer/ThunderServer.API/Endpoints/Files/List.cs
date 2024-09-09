using FastEndpoints;
using Shared.Models;
using Swashbuckle.AspNetCore.Annotations;
using ThunderServer.API.Services.Interfaces;
using ThunderServer.Models;
using static ThunderServer.API.Endpoints.Files.List;

namespace ThunderServer.API.Endpoints.Files;

public class List : Endpoint<ListFilesRequest, List<ThunderFile>>
{
	private readonly IThunderFileService _thunderFileService;

	public List(IThunderFileService thunderFileService)
	{
		_thunderFileService = thunderFileService ?? throw new ArgumentNullException(nameof(thunderFileService));
	}

	public override void Configure()
	{
		Post($"/api/{nameof(ThunderFile)}/List");
		AllowAnonymous();
	}

	[SwaggerOperation(
	   Summary = "Creates a new Author",
	   Description = "Creates a new Author",
	   OperationId = "Author_Create",
	   Tags = [$"{nameof(ThunderFile)}Endpoint"])]
	public override async Task<List<ThunderFile>> HandleAsync(ListFilesRequest r, CancellationToken c)
	{
		return null;
	}

	public sealed record ListFilesRequest
	{
		public string FolderName { get; set; }
	}
}
