using Api.Interface;
using Api.Service.AdminServices;

using Data.Entities;

namespace Api.Endpoints.Admin;

public record CreateAccountRequest
{
    public required string Name { get; init; }
    public required string Username { get; init; }
    public required string PhoneNumber { get; init; }
    public required string Password { get; init; }
    public string? Role { get; init; }
}

public class CreateAccountRequestValidator : Validator<CreateAccountRequest>
{
    public CreateAccountRequestValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .Length(6, 20)
            .WithMessage("Username must be longer than 6 and shorter than 20");
        RuleFor(x => x.Username)
            .NotEmpty();
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

public record Data
{
    public required string Result { get; init; }
}

public class CreateAccountResponse : BaseResponse<Data>
{
}

public class CreateAccountEndpoint : Endpoint<CreateAccountRequest, CreateAccountResponse>
{
    private readonly IAdminService _adminService;


    public CreateAccountEndpoint(IAdminService adminService)
    {
        _adminService = adminService;
    }

    public override void Configure()
    {
        Post("/admin/create-account");
        Roles(RoleName.Admin.ToString());
    }

    public override async Task HandleAsync(CreateAccountRequest req, CancellationToken ct)
    {
        bool res = await _adminService.CreateAccountAsync(req.PhoneNumber, req.Password,
            Enum.Parse<RoleName>(req.Role ?? "Teacher"),
            req.Username, req.Name, ct);
        if (res)
        {
            await SendOkAsync(
                new CreateAccountResponse
                {
                    Data = new Data { Result = "Success" }, Message = "Success", Status = "Ok"
                },
                ct);
        }
        else
        {
            await SendErrorsAsync(cancellation: ct);
        }
    }
}