using FoodLink.Application.Features.Donations.Dtos;

namespace FoodLink.Application.Common.Interfaces.Services;
public interface IDonationService
{
    Task<Guid> CreateDonationAsync(CreateDonationRequest request);
    Task AddItemAsync(Guid donationId, AddDonationItemRequest request);
    Task UpdateDonationAsync(UpdateDonationRequest request);
    Task UpdateItemAsync(Guid donationId, Guid itemId, UpdateDonationItemRequest request);
    Task RemoveItemAsync(Guid donationId, Guid itemId);
    Task<List<DonationResponse>> GetAllActiveDonationsAsync();
    Task<DonationResponse?> GetDonationByIdAsync(Guid id);
}