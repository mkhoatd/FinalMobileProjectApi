using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Data.Entities;

public class Classroom
{
    public required Guid Id { get; set; }
    public required string Name { get; set; }
    public required string Description { get; set; }
    public required List<Student> Students { get; set; }
    public required Guid TeacherId { get; set; }
    public required Teacher Teacher { get; set; }
    public required List<StudySession> StudySessions { get; set; }
}

public class ClassroomEntityConfiguration : IEntityTypeConfiguration<Classroom>
{
    public void Configure(EntityTypeBuilder<Classroom> builder)
    {
        builder.HasKey(u => u.Id);
        builder
            .HasMany(c => c.Students)
            .WithMany(s => s.Classrooms);
        builder.HasOne(c => c.Teacher)
            .WithMany(t => t.Classrooms)
            .HasForeignKey(c => c.TeacherId)
            .IsRequired();
        builder.HasMany(c => c.StudySessions)
            .WithOne(s => s.Classroom);
    }
}