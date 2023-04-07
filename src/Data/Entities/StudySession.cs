using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Data.Entities;

public class StudySession
{   
    public int Id { get; set; }
    public required DayOfWeek DayOfWeek { get; set; }
    public required TimeSpan StartTime { get; set; }
    public required TimeSpan EndTime { get; set; }
    public required int ClassroomId { get; set; }
    public required Classroom Classroom { get; set; }
}

public class StudySessionEntityConfiguration : IEntityTypeConfiguration<StudySession>
{
    public void Configure(EntityTypeBuilder<StudySession> builder)
    {
        builder.HasKey(s => s.Id);
        builder.Property(s => s.DayOfWeek)
            .HasConversion(
                d => d.ToString(),
                d => Enum.Parse<DayOfWeek>(d));
        builder.Property(s => s.StartTime)
            .HasConversion(
                t => t.ToString(),
                t => TimeSpan.Parse(t));
        builder.Property(s => s.EndTime)
            .HasConversion(
                t => t.ToString(),
                t => TimeSpan.Parse(t));
        builder.HasOne(s => s.Classroom)
            .WithMany(c => c.StudySessions)
            .HasForeignKey(s => s.ClassroomId)
            .IsRequired();
    }
}