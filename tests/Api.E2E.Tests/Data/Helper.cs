using Data;
using Data.Entities;

using Microsoft.EntityFrameworkCore;

namespace FinalMobileProjectApi.E2E.Tests.Data;

public static class Helper
{
    public static async Task SeedDbContextTestAsync(this TutorDbContext @this)
    {
        await SeedRoleAsync(@this);
        await SeedUserAsync(@this);
    }

    private static async Task SeedUserAsync(TutorDbContext @this)
    {
        if (await @this.Users.AnyAsync())
        {
            return;
        }

        var roles = await @this.Roles.ToListAsync();
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
                Avatar =
                    "https://tinhayvip.com/wp-content/uploads/2022/10/top-dien-vien-jav-an-khach-nhat-bookmate-9-2022-4.jpg",
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
                Avatar =
                    "https://tinhayvip.com/wp-content/uploads/2022/10/top-dien-vien-jav-an-khach-nhat-bookmate-9-2022-4.jpg",
                PasswordHash = hash2,
                PasswordSalt = salt2
            },
        };
        await @this.Users.AddRangeAsync(users);
        await @this.SaveChangesAsync();
    }

    private static async Task SeedRoleAsync(TutorDbContext @this)
    {
        if (await @this.Roles.AnyAsync())
        {
            return;
        }

        var roles = new List<Role>
        {
            new Role { Name = "Giáo viên", Users = new() }, new Role { Name = "Học sinh", Users = new() }
        };
        await @this.Roles.AddRangeAsync(roles);
        await @this.SaveChangesAsync();
    }
}