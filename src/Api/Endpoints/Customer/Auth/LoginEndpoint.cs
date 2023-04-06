using System.Text.Json.Serialization;

using Api.Interface;
using Api.Service.AuthService;

using Api.Service.UserService;

namespace Api.Endpoints.Customer.Auth;


public record LoginRequest
{
    [JsonPropertyName("phone_number")]
    public required string PhoneNumber { get; init; }
    public required string Password { get; init; }
}

public class LoginRequestValidator : Validator<LoginRequest>
{
    public LoginRequestValidator()
    {
        RuleFor(x => x.PhoneNumber)
            .NotEmpty()
            .WithMessage("Phone number is required")
            .Length(10)
            .WithMessage("Phone number must be 10 digits");
        RuleFor(x => x.Password)
            .NotEmpty()
            .WithMessage("Password is required")
            .Length(6, 50)
            .WithMessage("Password must be longer than 6 ");
    }
}

public class Data
{
    public required UserAuthDto Info { get; init; }
    public required string AccessToken { get; init; }
}

public class ValidCredentials : BaseResponse<Data>
{
}

public class LoginEndpoint : Endpoint<LoginRequest, ValidCredentials>
{
    private readonly IAuthService _authService;
    private readonly ICustomerAuthEndpointService _customerAuthEndpointService;

    public LoginEndpoint(IAuthService authService, ICustomerAuthEndpointService customerAuthEndpointService)
    {
        _authService = authService;
        _customerAuthEndpointService = customerAuthEndpointService;
    }

    public override void Configure()
    {
        Post("customer/auth/login");
        AllowAnonymous();
    }

    public override async Task HandleAsync(LoginRequest req, CancellationToken ct)
    {
        // var response = new OkResponse { Message = "cc", Status = "1", Data = "cc" };
        // await SendAsync(response, cancellation: ct);
        if (await _authService.CredentialAreValidAsync(req.PhoneNumber, req.Password).ConfigureAwait(false))
        {
            var jwtToken = _authService.CreateToken()!;
            await SendOkAsync(new ValidCredentials
            {
                Message = "Success",
                Status = "1",
                Data = new Data
                {
                    Info = await _customerAuthEndpointService.GetUserAsync(req.PhoneNumber, ct).ConfigureAwait(false),
                    AccessToken = jwtToken
                },
            }, ct).ConfigureAwait(false);
        }
        else
        {
            AddError(req => req.PhoneNumber, "Invalid phone number or password");
            AddError(req => req.Password, "Invalid phone number or password");
            await SendErrorsAsync(401, ct).ConfigureAwait(false);
        }
    }
}