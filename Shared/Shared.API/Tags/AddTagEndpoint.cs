using FastEndpoints;

public class AddTagEndpoint : Endpoint<AddTagRequest, AddTagResponse>
{
    public override void Configure()
    {
        Post("/tags/add");
        AllowAnonymous(); // Adjust authentication as necessary
    }

    public override async Task HandleAsync(AddTagRequest req, CancellationToken ct)
    {
        //await _tagService.AddTagToFileAsync(req.FileId, req.TagName);
        //await SendOkAsync();
    }
}

public class AddTagRequest
{
    public Guid FileId { get; set; }
    public string TagName { get; set; }
}

public class AddTagResponse { }
