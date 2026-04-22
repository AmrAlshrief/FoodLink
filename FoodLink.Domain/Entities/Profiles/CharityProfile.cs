using FoodLink.Domain.Common;
using FoodLink.Domain.Enums;
using FoodLink.Domain.Common.Exceptions;
namespace FoodLink.Domain.Entities.Profiles;

public class CharityProfile : Profile
{
    public string Name { get; private set; }
    public string LicenseNumber { get; private set; }
    public string Address { get; private set; }
    public int NoShowCount { get; private set; }

    private CharityProfile() { }

    internal CharityProfile(Guid userId, string name, string license, string address) 
        : base(Guid.NewGuid())
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new DomainException("Name is required");

        if (string.IsNullOrWhiteSpace(license))
            throw new DomainException("License number is required");

        if (string.IsNullOrWhiteSpace(address))
            throw new DomainException("Address is required");


        UserId = userId;
        Name = name;
        LicenseNumber = license;
        Address = address;
    }

    public static CharityProfile Create(Guid userId, string name, string license, string address)
    {
        return new CharityProfile(userId, name, license, address);
    }

    public void UpdateDetails(string name, string address)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new DomainException("Name is required");

        if (string.IsNullOrWhiteSpace(address))
            throw new DomainException("Address is required");

        Name = name;
        Address = address;
    }

    public void MarkNoShow()
    {
        NoShowCount++;
    }

    
}