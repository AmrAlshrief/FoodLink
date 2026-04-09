using FoodLink.Domain.Common;
using FoodLink.Domain.Enums;
using FoodLink.Domain.Common.Exceptions;
namespace FoodLink.Domain.Entities;

public class ReservationItem : Entity
{
    public Guid ReservationId { get; private set; }
    public Guid DonationItemId { get; private set; }
    public int Quantity { get; private set; }

    private ReservationItem() { }

    internal ReservationItem(Guid reservationId, Guid donationItemId, int quantity) : base(Guid.NewGuid())
    {
        ReservationId = reservationId;
        DonationItemId = donationItemId;
        Quantity = quantity;
    }
}