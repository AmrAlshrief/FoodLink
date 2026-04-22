using FoodLink.Domain.Common;
using FoodLink.Domain.Enums;
using FoodLink.Domain.Common.Exceptions;
namespace FoodLink.Domain.Entities;

public class Donation : AuditableEntity
{
    public Guid BusinessProfileId { get; private set; }
    public string Title { get; private set; } = string.Empty;
    public string Description { get; private set; } = string.Empty;
    public DateTime ExpiryDate { get; private set; }
    public DonationStatus Status { get; private set; }
    public string ImageUrl { get; private set; } = string.Empty;

    private readonly List<DonationItem> _items = new();
    public IReadOnlyCollection<DonationItem> Items => _items.AsReadOnly();

    private Donation() { }

    public Donation(Guid businessProfileId,
                    string title,
                    string description,
                    DateTime expiry,
                    string imageUrl) : base(Guid.NewGuid())
    {
        if (expiry <= DateTime.UtcNow) throw new DomainException("Expiry must be in the future.");
        
        if (businessProfileId == Guid.Empty)
            throw new DomainException("Business profile is required.");

        if (string.IsNullOrWhiteSpace(title))
            throw new DomainException("Title is required.");

        if (expiry <= DateTime.UtcNow)
            throw new DomainException("Expiry must be in the future.");

        BusinessProfileId = businessProfileId;
        Title = title;
        Description = description;
        ExpiryDate = expiry;
        Status = DonationStatus.Available;
        ImageUrl = imageUrl ?? string.Empty;
    }

   // ----------------------------
    // Donation Updates
    // ----------------------------

    public void UpdateDetails(string? title, string? description, DateTime? expiryDate)
    {
        EnsureEditable();

        if (!string.IsNullOrWhiteSpace(title))
            Title = title;

        if (description is not null)
            Description = description;

        if (expiryDate.HasValue)
        {
            if (expiryDate.Value <= DateTime.UtcNow)
                throw new DomainException("Expiry must be in the future.");

            ExpiryDate = expiryDate.Value;
        }
    }

    public void SetImage(string imageUrl)
    {
        EnsureEditable();
        ImageUrl = imageUrl ?? string.Empty;
    }

    // ----------------------------
    // Items Management
    // ----------------------------

    public void AddItem(string name, int quantity, string unit, string imageUrl = "")
    {
        EnsureEditable();

        _items.Add(new DonationItem(Id, name, quantity, unit, imageUrl));
        UpdateStatus();
    }

    public void UpdateItem(Guid itemId, string name, int quantity, string unit)
    {
        EnsureEditable();

        var item = GetItem(itemId);
        item.UpdateDetails(name, quantity, unit);

        UpdateStatus();
    }

    public void SetItemImage(Guid itemId, string imageUrl)
    {
        EnsureEditable();

        var item = GetItem(itemId);
        item.SetImage(imageUrl);
    }

    public void RemoveItem(Guid itemId)
    {
        EnsureEditable();

        var item = GetItem(itemId);

        if (item.ReservedQuantity > 0)
            throw new DomainException("Cannot remove item with reservations.");

        _items.Remove(item);
        UpdateStatus();
    }

    // ----------------------------
    // Reservation Logic
    // ----------------------------

    public void ReserveItem(Guid itemId, int quantity)
    {
        EnsureNotExpired();
        EnsureAvailable();

        var item = GetItem(itemId);
        item.ReserveQuantity(quantity);

        UpdateStatus();
    }

    public void ReleaseItem(Guid itemId, int quantity)
    {
        var item = GetItem(itemId);
        item.ReleaseQuantity(quantity);

        UpdateStatus();
    }

    // ----------------------------
    // Lifecycle
    // ----------------------------

    public void Cancel()
    {
        EnsureEditable();
        Status = DonationStatus.Cancelled;
    }

    public void Expire()
    {
        if (ExpiryDate > DateTime.UtcNow)
            throw new DomainException("Cannot expire before expiry date.");

        Status = DonationStatus.Expired;
    }

    public void Complete()
    {
        if (Status != DonationStatus.FullyReserved)
            throw new DomainException("Cannot complete unless fully reserved.");

        Status = DonationStatus.Completed;
    }

    // ----------------------------
    // Helpers
    // ----------------------------

    public bool IsExpired()
    {
        return ExpiryDate <= DateTime.UtcNow;
    }

    public DonationItem GetItem(Guid itemId)
    {
        return _items.FirstOrDefault(i => i.Id == itemId)
            ?? throw new DomainException("Item not found.");
    }

    private void EnsureEditable()
    {
        if (Status != DonationStatus.Available)
            throw new DomainException("Donation is not editable.");
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

    private void UpdateStatus()
    {
        if (_items.Count == 0)
        {
            Status = DonationStatus.Available;
            return;
        }

        if (_items.All(i => i.AvailableQuantity == i.TotalQuantity))
            Status = DonationStatus.Available;

        else if (_items.All(i => i.AvailableQuantity == 0))
            Status = DonationStatus.FullyReserved;

        else if (_items.Any(i => i.ReservedQuantity > 0))
            Status = DonationStatus.PartiallyReserved;
    }

    


}