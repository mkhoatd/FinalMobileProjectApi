using Data.Entities.Interfaces;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Data.Entities;

public class Teacher : User
{
    public required List<Classroom> Classrooms { get; set; }
}

public class TeacherEntityConfiguration : IEntityTypeConfiguration<Teacher>
{
    public void Configure(EntityTypeBuilder<Teacher> builder)
    {
        builder.HasMany(t => t.Classrooms).WithOne(c => c.Teacher);
    }
}