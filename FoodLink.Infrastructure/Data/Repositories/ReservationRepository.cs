using FoodLink.Application.Common.Interfaces.Repositories;
using FoodLink.Domain.Entities;
using FoodLink.Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace FoodLink.Infrastructure.Data.Repositories;

public class ReservationRepository(AppDbContext dbContext) : IReservationRepository
{
    public async Task<Reservation?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await dbContext.Reservations
            .Include(r => r.Items)
            .FirstOrDefaultAsync(r => r.Id == id, cancellationToken);
    }

    public async Task<List<Reservation>> GetByCharityIdAsync(Guid charityId, CancellationToken cancellationToken = default)
    {
        return await dbContext.Reservations
            .Include(r => r.Items)
            .Where(r => r.CharityId == charityId)
            .OrderByDescending(r => r.CreatedAtUtc)
            .ToListAsync(cancellationToken);
    }

    public async Task<List<Reservation>> GetByDonationIdAsync(Guid donationId, CancellationToken cancellationToken = default)
    {
        return await dbContext.Reservations
            .Include(r => r.Items)
            .Where(r => r.DonationId == donationId)
            .OrderByDescending(r => r.CreatedAtUtc)
            .ToListAsync(cancellationToken);
    }

    public async Task<List<Reservation>> GetExpiredPendingAsync(DateTime utcNow, CancellationToken cancellationToken = default)
    {
        return await dbContext.Reservations
            .Include(r => r.Items)
            .Where(r => r.Status == ReservationStatus.Pending && r.ExpiresAt <= utcNow)
            .ToListAsync(cancellationToken);
    }

    public void Add(Reservation reservation)
    {
        dbContext.Reservations.Add(reservation);
    }

    public void Update(Reservation reservation)
    {
        dbContext.Reservations.Update(reservation);
    }
}
