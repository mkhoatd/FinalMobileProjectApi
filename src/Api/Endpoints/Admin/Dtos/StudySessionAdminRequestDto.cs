namespace Api.Endpoints.Admin.Dtos;

public record StudySessionAdminRequestDto
{
    public required string DayOfWeek { get; init; }
    public required string StartTime { get; init; }
    public required string EndTime { get; init; }
}