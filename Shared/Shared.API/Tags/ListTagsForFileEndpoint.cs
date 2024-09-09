using FastEndpoints;

public class ListTagsForFileEndpoint : Endpoint<ListTagsForFileRequest, ListTagsForFileResponse>
{
    public override void Configure()
    {
        Get("/tags/{FileId}");
        AllowAnonymous(); // Adjust authentication as necessary
    }

    public override async Task HandleAsync(ListTagsForFileRequest req, CancellationToken ct)
    {
        //var tags = await _tagService.GetTagsForFileAsync(req.FileId);
        //await SendAsync(new ListTagsForFileResponse { Tags = tags });
    }
}

public class ListTagsForFileRequest
{
    public Guid FileId { get; set; }
}

public class ListTagsForFileResponse
{
    public List<string> Tags { get; set; }
}
