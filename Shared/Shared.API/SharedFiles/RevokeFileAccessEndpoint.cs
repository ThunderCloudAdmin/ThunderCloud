using FastEndpoints;

public class RevokeFileAccessEndpoint : Endpoint<RevokeFileAccessRequest, RevokeFileAccessResponse>
{
    public override void Configure()
    {
        Delete("/files/revoke");
        AllowAnonymous(); // Adjust authentication as necessary
    }

    public override async Task HandleAsync(RevokeFileAccessRequest req, CancellationToken ct)
    {
        //await _fileService.RevokeFileAccessAsync(req.FileId, req.RecipientId);
        //await SendOkAsync();
    }
}

public class RevokeFileAccessRequest
{
    public Guid FileId { get; set; }
    public Guid RecipientId { get; set; }
}

public class RevokeFileAccessResponse { }
