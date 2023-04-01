using Data.Entities;

using Microsoft.EntityFrameworkCore;

namespace Data;

public class TutorDbContext : DbContext
{
    public TutorDbContext(DbContextOptions<TutorDbContext> options) : base(options)
    {
    }
    public DbSet<User> Users { get; set; } = null!;
    public DbSet<Role> Roles { get; set; } = null!;
}