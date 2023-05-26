namespace Api.Endpoints.Classrooms.Dto;

public record ClassroomDto
{
    public int Id { get; init; }
    public string Name { get; init; }

    public string Description { get; init; }
    public int ClassroomSize { get; init; }
    public List<StudySessionDto> StudySessions { get; init; }
};