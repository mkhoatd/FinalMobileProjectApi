using Api.Endpoints.Classrooms.Dto;

namespace Api.Endpoints.Admin.Dtos;

public record TeacherInformationDto
{
    public required int Id { get; init; }
    public required string Name { get; init; }
    public required string Username { get; init; }
    public required string Avatar { get; init; }

    public required string PhoneNumber { get; init; }
    public required List<StudySessionDto> TeachingSessions { get; init; }
}