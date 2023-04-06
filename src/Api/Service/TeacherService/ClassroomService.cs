using Api.Endpoints.Classroom;

using Data;

using Microsoft.EntityFrameworkCore;

namespace Api.Service.TeacherService;

public class ClassroomService : IClassroomService
{
    private readonly TutorDbContext _dbContext;

    public ClassroomService(TutorDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<List<ClassroomDto>> GetTeachingClassroomsDtoAsync(Guid teacherId, CancellationToken ct)
    {
        var res = await _dbContext.Classrooms
            .Where(c => c.TeacherId == teacherId)
            .Select(c => new ClassroomDto
            {
                Id = c.Id,
                Name = c.Name,
                Description = c.Description,
                StudySessions = c.StudySessions.Select(ss => new StudySessionDto
                {
                    Id = ss.Id,
                    DayOfWeek = ss.DayOfWeek.ToString(),
                    StartTime = ss.StartTime,
                    EndTime = ss.EndTime
                }).ToList()
            })
            .ToListAsync(ct);
        return res;
    }
}