﻿using Api.Endpoints.Admin.Dtos;
using Api.Endpoints.Classroom.Dto;

using Bogus;

using Data;
using Data.Entities;
using Data.Entities.Interfaces;

using Microsoft.EntityFrameworkCore;

using OneOf;
using OneOf.Monads;

namespace Api.Service.AdminServices;

public class AdminService : IAdminService
{
    private readonly TutorDbContext _dbContext;


    public AdminService(TutorDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<bool> CreateAccountAsync(string phoneNumber, string password, RoleName roleName,
        string username, string name, CancellationToken ct)
    {
        if (await _dbContext.Users.AnyAsync(u => u.Phone == phoneNumber, ct))
        {
            return false;
        }

        if (await _dbContext.Users.AnyAsync(u => u.Username == username, ct))
        {
            return false;
        }

        if (await _dbContext.Users.AnyAsync(u => u.Name == name, ct))
        {
            return false;
        }

        (byte[] passwordHash, byte[] passwordSalt) = UserUtility.HashPassword(password);
        Faker faker = new();
        OneOf<Admin, Teacher, Student> user = roleName switch
        {
            RoleName.Admin => new Admin
            {
                Address = "",
                RoleName = roleName,
                Username = username,
                Name = name,
                Phone = phoneNumber,
                PasswordHash = passwordHash,
                PasswordSalt = passwordSalt,
                Email = faker.Internet.Email(),
                Classrooms = new List<Classroom>(),
                DeviceToken = "",
                Avatar = faker.Internet.Avatar()
            },
            RoleName.Student => new Student
            {
                Address = "",
                RoleName = roleName,
                Username = username,
                Name = name,
                Phone = phoneNumber,
                PasswordHash = passwordHash,
                PasswordSalt = passwordSalt,
                Email = faker.Internet.Email(),
                Classrooms = new List<Classroom>(),
                DeviceToken = "",
                Avatar = faker.Internet.Avatar()
            },
            RoleName.Teacher => new Teacher
            {
                Address = "",
                RoleName = roleName,
                Username = username,
                Name = name,
                Phone = phoneNumber,
                PasswordHash = passwordHash,
                PasswordSalt = passwordSalt,
                Email = faker.Internet.Email(),
                Classrooms = new List<Classroom>(),
                DeviceToken = "",
                Avatar = faker.Internet.Avatar()
            }
        };
        user.Match(
            admin =>
            {
                _dbContext.Admins.Add(admin);
                return new None();
            },
            teacher =>
            {
                _dbContext.Teachers.Add(teacher);
                return new None();
            },
            student =>
            {
                _dbContext.Students.Add(student);
                return new None();
            }
        );
        await _dbContext.SaveChangesAsync(ct);
        return true;
    }

    public async Task<bool> CreateClasssroomAsync(SubjectName subject, int teacherId, string description,
        List<int> studentIds, List<StudySessionAdminDto> studySessions)
    {
        List<Student> students = await _dbContext.Students.Where(s => studentIds.Contains(s.Id)).ToListAsync();
        Classroom classroom = new()
        {
            Name = subject,
            TeacherId = teacherId,
            Description = description,
            Students = students,
            StudySessions = new List<StudySession>()
        };
        foreach (StudySessionAdminDto studySession in studySessions)
        {
            classroom.StudySessions.Add(new StudySession
            {
                DayOfWeek = studySession.DayOfWeek,
                StartTime = studySession.StartTime,
                EndTime = studySession.EndTime
            });
        }

        _dbContext.Classrooms.Add(classroom);
        await _dbContext.SaveChangesAsync();
        return true;
    }

    public async Task<List<StudentInformationDto>> GetStudentsInformationAsync(CancellationToken ct)
    {
        List<StudentInformationDto> students = await _dbContext.Students.Select(s => new StudentInformationDto
        {
            Id = s.Id,
            Name = s.Name,
            PhoneNumber = s.Phone,
            Username = s.Username,
            StudySessions = s.Classrooms.SelectMany(c => c.StudySessions).Select(ss => new StudySessionDto
            {
                Id = ss.Id, DayOfWeek = ss.DayOfWeek.ToString(), StartTime = ss.StartTime, EndTime = ss.EndTime
            }).ToList()
        }).ToListAsync(ct);
        return students;
    }

    public async Task<List<TeacherInformationDto>> GetTeachersInformationAsync(CancellationToken ct)
    {
        List<TeacherInformationDto> teachers = await _dbContext.Teachers.Select(s => new TeacherInformationDto
        {
            Id = s.Id,
            Name = s.Name,
            PhoneNumber = s.Phone,
            Username = s.Username,
            TeachingSessions = s.Classrooms.SelectMany(c => c.StudySessions).Select(ss => new StudySessionDto
            {
                Id = ss.Id, DayOfWeek = ss.DayOfWeek.ToString(), StartTime = ss.StartTime, EndTime = ss.EndTime
            }).ToList()
        }).ToListAsync(ct);
        return teachers;
    }
}