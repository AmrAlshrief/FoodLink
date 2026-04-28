using FoodLink.Application.Common.Interfaces.Repositories;
using FoodLink.Application.Features.Charity.Dtos;
using FoodLink.Domain.Entities.Profiles;

namespace FoodLink.Application.Features.Charity.Services;

public class CharityService(ICharityProfileRepository charityProfileRepository) : ICharityService
{
    public async Task<List<CharityResponse>> GetAllCharitiesAsync(CancellationToken cancellationToken = default)
    {
        var charities = await charityProfileRepository.GetAllAsync(cancellationToken);
        return charities.Select(MapToResponse).ToList();
    }

    public async Task<CharityResponse?> GetCharityByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var charity = await charityProfileRepository.GetByIdAsync(id, cancellationToken);
        return charity != null ? MapToResponse(charity) : null;
    }

    public async Task<CharityResponse?> GetCharityByUserIdAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        var charity = await charityProfileRepository.GetByUserIdAsync(userId, cancellationToken);
        return charity != null ? MapToResponse(charity) : null;
    }

    private static CharityResponse MapToResponse(CharityProfile charity)
    {
        return new CharityResponse
        {
            Id = charity.Id,
            UserId = charity.UserId,
            Name = charity.Name,
            LicenseNumber = charity.LicenseNumber,
            Address = charity.Address,
            NoShowCount = charity.NoShowCount,
            AverageRating = charity.AverageRating,
            RatingCount = charity.RatingCount
        };
    }
}
