using FoodLink.Application.Features.Donations.Dtos;

namespace FoodLink.Application.Common.Interfaces.Services;
public interface IDonationService
{
    Task<Guid> CreateDonationAsync(CreateDonationRequest request);
    Task AddItemAsync(Guid donationId, AddDonationItemRequest request);
    Task UpdateDonationAsync(UpdateDonationRequest request);
    Task UpdateItemAsync(Guid donationId, Guid itemId, UpdateDonationItemRequest request);
    Task RemoveItemAsync(Guid donationId, Guid itemId);
    Task RemoveDonationAsync(Guid id);
    Task<List<DonationResponse>> GetAllActiveDonationsAsync();
    Task<DonationResponse?> GetDonationByIdAsync(Guid id);
    Task<List<DonationResponse>> GetDonationsByBusinessIdAsync(Guid businessId, DonationFilterRequest filter, CancellationToken cancellationToken = default);
    Task CancelDonationAsync(Guid donationId, CancellationToken cancellationToken = default);
}