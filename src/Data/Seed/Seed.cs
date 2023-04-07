using Bogus;

using Data.Entities;
using Data.Entities.Interfaces;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Data.Seed;

public static class Seed
{
    public static async Task SeedDbContextAsync(this TutorDbContext @this, int userNumber,
        ILogger<TutorDbContext>? logger = null)
    {
        // await SeedRoleAsync(@this, logger).ConfigureAwait(false);
        // await SeedUserAsync(@this, 2, logger).ConfigureAwait(false);
        await @this.SeedUserAsync(userNumber, logger);
        await @this.SeedClassroomAsync(20, logger);
    }

    private static async Task SeedClassroomAsync(this TutorDbContext @this, int numClass,
        ILogger<TutorDbContext>? logger)
    {
        if (await @this.Classrooms.AnyAsync())
        {
            logger?.LogInformation("Seed classroom skipped");
            return;
        }

        var teachers = await @this.Teachers.ToListAsync();
        var students = await @this.Students.ToListAsync();
        var classroomFaker = new Faker<Classroom>()
            .RuleFor(c => c.Name, f => (SubjectName)f.Random.Int(0, 5))
            .RuleFor(c => c.Description, f => f.Lorem.Sentence())
            .RuleFor(c => c.TeacherId, f =>
            {
                var id = f.Random.Int(0, teachers.Count - 1);
                var res = teachers[id].Id;
                return res;
            })
            .RuleFor(c => c.Students, f => students.Where(s => f.Random.Bool(0.5f)).ToList())
            .RuleFor(c => c.IsDeleted, f => false);
        
        var classrooms = classroomFaker.Generate(numClass);
        var studySessionFaker = new Faker<StudySession>()
            .RuleFor(c => c.DayOfWeek, f =>
            {
                var index = f.Random.Int(0, 6);
                return (DayOfWeek)index;
            })
            .RuleFor(c => c.StartTime, f =>
            {
                var hour = f.Random.Int(7, 19);
                var minute = f.Random.Int(0, 1) * 30;
                return new TimeSpan(hour, minute, 0);
            });
        for (int i = 0; i < classrooms.Count; i++)
        {
            Classroom? classroom = classrooms[i];
            var sessions = studySessionFaker.Generate(3);
            foreach (var session in sessions)
            {
                var hours = 1;
                var minute = 30;
                session.EndTime = session.StartTime + new TimeSpan(hours, minute, 0);
            }

            while (sessions.DistinctBy(s => s.DayOfWeek).Count() != 3)
            {
                sessions = studySessionFaker.Generate(3);

                foreach (var session in sessions)
                {
                    var hours = 1;
                    var minute = 30;
                    session.EndTime = session.StartTime + new TimeSpan(hours, minute, 0);
                }
            }

            classroom.StudySessions = sessions;
        }

        await @this.Classrooms.AddRangeAsync(classrooms);
        await @this.SaveChangesAsync();
    }


    private static async Task SeedUserAsync(this TutorDbContext @this, int userNumber, ILogger<TutorDbContext>? logger)
    {
        if ((await @this.Students.AnyAsync().ConfigureAwait(false)) && (await @this.Teachers.AnyAsync()))
        {
            logger?.LogInformation("Seed users skipped");
            return;
        }

        var studentFaker = new Faker<Student>()
            .RuleFor(u => u.Username, f => f.Internet.UserName())
            .RuleFor(u => u.Address, f => f.Address.FullAddress())
            .RuleFor(u => u.Email, f => f.Internet.Email())
            .RuleFor(u => u.Avatar, f => f.Internet.Avatar())
            .RuleFor(u => u.Name, f => f.Name.FullName())
            .RuleFor(u => u.Phone, f => f.Phone.PhoneNumber("##########"));

        var teacherFaker = new Faker<Teacher>()
            .RuleFor(u => u.Username, f => f.Internet.UserName())
            .RuleFor(u => u.Address, f => f.Address.FullAddress())
            .RuleFor(u => u.Email, f => f.Internet.Email())
            .RuleFor(u => u.Avatar, f => f.Internet.Avatar())
            .RuleFor(u => u.Name, f => f.Name.FullName())
            .RuleFor(u => u.Phone, f => f.Phone.PhoneNumber("##########"));

        List<Student> students = new();
        for (int i = 0; i < userNumber; i++)
        {
            var student = studentFaker.Generate();
            (byte[] hash, byte[] salt1) = UserUtility.HashPassword("123456789");
            student.PasswordHash = hash;
            student.PasswordSalt = salt1;
            students.Add(student);
        }

        List<Teacher> teachers = new();
        for (int i = 0; i < userNumber * 0.1; i++)
        {
            var teacher = teacherFaker.Generate();
            (byte[] hash, byte[] salt1) = UserUtility.HashPassword("123456789");
            teacher.PasswordHash = hash;
            teacher.PasswordSalt = salt1;
            teachers.Add(teacher);
        }

        await @this.Students.AddRangeAsync(students);
        await @this.Teachers.AddRangeAsync(teachers);
        await @this.SaveChangesAsync().ConfigureAwait(false);
        logger?.LogInformation("Seed users success");
    }
}