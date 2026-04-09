using FoodLink.Domain.Entities;
using FoodLink.Application.Common.Interfaces;
using FoodLink.Application.Common.Interfaces.Services;
using FoodLink.Application.Common.Interfaces.Repositories;
using FoodLink.Application.Features.Donations.Dtos;
namespace FoodLink.Application.Features.Donations;
public class DonationService(
    IDonationRepository donationRepository, 
    IUnitOfWork unitOfWork,
    IImageService imageService) : IDonationService
{
    public async Task<Guid> CreateDonationAsync(CreateDonationRequest request)
    {
        // 1. Upload the image first, if provided
        var imageUrl = string.Empty;
        if (request.Image != null)
        {
            imageUrl = await imageService.UploadImageAsync(
                request.Image,
                request.ImageFileName ?? string.Empty);
        }

        // 2. Create the Aggregate Root
        var donation = new Donation(
            request.BusinessId,
            request.Title,
            request.Description ?? string.Empty,
            request.ExpiryDate?.DateTime ?? DateTime.UtcNow.AddDays(1),
            imageUrl);

        // 3. Add children through the Domain logic
        foreach (var item in request.Items)
        {
            donation.AddItem(item.Name, item.Quantity, item.Unit);
        }

        // 3. Persist via Repository and UoW
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
        var donations = await donationRepository.GetActiveDonationsAsync();
        var donation = donations.FirstOrDefault(d => d.Id == id);
        return donation != null ? MapToResponse(donation) : null;
    }

    private DonationResponse MapToResponse(Donation donation)
    {
        return new DonationResponse
        {
            Id = donation.Id,
            Title = donation.Title,
            Description = donation.Description,
            ExpiryDate = donation.ExpiryDate,
            Items = donation.Items.Select(i => new DonationItemResponse 
            { 
                Name = i.Name, 
                Quantity = i.AvailableQuantity, 
                Unit = i.Unit 
            }).ToList()
        };
    }
}