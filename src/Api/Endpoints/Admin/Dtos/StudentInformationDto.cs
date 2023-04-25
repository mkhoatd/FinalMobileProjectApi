using Api.Endpoints.Classroom.Dto;

namespace Api.Endpoints.Admin.Dtos;

public record StudentInformationDto
{
    public required int Id { get; init; }
    public required string Name { get; init; }
    public required string Username { get; init; }
    public required string PhoneNumber { get; init; }
    public required List<StudySessionDto> StudySessions { get; init; }
}