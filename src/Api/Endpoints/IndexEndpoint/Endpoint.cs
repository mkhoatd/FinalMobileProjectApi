namespace Api.Endpoints;

public class IndexResponse
{
    public string Message { get; set; } = default!;
}

public class IndexRequest
{
    [FromClaim] public string UserId { get; init; } = default!;
}

public class IndexEndpoint : Endpoint<IndexRequest, IndexResponse>
{
    public override void Configure()
    {
        Get("/");
        Claims("UserId");
    }

    public override async Task HandleAsync(IndexRequest req, CancellationToken ct)
    {
        await SendOkAsync(new IndexResponse
        {
            Message = $"Hello {req.UserId}"
        }, ct);
    }
}
