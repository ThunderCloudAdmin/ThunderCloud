using FastEndpoints;

public class RemoveTagEndpoint : Endpoint<RemoveTagRequest, RemoveTagResponse>
{
    public override void Configure()
    {
        Delete("/tags/remove");
        AllowAnonymous(); // Adjust authentication as necessary
    }

    public override async Task HandleAsync(RemoveTagRequest req, CancellationToken ct)
    {
        //await _tagService.RemoveTagFromFileAsync(req.FileId, req.TagName);
        //await SendOkAsync();
    }
}

public class RemoveTagRequest
{
    public Guid FileId { get; set; }
    public string TagName { get; set; }
}

public class RemoveTagResponse { }
