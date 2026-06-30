using FoodLink.Domain.Entities;
using FoodLink.Domain.Enums;
using FoodLink.Application.Features.Reviews.Interfaces;
using FoodLink.Application.Features.Reservation.Interfaces;
using FoodLink.Application.Common.Interfaces.Repositories;
using FoodLink.Domain.Common.Exceptions;
using FoodLink.Application.Features.Reviews.DTOs;
using FoodLink.Application.Features.Notifications.Interfaces;

namespace FoodLink.Application.Features.Reviews.Services;

public class ReviewService(
    IReviewRepository reviewRepository,
    IReservationRepository reservationRepository,
    IUnitOfWork unitOfWork,
    INotificationService notificationService
) : IReviewService
{
    public async Task CreateReviewAsync(
        Guid currentUserId,
        CreateReviewRequest request,
        CancellationToken cancellationToken = default)
    {
        var reservation = await reservationRepository.GetByIdWithReviewDataAsync(request.ReservationId, cancellationToken);

        if(reservation is null)
        {
            throw new DomainException("Reservation not found.");
        }

        if (reservation.Status != ReservationStatus.PickedUp)
            throw new DomainException(
                "Review can only be submitted after pickup.");

        ReviewType reviewType;
        Guid targetUserId;

        // Charity -> Business
        if (reservation.Charity.UserId == currentUserId)
        {
            reviewType = ReviewType.CharityToBusiness;

            targetUserId =
                reservation.Donation.BusinessProfile.UserId;

            reservation.Donation.BusinessProfile
                .AddRating(request.Rating);
        }
        // Business -> Charity
        else if (
            reservation.Donation.BusinessProfile.UserId ==
            currentUserId)
        {
            reviewType = ReviewType.BusinessToCharity;

            targetUserId =
                reservation.Charity.UserId;

            reservation.Charity
                .AddRating(request.Rating);
        }
        else
        {
            throw new DomainException(
                "You are not allowed to review this reservation.");
        }

        var alreadyReviewed =
            await reviewRepository.ExistsAsync(
                reservation.Id,
                currentUserId,
                reviewType,
                cancellationToken);

        if (alreadyReviewed)
            throw new DomainException(
                "You already reviewed this reservation.");

        var review = Review.Create(
            reservation.Id,
            currentUserId,
            targetUserId,
            reviewType,
            request.Rating,
            request.Comment);

        await reviewRepository.AddAsync(
            review,
            cancellationToken);

        await unitOfWork.SaveChangesAsync(cancellationToken);

        await notificationService.CreateNotificationAsync(
            targetUserId,
            "New Review",
            $"You have received a new {request.Rating}-star review.",
            "Review",
            reservation.Id);
    }
}
