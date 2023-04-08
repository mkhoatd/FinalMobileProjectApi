using Data.Entities.Interfaces;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Data.Entities;

public class Student : User
{
}

public class StudentEntityConfiguration : IEntityTypeConfiguration<Student>
{
    public void Configure(EntityTypeBuilder<Student> builder)
    {
        builder.HasMany(s => s.Classrooms).WithMany(c => c.Students);
    }
}