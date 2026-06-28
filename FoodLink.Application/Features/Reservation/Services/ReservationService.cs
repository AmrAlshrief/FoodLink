using FoodLink.Application.Common.Interfaces;
using FoodLink.Application.Common.Interfaces.Repositories;
using FoodLink.Application.Features.Reservation.Interfaces;
using FoodLink.Application.Features.Reservation.Dtos;
using FoodLink.Application.Features.Donation.Interfaces;
using FoodLink.Application.Features.Charities.Interfaces;
using FoodLink.Application.Features.Notifications.Interfaces;
using FoodLink.Domain.Common.Exceptions;
using Reservation = FoodLink.Domain.Entities.Reservation;
using Donation = FoodLink.Domain.Entities.Donation;

namespace FoodLink.Application.Features.Reservation.Services;

public class ReservationService(
    IReservationRepository reservationRepository,
    IDonationRepository donationRepository,
    ICharityProfileRepository charityProfileRepository,
    IUserContext userContext,
    IUnitOfWork unitOfWork,
    INotificationService notificationService) : IReservationService
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

        var reservation = new FoodLink.Domain.Entities.Reservation(charityId, donation.Id, expiresAt);

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

        if (donation.BusinessProfile != null)
        {
            await notificationService.CreateNotificationAsync(
                donation.BusinessProfile.UserId,
                "New Reservation",
                $"A new reservation has been placed on your donation '{donation.Title}'.",
                "Reservation",
                reservation.Id);
        }

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

        if (donation.BusinessProfile != null)
        {
            await notificationService.CreateNotificationAsync(
                donation.BusinessProfile.UserId,
                "Reservation Cancelled",
                $"A reservation for your donation '{donation.Title}' has been cancelled by the charity.",
                "Reservation",
                reservation.Id);
        }
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

        var charityProfile = await charityProfileRepository.GetByIdAsync(reservation.CharityId, cancellationToken);
        if (charityProfile != null)
        {
            await notificationService.CreateNotificationAsync(
                charityProfile.UserId,
                "Reservation Picked Up",
                $"Your reservation for donation '{donation.Title}' has been marked as picked up.",
                "Reservation",
                reservation.Id);
        }
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

    private static ReservationResponse MapToResponse(FoodLink.Domain.Entities.Reservation reservation)
    {
        return new ReservationResponse
        {
            Id = reservation.Id,
            //DonationId = reservation.DonationId,
            //CharityId = reservation.CharityId,
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