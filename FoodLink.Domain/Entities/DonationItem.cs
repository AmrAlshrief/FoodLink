using FoodLink.Domain.Common;
using FoodLink.Domain.Enums;
using FoodLink.Domain.Common.Exceptions;

namespace FoodLink.Domain.Entities;

public class DonationItem : Entity
{
    public Guid DonationId { get; private set; }
    public string Name { get; private set; }
    public int TotalQuantity { get; private set; }
    public int ReservedQuantity { get; private set; }
    public string Unit { get; private set; }
    public string ImageUrl { get; private set; } = string.Empty;
    public int AvailableQuantity => TotalQuantity - ReservedQuantity;

    private DonationItem() { }

    internal DonationItem(Guid donationId, string name, int totalQuantity, string imageUrl, string unit) : base(Guid.NewGuid())
    {
        if (string.IsNullOrWhiteSpace(name))
    throw new DomainException("Name is required");

    if (totalQuantity <= 0)
        throw new DomainException("Quantity must be > 0");

    if (string.IsNullOrWhiteSpace(unit))
        throw new DomainException("Unit is required");

        DonationId = donationId;
        Name = name;
        TotalQuantity = totalQuantity;
        Unit = unit;
        ImageUrl = imageUrl;
        ReservedQuantity = 0;
    }

    public void ReserveQuantity(int amount)
    {
        if (amount <= 0) throw new DomainException("Reservation amount must be positive.");
        if (amount > AvailableQuantity) throw new DomainException($"Insufficient quantity for {Name}.");
        
        ReservedQuantity += amount;
    }

    public void SetImage(string imageUrl)
    {
        // if (string.IsNullOrWhiteSpace(imageUrl))
        //     throw new DomainException("Image URL is required.");
        ImageUrl = imageUrl;
    }

    public void ReleaseQuantity(int amount)
    {
        if (amount <= 0)
            throw new DomainException("Release amount must be positive.");

        if (amount > ReservedQuantity)
            throw new DomainException("Cannot release more than reserved.");

        ReservedQuantity -= amount;
    }
}