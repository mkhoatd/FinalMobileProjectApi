using Api.Endpoints.Classrooms.Dto;

namespace Api.Service.ClassroomServices;

public interface IClassroomService
{
    Task<List<ClassroomDto>> GetClassroomsDtoAsync(int teacherId, CancellationToken ct);
}