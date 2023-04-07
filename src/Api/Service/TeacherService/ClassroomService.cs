﻿using Api.Endpoints.Classroom;
using Api.Endpoints.Classroom.Dto;

using Data;

using Microsoft.EntityFrameworkCore;

namespace Api.Service.TeacherService;

public class ClassroomService : IClassroomService
{
    private readonly static Func<TutorDbContext, int, IAsyncEnumerable<ClassroomDto>> QueryTeachingClassroomsAsync =
        EF.CompileAsyncQuery(((TutorDbContext context, int teacherId) =>
            from c in context.Classrooms
            where c.TeacherId == teacherId && c.IsDeleted == false
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
                    }).ToList(),
            }));

    private readonly TutorDbContext _dbContext;

    public ClassroomService(TutorDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<List<ClassroomDto>> GetTeachingClassroomsDtoAsync(int teacherId, CancellationToken ct)
    {
        var res = new List<ClassroomDto>();
        await foreach (var item in QueryTeachingClassroomsAsync(_dbContext, teacherId).WithCancellation(ct))
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