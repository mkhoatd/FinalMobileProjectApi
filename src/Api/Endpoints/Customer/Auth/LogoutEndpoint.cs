namespace Api.Endpoints.Customer.Auth;

// public record LogoutRequest
// {
//     public required string authToken{ get; init; }
// }


public class LogoutEndpoint : EndpointWithoutRequest 
{

    public override void Configure()
    {
        Post("customer/auth/logout");
        AllowAnonymous();
    }

    public override async Task HandleAsync(CancellationToken ct)
    {
        await SendOkAsync("ok nho",ct);
    }
}