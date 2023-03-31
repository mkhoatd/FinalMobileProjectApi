using FastEndpoints.Security;

namespace Api.Service.AuthService;

public interface IAuthService
{
    Task<bool> CredentialAreValidAsync(string phoneNumber, string password);
    string? CreateToken();
}