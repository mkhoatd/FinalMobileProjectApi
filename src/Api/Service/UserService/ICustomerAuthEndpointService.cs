using Api.Endpoints.Customer.Auth;

namespace Api.Service.UserService;

public interface ICustomerAuthEndpointService
{
    Task<UserAuthDto> GetUserAsync(string phone, CancellationToken ct);
    Task<bool> SaveDeviceToken(string phoneNumber, string deviceToken, CancellationToken ct);
 
}