using Api.Endpoints.Classroom;
using Api.Endpoints.Classroom.Dto;

namespace Api.Service.TeacherService;

public interface IClassroomService
{
    Task<List<ClassroomDto>> GetClassroomsDtoAsync(int teacherId, CancellationToken ct);
}