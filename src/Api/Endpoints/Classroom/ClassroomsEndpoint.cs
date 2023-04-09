using Api.Endpoints.Classroom.Dto;
using Api.Interface;
using Api.Service.TeacherService;

using Data.Entities;

// ReSharper disable UnusedAutoPropertyAccessor.Global
// ReSharper disable ClassNeverInstantiated.Global
#pragma warning disable CS8618

namespace Api.Endpoints.Classroom;

public record DataClassroomsEndpoint
{
    public decimal? Salary { get; init; }

    public List<ClassroomDto> Classrooms { get; init; }
}

public record GetTeachingClassroomsRequest
{
    [FromClaim("UserId")] public int TeacherId { get; init; }
    [FromClaim("Role")] public RoleName RoleName { get; init; }
}

public class GetTeachingClassroomsResponse : BaseResponse<DataClassroomsEndpoint>
{
}

public class ClassroomsEndpoint : Endpoint<GetTeachingClassroomsRequest, GetTeachingClassroomsResponse>
{
    private readonly IClassroomService _classroomService;

    public ClassroomsEndpoint(IClassroomService classroomService)
    {
        _classroomService = classroomService;
    }

    public override void Configure()
    {
        Get("/classrooms");
        Roles(RoleName.Teacher.ToString(), RoleName.Student.ToString());
    }

    public override async Task HandleAsync(GetTeachingClassroomsRequest req, CancellationToken ct)
    {
        var dto = await _classroomService.GetClassroomsDtoAsync(req.TeacherId, ct);
        var numStudent = 0;
        foreach (ClassroomDto classroomDto in dto)
        {
            numStudent += classroomDto.ClassroomSize;
        }

        decimal? salary = null;
        if (req.RoleName == RoleName.Teacher)
        {
            salary = numStudent * 500000 * 0.7M;
        }

        var response = new GetTeachingClassroomsResponse
        {
            Status = "1",
            Message = "Success",
            Data = new DataClassroomsEndpoint { Classrooms = dto, Salary = salary, }
        };
        await SendOkAsync(response, cancellation: ct);
    }
}