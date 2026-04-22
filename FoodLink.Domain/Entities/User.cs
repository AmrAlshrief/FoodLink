using FoodLink.Domain.Common;
using FoodLink.Domain.Enums;
using FoodLink.Domain.Entities.Profiles;
using FoodLink.Domain.Common.Exceptions;

namespace FoodLink.Domain.Entities;

public class User : AuditableEntity
{
    public string Name { get; private set; }
    public string Email { get; private set; }
    public string PasswordHash { get; private set; }
    public UserRole Role { get; private set; }
    public string Phone { get; private set; }
    public string? ProfileImage { get; private set; }

    private User(){}

     private User(string name, string email, string passwordHash, UserRole role, string phone)
        : base(Guid.NewGuid())
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new DomainException("Name is required");

        if (string.IsNullOrWhiteSpace(email))
            throw new DomainException("Email is required");

        if (string.IsNullOrWhiteSpace(passwordHash))
            throw new DomainException("Password is required");

        if (string.IsNullOrWhiteSpace(phone))
            throw new DomainException("Phone is required");

        Name = name;
        Email = email;
        PasswordHash = passwordHash;
        Role = role;
        Phone = phone;
    }

    public static User Create(string name, string email, string passwordHash, UserRole role, string phone)
    {
        return new User(name, email, passwordHash, role, phone);
    }

    public void UpdateName(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new DomainException("Name cannot be empty");

        Name = name;
    }

    public void UpdatePhone(string phone)
    {
        if (string.IsNullOrWhiteSpace(phone))
            throw new DomainException("Phone cannot be empty");

        Phone = phone;
    }

    public bool IsAdmin() => Role == UserRole.Admin;

    public void SetProfileImage(string imageUrl)
    {
        ProfileImage = imageUrl;
    }

}