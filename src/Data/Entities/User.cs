using System.ComponentModel.DataAnnotations.Schema;
using System.Security.Cryptography;
using System.Text;

using Microsoft.EntityFrameworkCore;

namespace Data.Entities;

[PrimaryKey(nameof(Id))]
[Index(nameof(Phone), IsUnique = true)]
public class User
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public required string Email { get; set; }
    public required string Avatar { get; set; }
    public required string Phone { get; set; }
    public required string Address { get; set; }
    public required string Username { get; set; }
    public required byte[] PasswordHash { get; set; }
    public required byte[] PasswordSalt { get; set; }
    public int RoleId { get; set; }
    [ForeignKey("RoleId")]
    public required Role Role{ get; set; }
}

public static class UserUtility
{
    public static (byte[] hash, byte[] salt) HashPassword(string password)
    {
        using var hmac = new HMACSHA512();
        var hash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
        var salt = hmac.Key;
        return (hash, salt);
    }
    public static bool VerifyPassword(string password, byte[] hash, byte[] salt)
    {
        using var hmac = new HMACSHA512(salt);
        var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
        for (var i = 0; i < computedHash.Length; i++)
        {
            if (computedHash[i] != hash[i])
            {
                return false;
            }
        }
        return true;
    }
}