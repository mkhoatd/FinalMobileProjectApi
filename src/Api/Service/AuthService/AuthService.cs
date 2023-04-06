using Data;
using Data.Entities;
using Data.Entities.Interfaces;

using FastEndpoints.Security;

using Microsoft.EntityFrameworkCore;

namespace Api.Service.AuthService;

public class AuthService : IAuthService
{
    private readonly IConfiguration _config;
    private readonly TutorDbContext _context;
    private Student? _student;
    private Teacher? _teacher;
    private User? _user;
    private string _role;
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
        var student = await _context.Students.AsNoTracking().Where(u => u.Phone == phone).FirstOrDefaultAsync()
            .ConfigureAwait(false);
        var teacher = await _context.Teachers.AsNoTracking().Where(u => u.Phone == phone).FirstOrDefaultAsync()
            .ConfigureAwait(false);
        if (student == null)
        {
            this._user = teacher;
            this._role = RoleName.Teacher.ToString();
        }
        else
        {
            this._user = student;
            this._role = RoleName.Student.ToString();
        }

        if (this._user == null)
        {
            this._isCredentialValid = false;
            return false;
        }

        this._isCredentialValid =
            UserUtility.VerifyPassword(_password, this._user.PasswordHash, this._user.PasswordSalt);
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
            expireAt: DateTime.UtcNow.AddHours(1),
            issuer: _config["Token:Issuer"],
            priviledges: u =>
            {
                u.Claims.Add(new("PhoneNumber", this._phone));
                u["UserID"] = this._user?.Id.ToString() ?? string.Empty; //indexer based claim setting
                u["Role"] = this._role;
            });
        return jwtToken;
    }
}