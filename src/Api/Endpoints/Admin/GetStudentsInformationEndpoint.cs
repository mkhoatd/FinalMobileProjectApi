using Api.Endpoints.Admin.Dtos;
using Api.Interface;
using Api.Service.AdminServices;

using Data.Entities;

namespace Api.Endpoints.Admin;

public record GetStudentsInformationResponseData
{
    public required List<StudentInformationDto> Students { get; init; }
}

public class GetStudentsInformationEndpoint : EndpointWithoutRequest<BaseResponse<List<StudentInformationDto>>>
{
    private readonly IAdminService _adminService;

    public GetStudentsInformationEndpoint(IAdminService adminService)
    {
        _adminService = adminService;
    }

    public override void Configure()
    {
        Get("/admin/students");
        Roles(RoleName.Admin.ToString());
    }

    public override async Task HandleAsync(CancellationToken cancellationToken)
    {
        List<StudentInformationDto> students = await _adminService.GetStudentsInformationAsync(cancellationToken);
        BaseResponse<List<StudentInformationDto>> response = new BaseResponse<List<StudentInformationDto>>
        {
            Data = students, Message = "successfully", Status = "Ok"
        };
        await SendOkAsync(response, cancellationToken);
    }
}