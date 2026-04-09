using FoodLink.Domain.Enums;
namespace FoodLink.Domain.Entities.Profiles;

public class BusinessProfile : Profile
{
    public string BusinessName { get; private set;}
    public string Address { get; private set; }
    public string BusinessType { get; private set; }
    
    private BusinessProfile() { }

    private BusinessProfile(Guid userId, string businessName, string address, string businessType)
    {
        Id = Guid.NewGuid();
        UserId = userId;
        BusinessName = businessName;
        Address = address;
        BusinessType = businessType;
    }

    public static BusinessProfile Create(Guid userId, string businessName, string address, string businessType)
    {
        return new BusinessProfile(userId, businessName, address, businessType);
    }
}