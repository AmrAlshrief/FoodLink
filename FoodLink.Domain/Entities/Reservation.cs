using FoodLink.Domain.Common;
using FoodLink.Domain.Enums;
using FoodLink.Domain.Common.Exceptions;

namespace FoodLink.Domain.Entities;

public class Reservation : AuditableEntity
{
    public Guid CharityId { get; private set; }
    public Guid DonationId { get; private set; }
    
    public ReservationStatus Status { get; private set; }
    public DateTime? PickedUpAt { get; private set; }
    public DateTime ExpiresAt { get; private set; }

    private readonly List<ReservationItem> _items = new();
    public IReadOnlyCollection<ReservationItem> Items => _items.AsReadOnly();

    private Reservation() { }

    public Reservation(Guid charityId, Guid donationId, DateTime expiresAt) : base(Guid.NewGuid())
    {
        CharityId = charityId;
        DonationId = donationId;
        ExpiresAt = expiresAt;
        Status = ReservationStatus.Pending;
    }

    public void AddItem(Guid donationItemId, string itemName, string unit, int quantity)
    {
        if (quantity <= 0)
        throw new DomainException("Quantity must be positive.");

         if (_items.Any(i => i.DonationItemId == donationItemId))
            throw new DomainException("Item already added.");
        
        _items.Add(new ReservationItem(Id, donationItemId, itemName, unit, quantity));
    }

    public void Cancel()
    {
        if (Status == ReservationStatus.PickedUp)
        throw new DomainException("Cannot cancel picked up reservation.");

        if (Status == ReservationStatus.Cancelled)
            throw new DomainException("Reservation already cancelled.");

        if (Status == ReservationStatus.NoShow)
            throw new DomainException("Cannot cancel no-show reservation.");


        Status = ReservationStatus.Cancelled;
    }

    public void CompletePickup()
    {
        if (DateTime.UtcNow > ExpiresAt)
            throw new DomainException("Cannot pick up expired reservation.");
            
        if (Status != ReservationStatus.Pending)
            throw new DomainException("Only active reservations can be picked up.");

        Status = ReservationStatus.PickedUp;
        PickedUpAt = DateTime.UtcNow;
    }

    public void MarkNoShow()
    {
        if (DateTime.UtcNow <= ExpiresAt)
            throw new DomainException("Cannot mark reservation as no-show before it expires.");

        if (Status != ReservationStatus.Pending)
            throw new DomainException("Only active reservations can be marked as no-show.");

        Status = ReservationStatus.NoShow;
    }

    public void EnsureHasItems()
    {
        if (!_items.Any())
            throw new DomainException("Reservation must contain at least one item.");
    }
}