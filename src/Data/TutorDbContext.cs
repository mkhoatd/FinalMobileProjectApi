using System.Reflection;

using Data.Entities;
using Data.Entities.Interfaces;

using Microsoft.EntityFrameworkCore;

namespace Data;

public class TutorDbContext : DbContext
{
    public TutorDbContext(DbContextOptions<TutorDbContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }

    public DbSet<Student> Students { get; set; } = null!;
    public DbSet<Teacher> Teachers { get; set; } = null!;
    public DbSet<Classroom> Classrooms { get; set; } = null!;
    public DbSet<StudySession> Sessions { get; set; } = null!;
    public DbSet<User> Users { get; set; } = null!;
    public DbSet<Admin> Admins { get; set; } = null!;
}