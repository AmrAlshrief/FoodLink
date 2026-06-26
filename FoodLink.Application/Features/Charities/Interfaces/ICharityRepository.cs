using FoodLink.Domain.Entities.Profiles;

namespace FoodLink.Application.Features.Charities.Interfaces;

public interface ICharityRepository
{
    Task<CharityProfile?> GetByIdAsync(Guid charityProfileId, CancellationToken ct = default);
    Task UpdateAsync(CharityProfile charity, CancellationToken ct = default);
}
