namespace Api.Endpoints.Classroom;

public record StudySessionDto

{
    public Guid Id { get; init; }
    public string DayOfWeek { get; init; }

    public TimeSpan StartTime { get; init; }
    public TimeSpan EndTime { get; init; }
}