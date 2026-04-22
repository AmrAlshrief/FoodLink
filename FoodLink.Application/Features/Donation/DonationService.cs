using FoodLink.Domain.Entities;
using FoodLink.Application.Common.Interfaces;
using FoodLink.Application.Common.Interfaces.Services;
using FoodLink.Application.Common.Interfaces.Repositories;
using FoodLink.Application.Features.Donations.Dtos;
using FoodLink.Domain.Common.Exceptions;

namespace FoodLink.Application.Features.Donations;
public class DonationService(
    IDonationRepository donationRepository, 
    IUnitOfWork unitOfWork,
    IImageService imageService,
    IUserContext userContext) : IDonationService
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

        var donation = new Donation(
            businessId,
            request.Title,
            request.Description ?? string.Empty,
            request.ExpiryDate.Value.DateTime,
            imageUrl);

        donationRepository.Add(donation);
        await unitOfWork.SaveChangesAsync();

        return donation.Id;
    }

    public async Task<List<DonationResponse>> GetAllActiveDonationsAsync()
    {
        var donations = await donationRepository.GetActiveDonationsAsync();
        
        // Simple manual mapping (No AutoMapper)
        return donations.Select(MapToResponse).ToList();
    }

    public async Task<DonationResponse?> GetDonationByIdAsync(Guid id)
    {
        var donation = await donationRepository.GetByIdAsync(id);
        return donation is null ? null : MapToResponse(donation);
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

        // Fetch entity after I/O to keep DbContext fresh
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

    private DonationResponse MapToResponse(Donation donation)
    {
        return new DonationResponse
        {
            Id = donation.Id,
            Title = donation.Title,
            Description = donation.Description,
            ExpiryDate = donation.ExpiryDate,
            ImageUrl = donation.ImageUrl,
            Items = donation.Items.Select(i => new DonationItemResponse 
            { 
                Id = i.Id,  
                Name = i.Name, 
                Quantity = i.AvailableQuantity, 
                Unit = i.Unit ,
                ImageUrl = i.ImageUrl
            }).ToList()
        };
    }
}