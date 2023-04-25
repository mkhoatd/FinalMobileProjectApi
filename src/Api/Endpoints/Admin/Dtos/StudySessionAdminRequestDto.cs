namespace Api.Endpoints.Admin.Dtos;

public record StudySessionAdminRequestDto
{
    public required string DayOfWeek { get; init; }
    public required TimeSpan StartTime { get; init; }
    public required TimeSpan EndTime { get; init; }
}