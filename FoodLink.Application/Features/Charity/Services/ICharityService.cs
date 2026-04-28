using FoodLink.Application.Features.Charity.Dtos;

namespace FoodLink.Application.Features.Charity.Services;

public interface ICharityService
{
    Task<List<CharityResponse>> GetAllCharitiesAsync(CancellationToken cancellationToken = default);
    Task<CharityResponse?> GetCharityByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<CharityResponse?> GetCharityByUserIdAsync(Guid userId, CancellationToken cancellationToken = default);
}
