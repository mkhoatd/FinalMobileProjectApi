using Api.Endpoints.Admin.Dtos;
using Api.Interface;
using Api.Service.AdminServices;

using Data;
using Data.Entities;

using Microsoft.EntityFrameworkCore;

namespace Api.Endpoints.Admin;

public record StudySessionInformation
{
    public required string DayOfWeek { get; init; }
    public required string StartTime { get; init; }
    public required string EndTime { get; init; }
}

public record ClassroomInformation
{
    public int Id { get; init; }
    public required SubjectName Name { get; init; }
    public required string Description { get; init; }
    public int TeacherId { get; init; }
    public int NumberOfStudent { get; init; }
    public required List<StudySessionInformation> StudySessions { get; init; }
}

public class GetClassroomsInformationResponse : BaseResponse<List<ClassroomInformation>>
{
}

public class GetClassroomsInformationEndpoint : EndpointWithoutRequest<GetClassroomsInformationResponse>
{
    private readonly TutorDbContext _dbContext;

    public GetClassroomsInformationEndpoint(TutorDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public override void Configure()
    {
        Get("/admin/classrooms");
        Roles(RoleName.Admin.ToString());
    }

    public override async Task HandleAsync(CancellationToken ct)
    {
        List<ClassroomInformation> res = await _dbContext.Classrooms.Select(c => new ClassroomInformation
        {
            Description = c.Description,
            Id = c.Id,
            Name = c.Name,
            NumberOfStudent = c.Students.Count,
            StudySessions = c.StudySessions.Select(s => new StudySessionInformation
            {
                DayOfWeek = s.DayOfWeek.ToString(),
                EndTime = s.EndTime.ToString(),
                StartTime = s.StartTime.ToString()
            }).ToList(),
            TeacherId = c.TeacherId
        }).ToListAsync(ct);
        await SendOkAsync(new GetClassroomsInformationResponse { Data = res, Message = "Success", Status = "Ok" }, ct);
    }

}