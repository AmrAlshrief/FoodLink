using FoodLink.Domain.Entities;
namespace FoodLink.Application.Common.Interfaces.Repositories;

public interface IDonationRepository
{
    Task AddAsync(Donation donation);
    Task<Donation?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<List<Donation>> GetActiveDonationsAsync(CancellationToken cancellationToken = default);
    public void Add(Donation donation);
    public void Update(Donation donation);
    public void Remove(Donation donation);
}

