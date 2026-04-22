using FoodLink.Application.Common.Interfaces;
using FoodLink.Application.Common.Interfaces.Repositories;
using FoodLink.Application.Common.Interfaces.Services;
using FoodLink.Application.Features.Reservation.Dtos;
using FoodLink.Domain.Common.Exceptions;
using FoodLink.Domain.Entities;

public class ReservationService(
    IReservationRepository reservationRepository,
    IDonationRepository donationRepository,
    ICharityProfileRepository charityProfileRepository,
    IUserContext userContext,
    IUnitOfWork unitOfWork) : IReservationService
{
    public async Task<Guid> CreateReservationAsync(CreateReservationRequest request, CancellationToken cancellationToken = default)
    {
        var charityId = userContext.CharityProfileId
            ?? throw new DomainException("User does not have a charity profile.");

        var donation = await donationRepository.GetByIdAsync(request.DonationId)
            ?? throw new DomainException("Donation not found.");

        if (donation.IsExpired())
            throw new DomainException("Donation is expired.");

        var expiresAt = DateTime.UtcNow.AddHours(24); 

        if (expiresAt > donation.ExpiryDate)
        {
            expiresAt = donation.ExpiryDate;
        }

        var reservation = new Reservation(charityId, donation.Id, expiresAt);

        foreach (var itemDto in request.Items)
        {
            var item = donation.GetItem(itemDto.DonationItemId);

            donation.ReserveItem(item.Id, itemDto.Quantity);

            reservation.AddItem(
                item.Id,
                item.Name,
                item.Unit,
                itemDto.Quantity);
        }

        reservation.EnsureHasItems();
        reservationRepository.Add(reservation);
        await unitOfWork.SaveChangesAsync();

        return reservation.Id;
    }

    public async Task CancelReservationAsync(Guid reservationId, CancellationToken cancellationToken = default)
    {
        var charityId = userContext.CharityProfileId
            ?? throw new DomainException("User does not have a charity profile.");

        var reservation = await reservationRepository.GetByIdAsync(reservationId, cancellationToken)
            ?? throw new DomainException("Reservation not found.");

        if (reservation.CharityId != charityId)
            throw new DomainException("You are not allowed to cancel this reservation.");

        var donation = await donationRepository.GetByIdAsync(reservation.DonationId, cancellationToken)
            ?? throw new DomainException("Donation not found.");

        foreach (var item in reservation.Items)
        {
            donation.ReleaseItem(item.DonationItemId, item.Quantity);
        }

        reservation.Cancel();

        await unitOfWork.SaveChangesAsync(cancellationToken);
    }

    public async Task MarkPickedUpAsync(Guid reservationId, CancellationToken cancellationToken = default)
    {
        var businessId = userContext.BusinessProfileId
            ?? throw new DomainException("User does not have a business profile.");

        var reservation = await reservationRepository.GetByIdAsync(reservationId, cancellationToken)
            ?? throw new DomainException("Reservation not found.");

        var donation = await donationRepository.GetByIdAsync(reservation.DonationId, cancellationToken)
            ?? throw new DomainException("Donation not found.");

        if (donation.BusinessProfileId != businessId)
            throw new DomainException("You are not allowed to mark this reservation as picked up.");

        reservation.CompletePickup();

        await unitOfWork.SaveChangesAsync(cancellationToken);
    }

    public async Task HandleExpiredReservationsAsync(CancellationToken cancellationToken = default)
    {
        var expiredReservations = await reservationRepository.GetExpiredPendingAsync(DateTime.UtcNow, cancellationToken);

        foreach (var reservation in expiredReservations)
        {
            var donation = await donationRepository.GetByIdAsync(reservation.DonationId, cancellationToken);
            if (donation is null)
                continue;

            foreach (var item in reservation.Items)
            {
                donation.ReleaseItem(item.DonationItemId, item.Quantity);
            }

            reservation.MarkNoShow();

            var charityProfile = await charityProfileRepository.GetByIdAsync(reservation.CharityId, cancellationToken);
            if (charityProfile is not null)
            {
                charityProfile.MarkNoShow();
            }
        }

        await unitOfWork.SaveChangesAsync(cancellationToken);
    }

    public async Task<List<ReservationResponse>> GetMyReservationsAsync(CancellationToken cancellationToken = default)
    {
        var charityId = userContext.CharityProfileId
            ?? throw new DomainException("User does not have a charity profile.");

        var reservations = await reservationRepository.GetByCharityIdAsync(charityId, cancellationToken);
        
        return reservations.Select(MapToResponse).ToList();
    }

    private static ReservationResponse MapToResponse(Reservation reservation)
    {
        return new ReservationResponse
        {
            Id = reservation.Id,
            DonationId = reservation.DonationId,
            CharityId = reservation.CharityId,
            Status = reservation.Status.ToString(),
            ExpiresAt = reservation.ExpiresAt,
            PickedUpAt = reservation.PickedUpAt,
            Items = reservation.Items.Select(i => new ReservationItemResponse
            {
                DonationItemId = i.DonationItemId,
                ItemName = i.ItemName,
                Unit = i.Unit,
                Quantity = i.Quantity
            }).ToList()
        };
    }


}