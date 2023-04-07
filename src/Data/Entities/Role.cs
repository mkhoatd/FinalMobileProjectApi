using Data.Entities.Interfaces;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Data.Entities;

public enum RoleName
{
    Teacher,
    Student,
    Admin,
}

public class Role
{
    public int Id { get; set; }
    public required RoleName Name { get; set; }
    public required List<User> Users { get; set; } 
}

