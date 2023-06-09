﻿using System.Security.Cryptography;
using System.Text;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Data.Entities.Interfaces;

public abstract class User
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public required RoleName RoleName { get; set; }
    public required string Email { get; set; }
    public required string Avatar { get; set; }
    public required string Phone { get; set; }
    public required string Address { get; set; }
    public required string Username { get; set; }
    public required byte[] PasswordHash { get; set; }
    public required byte[] PasswordSalt { get; set; }
    public required List<Classroom> Classrooms { get; set; }

    public string? DeviceToken { get; set; }
}

public class UserEntityConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.HasKey(u => u.Id);
        builder.HasIndex(u => u.Phone).IsUnique();
        builder.HasDiscriminator(u => u.RoleName)
            .HasValue<Teacher>(RoleName.Teacher)
            .HasValue<Student>(RoleName.Student)
            .HasValue<Admin>(RoleName.Admin);
    }
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
        return !computedHash.Where((t, i) => t != hash[i]).Any();
    }
}