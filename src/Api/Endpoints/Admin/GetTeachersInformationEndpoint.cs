using Api.Endpoints.Admin.Dtos;
using Api.Interface;
using Api.Service.AdminServices;

using Data.Entities;

namespace Api.Endpoints.Admin;

public record GetTeachersInformationResponseData
{
    public required List<TeacherInformationDto> Teachers { get; init; }
}

public class GetTeachersInformationEndpoint : EndpointWithoutRequest<BaseResponse<List<TeacherInformationDto>>>
{
    private readonly IAdminService _adminService;

    public GetTeachersInformationEndpoint(IAdminService adminService)
    {
        _adminService = adminService;
    }

    public override void Configure()
    {
        Get("/admin/teachers");
        Roles(RoleName.Admin.ToString());
    }

    public override async Task HandleAsync(CancellationToken cancellationToken)
    {
        List<TeacherInformationDto> teachers = await _adminService.GetTeachersInformationAsync(cancellationToken);
        BaseResponse<List<TeacherInformationDto>> response = new BaseResponse<List<TeacherInformationDto>>
        {
            Data = teachers, Message = "successfully", Status = "Ok"
        };
        await SendOkAsync(response, cancellationToken);
    }
}