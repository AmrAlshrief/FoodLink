using FoodLink.Application.Features.Donations.Dtos;

namespace FoodLink.Application.Common.Interfaces.Services;
public interface IDonationService
{
    Task<Guid> CreateDonationAsync(CreateDonationRequest request);
    Task<List<DonationResponse>> GetAllActiveDonationsAsync();
    Task<DonationResponse?> GetDonationByIdAsync(Guid id);
}