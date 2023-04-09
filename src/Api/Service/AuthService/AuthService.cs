using Data;
using Data.Entities;
using Data.Entities.Interfaces;

using FastEndpoints.Security;

using Microsoft.EntityFrameworkCore;

using OneOf.Monads;

using None = OneOf.Monads.None;

namespace Api.Service.AuthService;

public class AuthService : IAuthService
{
    private readonly IConfiguration _config;
    private readonly TutorDbContext _context;
    private Option<User> _user;
    private bool _isCredentialValid;


    public AuthService(IConfiguration config, TutorDbContext context)
    {
        _config = config;
        _context = context;
    }

    public async Task<Option<string>> CheckCredentialAndCreateTokenAsync(string phoneNumber, string password)
    {
        await this.CredentialAreValidAsync(phoneNumber, password);
        return this.CreateToken();
    }

    private async Task<bool> CredentialAreValidAsync(string phone, string password)
    {
        var user = await _context.Users.AsNoTracking().Where(u => u.Phone == phone).FirstOrDefaultAsync();
        this._user = user != null ? user : new None();
        this._isCredentialValid =
            this._user
                .Bind(some => UserUtility.VerifyPassword(password, some.PasswordHash, some.PasswordSalt).ToOption())
                .Match(
                    none => false,
                    some => some.Value);
        return this._isCredentialValid;
    }

    private Option<String> CreateToken()
    {
        return this._user.Match(
            none => (Option<string>)(new None()),
            some => JwtToken(some.Value));
    }

    private Option<string> JwtToken(User user)
    {
        var jwtToken = JWTBearer.CreateToken(
            signingKey: _config["Token:Key"]!,
            expireAt: DateTime.UtcNow.AddHours(1),
            issuer: _config["Token:Issuer"],
            priviledges: u =>
            {
                u.Claims.Add(new("PhoneNumber", user.Phone));
                u["UserID"] = user.Id.ToString();
                u["Role"] = user.RoleName.ToString();
                u.Roles.Add(user.RoleName.ToString());
            });
        return jwtToken;
    }
}