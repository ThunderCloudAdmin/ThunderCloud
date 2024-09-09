using FastEndpoints;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.Extensions.Options;
using Shared.Models;
using Shared.Services.Configurations;
using Swashbuckle.AspNetCore.Annotations;
using ThunderServer.API.Services.Interfaces;
using ThunderServer.Models;
using ProblemDetails = FastEndpoints.ProblemDetails;

namespace ThunderServer.API.Endpoints.Files;

public class Create : Endpoint<FileUploadRequest, Results<Ok<FileUploadResponse>, NotFound, ProblemDetails>>
{
    private readonly StorageConfiguration _storageConfiguration;
    private readonly ILogger<Create> _logger;
    private readonly IThunderFileService _thunderFileService;


    public Create(IOptionsMonitor<StorageConfiguration> options, ILogger<Create> logger, IThunderFileService thunderFileService)
    {
        this._storageConfiguration = options.CurrentValue;
        this._logger = logger;
        this._thunderFileService = thunderFileService;
        //this._mapper = mapper;
    }

    public override void Configure()
    {
        Post($"/api/{nameof(ThunderFile)}/Create");
        AllowAnonymous();
    }

    [SwaggerOperation(
        Summary = "Creates a new Author",
        Description = "Creates a new Author",
        OperationId = "Author_Create",
        Tags = [$"{nameof(ThunderFile)}Endpoint"])]
    public override async Task<Results<Ok<FileUploadResponse>, NotFound, ProblemDetails>> ExecuteAsync(FileUploadRequest request, CancellationToken cancellationToken = default)
    {
        try
        {
            // Check if files are present
            if (request.Files == null || request.Files.Count == 0)
            {
                //return TypedResults.BadRequest("No files were uploaded.");
            }

            // Directory to save uploaded files
            var uploadDirectory = Path.Combine(Directory.GetCurrentDirectory(), request.PathToStoreFiles ?? _storageConfiguration.Root);

            //var results = await _thunderFileService.AddFileToFolder(request.Files, cancellationToken, uploadDirectory);

            return TypedResults.Ok(new FileUploadResponse
            {
                ThunderFiles = null
            });
        }
        catch (Exception ex)
        {
            _logger.LogError("Error while uploading file to server {fileName}", ex.Message);
            //return TypedResults.StatusCode(500); //, $"Internal server error: {ex.Message}");
            return null;
        }
    }
}
