using Api.Endpoints.Customer.Auth;

using Data;

using Microsoft.EntityFrameworkCore;

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
            .Select(u => new UserAuthDto(u.Id, u.Name, u.Email, u.Avatar, u.Phone, u.Username, u.Role.Name))
            .FirstOrDefaultAsync(cancellationToken: ct);
        return user!;
    }
}