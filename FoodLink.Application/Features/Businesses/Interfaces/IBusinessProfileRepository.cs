using FoodLink.Domain.Entities.Profiles;
namespace FoodLink.Application.Features.Businesses.Interfaces;

public interface IBusinessProfileRepository
{
    Task<BusinessProfile?> GetByUserIdAsync(Guid userId, CancellationToken cancellationToken = default);
    void Add(BusinessProfile profile);
}   