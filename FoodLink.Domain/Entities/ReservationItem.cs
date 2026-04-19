using FoodLink.Domain.Common;
using FoodLink.Domain.Enums;
using FoodLink.Domain.Common.Exceptions;
namespace FoodLink.Domain.Entities;

public class ReservationItem : Entity
{
    public Guid ReservationId { get; private set; }
    public Guid DonationItemId { get; private set; }
    public string ItemName { get; private set; }
    public string Unit { get; private set; }
    public int Quantity { get; private set; }

    private ReservationItem() { }

    internal ReservationItem(
        Guid reservationId,
        Guid donationItemId,
        string itemName,
        string unit,
        int quantity)
    {
        if (quantity <= 0)
            throw new DomainException("Quantity must be positive.");

        if (string.IsNullOrWhiteSpace(itemName))
            throw new DomainException("Item name is required.");

        if (string.IsNullOrWhiteSpace(unit))
            throw new DomainException("Unit is required.");

        ReservationId = reservationId;
        DonationItemId = donationItemId;
        ItemName = itemName;
        Unit = unit;
        Quantity = quantity;
    }
}