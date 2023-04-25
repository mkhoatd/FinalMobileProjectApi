using Api.Endpoints.Classroom.Dto;

using Data;

using Microsoft.EntityFrameworkCore;

namespace Api.Service.ClassroomServices;

public class ClassroomService : IClassroomService
{
    private static readonly Func<TutorDbContext, int, IAsyncEnumerable<ClassroomDto>> QueryUserClassroomsAsync =
        EF.CompileAsyncQuery((TutorDbContext context, int userId) =>
            from c in context.Classrooms
            where c.TeacherId == userId || c.Students.Any(s => s.Id == userId)
            select new ClassroomDto
            {
                Id = c.Id,
                Name = c.Name.ToString(),
                Description = c.Description,
                ClassroomSize = c.Students.Count,
                StudySessions = (from ss in c.StudySessions
                    select new StudySessionDto
                    {
                        Id = ss.Id,
                        DayOfWeek = ss.DayOfWeek.ToString(),
                        StartTime = ss.StartTime,
                        EndTime = ss.EndTime
                    }).ToList()
            });

    private readonly TutorDbContext _dbContext;

    public ClassroomService(TutorDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<List<ClassroomDto>> GetClassroomsDtoAsync(int userId, CancellationToken ct)
    {
        List<ClassroomDto> res = new();
        await foreach (ClassroomDto item in QueryUserClassroomsAsync(_dbContext, userId).WithCancellation(ct))
        {
            res.Add(item);
        }

        return res;
    }

    public async Task<List<ClassroomDto>> GetTeachingClassroomsDtoAsync(int teacherId, CancellationToken ct)
    {
        List<ClassroomDto> res = new();
        await foreach (ClassroomDto item in QueryUserClassroomsAsync(_dbContext, teacherId).WithCancellation(ct))
        {
            res.Add(item);
        }

        return res;
        // var res = await _dbContext.Classrooms
        //     .Where(c => c.TeacherId == teacherId)
        //     .Select(c => new ClassroomDto
        //     {
        //         Id = c.Id,
        //         Name = c.Name,
        //         Description = c.Description,
        //         StudySessions = c.StudySessions.Select(ss => new StudySessionDto
        //         {
        //             Id = ss.Id,
        //             DayOfWeek = ss.DayOfWeek.ToString(),
        //             StartTime = ss.StartTime,
        //             EndTime = ss.EndTime
        //         }).ToList()
        //     })
        //     .ToListAsync(ct);
        // return res;
    }
}