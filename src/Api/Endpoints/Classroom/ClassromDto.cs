namespace Api.Endpoints.Classroom;

public record ClassroomDto
{
    public Guid Id { get; init; }
    public string Name { get; init; }

    public string Description { get; init; }
    public List<StudySessionDto> StudySessions { get; init; }
};