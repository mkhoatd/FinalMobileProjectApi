using FastEndpoints.Security;

using OneOf;
using OneOf.Monads;
using OneOf.Types;

namespace Api.Service.AuthService;

public interface IAuthService
{
    // Task<bool> CredentialAreValidAsync(string phoneNumber, string password);
    Task<Option<String>> CheckCredentialAndCreateTokenAsync(string phoneNumber, string password);
}