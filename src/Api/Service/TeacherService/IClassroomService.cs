using Api.Endpoints.Classroom;
using Api.Endpoints.Classroom.Dto;

namespace Api.Service.TeacherService;

public interface IClassroomService
{
    Task<List<ClassroomDto>> GetTeachingClassroomsDtoAsync(int teacherId, CancellationToken ct);
}