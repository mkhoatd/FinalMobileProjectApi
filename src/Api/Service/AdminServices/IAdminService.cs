using Api.Endpoints.Admin.Dtos;

using Data.Entities;

namespace Api.Service.AdminServices;

public interface IAdminService
{
    Task<bool> CreateAccountAsync(string phoneNumber, string password, RoleName roleName, string username,
        string name, CancellationToken ct);

    Task<bool> CreateClasssroomAsync(SubjectName subject, int teacherId, string description, List<int> studentIds,
        List<StudySessionAdminDto> studySessions);

    Task<List<StudentInformationDto>> GetStudentsInformationAsync(CancellationToken ct);
    Task<List<TeacherInformationDto>> GetTeachersInformationAsync(CancellationToken ct);
}