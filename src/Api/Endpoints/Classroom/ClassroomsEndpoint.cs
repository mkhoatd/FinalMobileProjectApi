using Api.Interface;
using Api.Service.TeacherService;

namespace Api.Endpoints.Classroom;

public record DataClassroomsEndpoint
{
    public List<ClassroomDto> Classrooms { get; init; }
}

public record GetTeachingClassroomsRequest
{
    [FromClaim("UserId", true, false)] public Guid TeacherId { get; init; }
    [FromClaim("Role", true, false)] public string Role { get; init; }
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
    }

    public override async Task HandleAsync(GetTeachingClassroomsRequest req, CancellationToken ct)
    {
        var dto = await _classroomService.GetTeachingClassroomsDtoAsync(req.TeacherId, ct);
        var response = new GetTeachingClassroomsResponse
        {
            Status = "1", Message = "Success", Data = new Data { Classrooms = dto }
        };
        await SendAsync(response, cancellation: ct);
    }
}