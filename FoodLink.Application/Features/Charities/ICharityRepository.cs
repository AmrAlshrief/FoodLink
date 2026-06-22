using FoodLink.Domain.Entities.Profiles;

namespace FoodLink.Application.Features.Charities;

public interface ICharityRepository
{
    Task<CharityProfile?> GetByIdAsync(Guid charityProfileId, CancellationToken ct = default);
    Task UpdateAsync(CharityProfile charity, CancellationToken ct = default);
}
