using Data.Entities;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Data.Seed;

public static class Seed
{
    public static async Task SeedDbContextAsync(this TutorDbContext @this, ILogger<TutorDbContext> logger)
    {
        await SeedRoleAsync(@this, logger);
        await SeedUserAsync(@this, logger);
    }

    private static async Task SeedUserAsync(TutorDbContext @this, ILogger<TutorDbContext> logger)
    {
        if (await @this.Users.AnyAsync())
        {
            logger.LogInformation("Seed users skipped");
            return;
        }
        var roles = await @this.Roles.ToListAsync();
        logger.LogInformation(message: roles.Select(r => r.Id).ToList().ToString());
        (byte[] hash1, byte[] salt1) = UserUtility.HashPassword("123456789");
        (byte[] hash2, byte[] salt2) = UserUtility.HashPassword("123456789");
        var users = new List<User>
        {
            new User
            {
                Username = "cogiaothao",
                Address = "Nhật Bản",
                Email = "cogiaothao@gmail.com",
                Name = "Cô giáo Thảo",
                Phone = "0969969696",
                Role = roles[0],
                RoleId = roles[0].Id,
                Avatar = "https://tinhayvip.com/wp-content/uploads/2022/10/top-dien-vien-jav-an-khach-nhat-bookmate-9-2022-4.jpg",
                PasswordHash = hash1,
                PasswordSalt = salt1
            },
            new User
            {
                Username = "dthle",
                Address = "Đà Nẵng",
                Email = "dthle@gmail.com",
                Name = "Lê Đức Thọ",
                Phone = "0969555444",
                Role = roles[1],
                RoleId = roles[1].Id,
                Avatar = "https://tinhayvip.com/wp-content/uploads/2022/10/top-dien-vien-jav-an-khach-nhat-bookmate-9-2022-4.jpg",
                PasswordHash = hash2,
                PasswordSalt = salt2
            },
        };
        await @this.Users.AddRangeAsync(users);
        await @this.SaveChangesAsync();
        logger.LogInformation("Seed users success");
        users = await @this.Users.ToListAsync();
        logger.LogInformation(message: users.Select(u => u.Id).ToList().ToString());
    }

    private static async Task SeedRoleAsync(TutorDbContext @this, ILogger<TutorDbContext> logger)
    {
        if (await @this.Roles.AnyAsync())
        {
            logger.LogInformation("Seed Roles skipped");
            return;
        }
        var roles = new List<Role>
        {
            new Role { Name = "Giáo viên", Users = new() },
            new Role { Name = "Học sinh", Users = new() }
        };
        await @this.Roles.AddRangeAsync(roles);
        await @this.SaveChangesAsync();
        logger.LogInformation("Seed Roles success");
    }
}