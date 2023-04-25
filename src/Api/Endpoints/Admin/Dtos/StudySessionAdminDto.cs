namespace Api.Endpoints.Admin.Dtos;

public record StudySessionAdminDto
{
    public required DayOfWeek DayOfWeek { get; init; }
    public required TimeSpan StartTime { get; init; }
    public required TimeSpan EndTime { get; init; }
}