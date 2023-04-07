using System.Net;
using System.Text.Json.Serialization;

using Api.Extensions;
using Api.Interface;
using Api.Service.AuthService;
using Api.Service.UserService;

namespace Api.Endpoints.Customer.Auth;

public record LoginRequest
{
    [JsonPropertyName("phone_number")] public required string PhoneNumber { get; init; }
    public required string Password { get; init; }
    public string? DeviceToken { get; init; }
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
        var jwtTokenOrNone = await _authService.CheckCredentialAndCreateTokenAsync(req.PhoneNumber, req.Password);
        await jwtTokenOrNone.Match(
            async _ =>
            {
                AddError(req => req.PhoneNumber, "Wrong phone number or password");
                AddError(req => req.Password, "Wrong phone number or password");
                await SendErrorsAsync(HttpStatusCode.Unauthorized.Value(), ct);
            },
            async jwtToken =>
            {
                if (!String.IsNullOrEmpty(req.DeviceToken))
                    await _customerAuthEndpointService.SaveDeviceToken(req.PhoneNumber, req.DeviceToken, ct);

                await SendOkAsync(
                    new ValidCredentials
                    {
                        Message = "Success",
                        Status = "1",
                        Data = new Data
                        {
                            Info = await _customerAuthEndpointService.GetUserAsync(req.PhoneNumber, ct)
                                .ConfigureAwait(false),
                            AccessToken = jwtToken.Value
                        },
                    }, ct).ConfigureAwait(false);
            });
    }
}