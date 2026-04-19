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

    public bool IsAdmin() => Role == UserRole.Admin;

    public void SetProfileImage(string imageUrl)
    {
        ProfileImage = imageUrl;
    }

    // private User(string name, string email, string passwordHash, UserRole role, string phone, string? profileImage) 
    //         : base(Guid.NewGuid())
    // {
    //     Name = name;
    //     Email = email;
    //     PasswordHash = passwordHash;
    //     Role = role;
    //     Phone = phone;
    //     ProfileImage = profileImage;
    // }

    // public static User Create(string name, string email, string passwordHash, UserRole role, string phone, string? profileImage)
    // {
    //     return new User(name, email, passwordHash, role, phone, profileImage);
    // }

    // public bool IsAdmin() => Role == UserRole.Admin;

    // public BusinessProfile CreateBusinessProfile(
    //     string businessName,
    //     string address,
    //     string businessType)
    // {
    //     if (IsAdmin())
    //         throw new DomainException("Admin cannot have profile");

    //     return BusinessProfile.Create(Id, businessName, address, businessType);
    // }

    // public CharityProfile CreateCharityProfile(Guid UserId,
    //     string organizationName,
    //     string licenseNumber,
    //     string address)
    // {
    //     if (IsAdmin())
    //         throw new DomainException("Admin cannot have profile");

    //     return CharityProfile.Create(UserId, organizationName, licenseNumber, address);
    // }
}