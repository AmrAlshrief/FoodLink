using FoodLink.Domain.Entities.Profiles;
namespace FoodLink.Application.Common.Interfaces.Repositories;

public interface ICharityProfileRepository
{
    Task<CharityProfile?> GetByUserIdAsync(Guid userId, CancellationToken cancellationToken = default);
    void Add(CharityProfile profile);
}