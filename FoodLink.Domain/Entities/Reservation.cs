using FoodLink.Domain.Common;
using FoodLink.Domain.Enums;
using FoodLink.Domain.Common.Exceptions;

namespace FoodLink.Domain.Entities;

public class Reservation : AuditableEntity
{
    public Guid CharityId { get; private set; }
    public ReservationStatus Status { get; private set; }
    public DateTime? PickedUpAt { get; private set; }

    private readonly List<ReservationItem> _items = new();
    public IReadOnlyCollection<ReservationItem> Items => _items.AsReadOnly();

    private Reservation() { }

    public Reservation(Guid charityId) : base(Guid.NewGuid())
    {
        CharityId = charityId;
        Status = ReservationStatus.Pending;
    }

    public void AddItem(DonationItem donationItem, int quantity)
    {
        if (quantity <= 0)
        throw new DomainException("Quantity must be positive.");
        
        _items.Add(new ReservationItem(Id, donationItem.Id, quantity));
    }

    public void Confirm()
    {
        if (Status != ReservationStatus.Pending)
            throw new DomainException("Only pending reservations can be confirmed.");

        Status = ReservationStatus.Confirmed;
    }

    public void Cancel()
    {
        if (Status == ReservationStatus.Completed)
            throw new DomainException("Cannot cancel completed reservation.");

        Status = ReservationStatus.Cancelled;
    }

    public void CompletePickup()
    {
        if (Status != ReservationStatus.Confirmed)
            throw new DomainException("Must confirm before completing.");

        Status = ReservationStatus.Completed;
        PickedUpAt = DateTime.UtcNow;
    }
}