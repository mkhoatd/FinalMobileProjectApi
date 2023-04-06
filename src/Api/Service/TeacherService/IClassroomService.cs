using Api.Endpoints.Classroom;

namespace Api.Service.TeacherService;

public interface IClassroomService
{
    Task<List<ClassroomDto>> GetTeachingClassroomsDtoAsync(Guid teacherId, CancellationToken ct);
}