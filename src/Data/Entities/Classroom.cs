using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Data.Entities;

public enum SubjectName
{
    Toán,
    Văn,
    Anh,
    Lý,
    Hóa,
    Sinh
}

public class Classroom
{
    public int Id { get; set; }
    public required SubjectName Name { get; set; }
    public required string Description { get; set; }
    public required List<Student> Students { get; set; }
    public int TeacherId { get; set; }
    public required Teacher Teacher { get; set; }
    public required List<StudySession> StudySessions { get; set; }
    public bool IsDeleted { get; set; }
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
        builder.Property(c => c.Name)
            .HasConversion(
                n => n.ToString(),
                s => Enum.Parse<SubjectName>(s));
    }
}