namespace FoodLink.Domain.Entities.Profiles;

public class CharityProfile : Profile
{
    public string Name { get; private set; }
    public string LicenseNumber { get; private set; }
    public string Address { get; private set; }

    private CharityProfile() { }

    internal CharityProfile(Guid userId, string name, string license, string address) 
        : base(userId)
    {
        Name = name;
        LicenseNumber = license;
        Address = address;
    }

    public static CharityProfile Create(Guid userId, string name, string license, string address)
    {
        return new CharityProfile(userId, name, license, address);
    }
}