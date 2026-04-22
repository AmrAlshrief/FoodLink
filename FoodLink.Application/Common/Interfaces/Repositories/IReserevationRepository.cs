using FoodLink.Domain.Entities;
namespace FoodLink.Application.Common.Interfaces.Repositories;

public interface IReservationRepository
{
    Task<Reservation?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<List<Reservation>> GetByCharityIdAsync(Guid charityId, CancellationToken cancellationToken = default);
    Task<List<Reservation>> GetByDonationIdAsync(Guid donationId, CancellationToken cancellationToken = default);
    Task<List<Reservation>> GetExpiredPendingAsync(DateTime utcNow, CancellationToken cancellationToken = default);
    void Add(Reservation reservation);
    void Update(Reservation reservation);
}