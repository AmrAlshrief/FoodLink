using Donation = FoodLink.Domain.Entities.Donation;
using FoodLink.Application.Common.Interfaces;
using FoodLink.Application.Features.Donation.Interfaces;
using FoodLink.Application.Common.Interfaces.Repositories;
using FoodLink.Application.Features.Donations.Dtos;
using FoodLink.Domain.Common.Exceptions;
using FoodLink.Domain.Enums;

using FoodLink.Application.Features.Reviews.Interfaces;
using FoodLink.Application.Features.Reservation.Interfaces;
using Microsoft.Extensions.Logging;

namespace FoodLink.Application.Features.Donations;
public class DonationService(
    IDonationRepository donationRepository, 
    IUnitOfWork unitOfWork,
    IImageService imageService,
    IUserContext userContext,
    IReviewQueries reviewQueries,
    IReservationQueries reservationQueries,
    ILogger<DonationService> logger) : IDonationService
{
    public async Task<Guid> CreateDonationAsync(CreateDonationRequest request)
    {
        var businessId = userContext.BusinessProfileId
            ?? throw new DomainException("User does not have a business profile.");

        if (!request.ExpiryDate.HasValue)
            throw new DomainException("Expiry date is required.");

        var imageUrl = string.Empty;
        if (request.Image != null)
        {
            imageUrl = await imageService.UploadImageAsync(
                request.Image,
                request.ImageFileName ?? string.Empty);
        }

        var donation = new FoodLink.Domain.Entities.Donation(
            businessId,
            request.Title,
            request.Description ?? string.Empty,
            request.ExpiryDate.Value.DateTime,
            imageUrl);

        donationRepository.Add(donation);
        await unitOfWork.SaveChangesAsync();

        logger.LogInformation("Donation {DonationId} created by Business {BusinessId}", donation.Id, businessId);

        return donation.Id;
    }

    public async Task<List<DonationResponse>> GetAllActiveDonationsAsync()
    {
        var donations = await donationRepository.GetActiveDonationsAsync();
        
        var responses = new List<DonationResponse>();
        foreach (var donation in donations)
        {
            responses.Add(await MapToResponseAsync(donation));
        }
        return responses;
    }

    public async Task<DonationResponse?> GetDonationByIdAsync(Guid id)
    {
        var donation = await donationRepository.GetByIdAsync(id);
        return donation is null ? null : await MapToResponseAsync(donation);
    }

    public async Task UpdateDonationAsync(UpdateDonationRequest request)
    {
        var businessId = userContext.BusinessProfileId
            ?? throw new DomainException("User does not have a business profile.");

        // Upload image BEFORE fetching entity to minimize time in DbContext
        var imageUrl = string.Empty;
        if (request.Image != null)
        {
            imageUrl = await imageService.UploadImageAsync(
                request.Image,
                request.ImageFileName ?? string.Empty);
        }

        var donation = await donationRepository.GetByIdAsync(request.Id)
            ?? throw new DomainException("Donation not found.");

        if (donation.BusinessProfileId != businessId)
            throw new DomainException("You are not allowed to update this donation.");

        donation.UpdateDetails(
            request.Title,
            request.Description,
            request.ExpiryDate?.DateTime);

        if (!string.IsNullOrEmpty(imageUrl))
        {
            donation.SetImage(imageUrl);
        }

        await unitOfWork.SaveChangesAsync();
     
    }

    public async Task AddItemAsync(Guid donationId, AddDonationItemRequest request)
    {
        var donation = await donationRepository.GetByIdAsync(donationId);

        if (donation is null)
            throw new DomainException("Donation not found.");

        var itemImageUrl = string.Empty;

        if (request.Image != null)
        {
            itemImageUrl = await imageService.UploadImageAsync(
                request.Image,
                request.ImageFileName ?? string.Empty);
        }

        donation.AddItem(
            request.Name,
            request.Quantity,
            request.Unit,
            itemImageUrl);

        await unitOfWork.SaveChangesAsync();
    }

    public async Task UpdateItemAsync(Guid donationId, Guid itemId, UpdateDonationItemRequest request)
    {
        var businessId = userContext.BusinessProfileId
            ?? throw new DomainException("User does not have a business profile.");

        var donation = await donationRepository.GetByIdAsync(donationId)
            ?? throw new DomainException("Donation not found.");

        if (donation.BusinessProfileId != businessId)
            throw new DomainException("You are not allowed to modify this donation.");

        donation.UpdateItem(
            itemId,
            request.Name,
            request.Quantity,
            request.Unit);

        if (request.Image != null)
        {
            var imageUrl = await imageService.UploadImageAsync(
                request.Image,
                request.ImageFileName ?? string.Empty);

            donation.SetItemImage(itemId, imageUrl);
        }

        await unitOfWork.SaveChangesAsync();
    }

    public async Task RemoveItemAsync(Guid donationId, Guid itemId)
    {
        var businessId = userContext.BusinessProfileId
            ?? throw new DomainException("User does not have a business profile.");

        var donation = await donationRepository.GetByIdAsync(donationId)
            ?? throw new DomainException("Donation not found.");

        if (donation.BusinessProfileId != businessId)
            throw new DomainException("You are not allowed to modify this donation.");

        donation.RemoveItem(itemId);

        await unitOfWork.SaveChangesAsync();
    }

    public async Task RemoveDonationAsync(Guid id)
    {
        var businessId = userContext.BusinessProfileId
            ?? throw new DomainException("User does not have a business profile.");

        var donation = await donationRepository.GetByIdAsync(id)
            ?? throw new DomainException("Donation not found.");

        if (donation.BusinessProfileId != businessId)
            throw new DomainException("You are not allowed to modify this donation.");

        donationRepository.Remove(donation);

        await unitOfWork.SaveChangesAsync();
    }

    public async Task<List<DonationResponse>> GetDonationsByBusinessIdAsync(Guid businessId, DonationFilterRequest filter, CancellationToken cancellationToken = default)
    {
        if (userContext.BusinessProfileId != businessId || businessId == Guid.Empty)
            throw new DomainException("You are not allowed to view these donations.");
            
        var donations = await donationRepository.GetByBusinessIdAsync(businessId);

        if (!string.IsNullOrWhiteSpace(filter.Status) &&
            Enum.TryParse<DonationStatus>(filter.Status, true, out var status))
        {
            donations = donations
                .Where(d => d.Status == status)
                .ToList();
        }

        var responses = new List<DonationResponse>();
        foreach (var donation in donations)
        {
            responses.Add(await MapToResponseAsync(donation));
        }
        return responses;
    }

    public async Task CancelDonationAsync(Guid donationId, CancellationToken cancellationToken = default)
    {
        var businessId = userContext.BusinessProfileId
            ?? throw new DomainException("User does not have a business profile.");

        var donation = await donationRepository.GetByIdAsync(donationId, cancellationToken)
            ?? throw new DomainException("Donation not found.");

        if (donation.BusinessProfileId != businessId)
            throw new DomainException("You are not allowed to cancel this donation.");

        donation.Cancel();

        await unitOfWork.SaveChangesAsync(cancellationToken);
    }

    public async Task HandleExpiredDonationsAsync(CancellationToken cancellationToken = default)
    {
        var expiredDonations = await donationRepository
            .GetExpiredActiveDonationsAsync(DateTime.UtcNow, cancellationToken);

        foreach (var donation in expiredDonations)
        {
            donation.Expire();
        }

        await unitOfWork.SaveChangesAsync(cancellationToken);
    }

    private async Task<DonationResponse> MapToResponseAsync(FoodLink.Domain.Entities.Donation donation)
    {
        bool hasHistory = false;
        var charityId = userContext.CharityProfileId;
        if (charityId.HasValue && donation.BusinessProfileId != Guid.Empty)
        {
            hasHistory = await reservationQueries.HasPastReservationAsync(charityId.Value, donation.BusinessProfileId);
        }

        var ratingSummary = await reviewQueries.GetBusinessRatingSummaryAsync(donation.BusinessProfileId);
        var businessSummary = new BusinessSummaryDto
        {
            Id = donation.BusinessProfileId,
            Name = donation.BusinessProfile?.BusinessName ?? string.Empty,
            AggregateRating = ratingSummary?.AverageRating ?? 0.0,
            ReviewCount = ratingSummary?.TotalRatings ?? 0,
            IsVerified = true, // Default to true as per requirements, could be mapped from profile later
            LogoUrl = donation.BusinessProfile?.User?.ProfileImage
        };

        return new DonationResponse
        {
            Id = donation.Id,
            Title = donation.Title,
            Description = donation.Description,
            ExpiryDate = donation.ExpiryDate,
            ImageUrl = donation.ImageUrl,
            Status = donation.Status.ToString(),
            HasPreviousHistory = hasHistory,
            Business = businessSummary,
            Items = donation.Items.Select(i => new DonationItemResponse 
            { 
                Id = i.Id,  
                Name = i.Name, 
                Quantity = i.AvailableQuantity, 
                TotalQuantity = i.TotalQuantity,
                ReservedQuantity = i.ReservedQuantity,
                Unit = i.Unit,
                ImageUrl = i.ImageUrl
            }).ToList()
        };
    }
}