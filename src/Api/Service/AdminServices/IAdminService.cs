using Api.Endpoints.Admin.Dtos;

using Data.Entities;

namespace Api.Service.AdminServices;

public interface IAdminService
{
    Task<bool> CreateAccountAsync(string phoneNumber, string password, RoleName roleName, string username,
        string name, CancellationToken ct);

    Task<bool> CreateClassroomAsync(SubjectName subject, int teacherId, string description, int numberOfStudent,
        List<StudySessionAdminDto> studySessions);

    Task<List<StudentInformationDto>> GetStudentsInformationAsync(CancellationToken ct);
    Task<List<TeacherInformationDto>> GetTeachersInformationAsync(CancellationToken ct);
}