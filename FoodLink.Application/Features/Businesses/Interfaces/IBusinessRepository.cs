using FoodLink.Domain.Entities.Profiles;

namespace FoodLink.Application.Features.Businesses.Interfaces;

public interface IBusinessRepository
{
    Task<BusinessProfile?> GetByIdAsync(Guid businessProfileId, CancellationToken ct = default);
    Task UpdateAsync(BusinessProfile business, CancellationToken ct = default);
}
