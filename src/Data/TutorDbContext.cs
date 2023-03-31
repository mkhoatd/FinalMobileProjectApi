using Data.Entities;

using Microsoft.EntityFrameworkCore;

namespace Data;

public class TutorDbContext : DbContext
{
    public TutorDbContext(DbContextOptions<TutorDbContext> options) : base(options)
    {
    }
    public required DbSet<User> Users { get; set; }
    public required DbSet<Role> Roles { get; set; }
}