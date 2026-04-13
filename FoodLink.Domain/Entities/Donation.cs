using FoodLink.Domain.Common;
using FoodLink.Domain.Enums;
using FoodLink.Domain.Common.Exceptions;
namespace FoodLink.Domain.Entities;

public class Donation : AuditableEntity
{
    public Guid BusinessProfileId { get; private set; }
    public string Title { get; private set; }
    public string Description { get; private set; }
    public DateTime ExpiryDate { get; private set; }
    public DonationStatus Status { get; private set; }
    public string ImageUrl { get; private set; } = string.Empty;

    private readonly List<DonationItem> _items = new();
    public IReadOnlyCollection<DonationItem> Items => _items.AsReadOnly();

    private Donation() { }

    public Donation(Guid businessProfileId, string title, string description, DateTime expiry, string imageUrl) : base(Guid.NewGuid())
    {
        if (expiry <= DateTime.UtcNow) throw new DomainException("Expiry must be in the future.");
        
        BusinessProfileId = businessProfileId;
        Title = title;
        Description = description;
        ExpiryDate = expiry;
        Status = DonationStatus.Available;
        ImageUrl = imageUrl;
    }

    public void ReserveItem(Guid itemId, int quantity)
    {
        EnsureAvailable();

        var item = _items.FirstOrDefault(i => i.Id == itemId)
            ?? throw new DomainException("Item not found.");

        item.ReserveQuantity(quantity);

        UpdateStatus();
    }

    private void UpdateStatus()
    {
        if (_items.All(i => i.AvailableQuantity == 0))
            Status = DonationStatus.FullyReserved;
        else if (_items.Any(i => i.ReservedQuantity > 0))
            Status = DonationStatus.PartiallyReserved;
    }

    public void AddItem(string name, int quantity, string unit)
    {
        if (Status != DonationStatus.Available) throw new DomainException("Cannot add items to a non-available donation.");
        _items.Add(new DonationItem(Id, name, quantity, ImageUrl, unit));
    }

    public void Cancel()
    {
        EnsureAvailable();
        Status = DonationStatus.Cancelled;
    }

    public void Expire()
    {
        if (ExpiryDate <= DateTime.UtcNow)
            Status = DonationStatus.Expired;
    }

    public void Complete()
    {
        if (Status != DonationStatus.FullyReserved)
            throw new DomainException("Cannot complete unless fully reserved.");

        Status = DonationStatus.Completed;
    }

    private void EnsureAvailable()
    {
        if (Status != DonationStatus.Available &&
            Status != DonationStatus.PartiallyReserved)
            throw new DomainException("Donation is not available.");
    }

    private void EnsureNotExpired()
    {
        if (ExpiryDate <= DateTime.UtcNow)
            throw new DomainException("Donation is expired.");
    }

    


}