namespace FoodLink.Application.Features.Donation.Interfaces;

public interface IDonationRepository
{
    Task AddAsync(FoodLink.Domain.Entities.Donation donation);
    Task<FoodLink.Domain.Entities.Donation?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<List<FoodLink.Domain.Entities.Donation>> GetActiveDonationsAsync(CancellationToken cancellationToken = default);
    Task<List<FoodLink.Domain.Entities.Donation>> GetByBusinessIdAsync(Guid businessId, CancellationToken cancellationToken = default);
    Task<List<FoodLink.Domain.Entities.Donation>> GetExpiredActiveDonationsAsync(DateTime utcNow, CancellationToken cancellationToken = default);
    void Add(FoodLink.Domain.Entities.Donation donation);
    void Update(FoodLink.Domain.Entities.Donation donation);
    void Remove(FoodLink.Domain.Entities.Donation donation);
    Task<(List<FoodLink.Domain.Entities.Donation> Items, int TotalCount)> GetDonationsByBusinessIdPagedAsync(
        Guid businessId,
        FoodLink.Domain.Enums.DonationScope scope,
        string? search,
        FoodLink.Domain.Enums.DonationStatus? status,
        string? sortBy,
        string? sortDirection,
        int page,
        int pageSize,
        CancellationToken cancellationToken = default);
}
