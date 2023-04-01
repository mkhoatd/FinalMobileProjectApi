using Data;
using Data.Entities;

using FastEndpoints.Security;

using Microsoft.EntityFrameworkCore;

namespace Api.Service.AuthService;

public class AuthService : IAuthService
{
    private readonly IConfiguration _config;
    private readonly TutorDbContext _context;
    private User? _user;
    private bool _isCredentialValid;
    
    private string _phone;
    private string _password;

    public AuthService(IConfiguration config, TutorDbContext context)
    {
        _config = config;
        _context = context;
    }

    public async Task<bool> CredentialAreValidAsync(string phone, string password)
    {
        this._phone = phone;
        this._password = password;
        var user = await _context.Users.AsNoTracking().Where(u => u.Phone == phone).FirstOrDefaultAsync();
        this._user = user;
        if (user == null)
        {
            this._isCredentialValid = false;
            return false;
            
        }
        this._isCredentialValid = UserUtility.VerifyPassword(_password, user.PasswordHash, user.PasswordSalt);
        return this._isCredentialValid;
    }
    public string? CreateToken()
    {
        if (!this._isCredentialValid)
        {
            return null;
        }
        var jwtToken = JWTBearer.CreateToken(
            signingKey: _config["Token:Key"]!,
            expireAt: DateTime.UtcNow.AddDays(1),
            issuer: _config["Token:Issuer"],
            priviledges: u =>
            {
                u.Claims.Add(new("PhoneNumber", this._phone));
                u["UserID"] = this._user.Id.ToString(); //indexer based claim setting
            });
        return jwtToken;
    }
}