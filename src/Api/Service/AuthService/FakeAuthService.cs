using Data;
using Data.Entities;

using FastEndpoints.Security;

using Microsoft.EntityFrameworkCore;

namespace Api.Service.AuthService;

public class FakeAuthService : IAuthService
{
    private readonly IConfiguration _config;
    private readonly TutorDbContext _context;
    private User _user;
    private bool isCredentialValid = false;
    
    private string phone = "";
    private string password = "";

    public FakeAuthService(IConfiguration config, TutorDbContext context)
    {
        _config = config;
        _context = context;
    }

    public async Task<bool> CredentialAreValidAsync(string phone, string password)
    {
        this.phone = phone;
        this.password = password;
        var user = await _context.Users.Where(u => u.Phone == phone).FirstOrDefaultAsync();
        this._user = user;
        if (user == null)
        {
            this.isCredentialValid = false;
            return false;
            
        }
        this.isCredentialValid = UserUtility.VerifyPassword(password, user.PasswordHash, user.PasswordSalt);
        return this.isCredentialValid;
    }
    public string? CreateToken()
    {
        if (!this.isCredentialValid)
        {
            return null;
        }
        var jwtToken = JWTBearer.CreateToken(
            signingKey: _config["Token:Key"]!,
            expireAt: DateTime.UtcNow.AddDays(1),
            issuer: _config["Token:Issuer"],
            priviledges: u =>
            {
                u.Claims.Add(new("PhoneNumber", this.phone));
                u["UserID"] = this._user.Id.ToString(); //indexer based claim setting
            });
        return jwtToken;
    }
}