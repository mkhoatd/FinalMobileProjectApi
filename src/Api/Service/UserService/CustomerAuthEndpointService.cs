using Api.Endpoints.Customer.Auth;

using Data;
using Data.Entities;
using Data.Entities.Interfaces;

using Microsoft.EntityFrameworkCore;

using OneOf;

namespace Api.Service.UserService;

public class CustomerAuthEndpointService : ICustomerAuthEndpointService
{
    private readonly TutorDbContext _dbContext;


    public CustomerAuthEndpointService(TutorDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<UserAuthDto> GetUserAsync(string phone, CancellationToken ct)
    {
        var user = await this._dbContext.Users
            .Where(u => u.Phone == phone)
            .Select(u => new UserAuthDto
            {
                Id = u.Id,
                Avatar = u.Avatar,
                Email = u.Email,
                Name = u.Name,
                Phone = u.Phone,
                Role = u.RoleName.ToString(),
                Username = u.Username
            }).FirstOrDefaultAsync(ct);
        return user!;
    }

    public async Task<bool> SaveDeviceToken(string phoneNumber, string deviceToken, CancellationToken ct)
    {
        var user = await _dbContext.Users.Where(u => u.Phone == phoneNumber).FirstOrDefaultAsync(ct);
        user!.DeviceToken = deviceToken;
        return await _dbContext.SaveChangesAsync(ct) > 0;
    }
}