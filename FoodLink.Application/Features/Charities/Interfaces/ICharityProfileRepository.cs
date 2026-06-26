using FoodLink.Domain.Entities.Profiles;
namespace FoodLink.Application.Features.Charities.Interfaces;

public interface ICharityProfileRepository
{
    Task<List<CharityProfile>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<CharityProfile?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<CharityProfile?> GetByUserIdAsync(Guid userId, CancellationToken cancellationToken = default);
    void Add(CharityProfile profile);
}