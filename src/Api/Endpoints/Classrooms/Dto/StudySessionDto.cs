namespace Api.Endpoints.Classrooms.Dto;

public record StudySessionDto

{
    public int Id { get; init; }
    public string DayOfWeek { get; init; }

    public TimeSpan StartTime { get; init; }
    public TimeSpan EndTime { get; init; }
}