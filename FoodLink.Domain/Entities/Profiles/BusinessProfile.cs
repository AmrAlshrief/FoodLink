using FoodLink.Domain.Common;
using FoodLink.Domain.Enums;
using FoodLink.Domain.Common.Exceptions;
namespace FoodLink.Domain.Entities.Profiles;

public class BusinessProfile : Profile
{
    public string BusinessName { get; private set;}
    public string Address { get; private set; }
    public string BusinessType { get; private set; }
    
    private BusinessProfile() { }

    private BusinessProfile(Guid userId, string businessName, string address, string businessType)
            : base(Guid.NewGuid())
    {
        if (string.IsNullOrWhiteSpace(businessName))
            throw new DomainException("Business name is required");

        if (string.IsNullOrWhiteSpace(address))
            throw new DomainException("Address is required");

        if (string.IsNullOrWhiteSpace(businessType))
            throw new DomainException("Business type is required");


        UserId = userId;
        BusinessName = businessName;
        Address = address;
        BusinessType = businessType;
    }

    public static BusinessProfile Create(Guid userId, string businessName, string address, string businessType)
    {
        return new BusinessProfile(userId, businessName, address, businessType);
    }

    public void UpdateDetails(string businessName, string address, string businessType)
    {
        if (string.IsNullOrWhiteSpace(businessName))
            throw new DomainException("Business name is required");

        if (string.IsNullOrWhiteSpace(address))
            throw new DomainException("Address is required");

        if (string.IsNullOrWhiteSpace(businessType))
            throw new DomainException("Business type is required");

        BusinessName = businessName;
        Address = address;
        BusinessType = businessType;
    }
}