namespace FoodLink.Application.Features.Reservation.Interfaces;

public interface IReservationRepository
{
    Task<FoodLink.Domain.Entities.Reservation?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<List<FoodLink.Domain.Entities.Reservation>> GetByCharityIdAsync(Guid charityId, CancellationToken cancellationToken = default);
    Task<List<FoodLink.Domain.Entities.Reservation>> GetByDonationIdAsync(Guid donationId, CancellationToken cancellationToken = default);
    Task<List<FoodLink.Domain.Entities.Reservation>> GetExpiredPendingAsync(DateTime utcNow, CancellationToken cancellationToken = default);
    Task<FoodLink.Domain.Entities.Reservation?> GetByIdWithReviewDataAsync(
        Guid id,
        CancellationToken cancellationToken = default);
    void Add(FoodLink.Domain.Entities.Reservation reservation);
    void Update(FoodLink.Domain.Entities.Reservation reservation);
}
