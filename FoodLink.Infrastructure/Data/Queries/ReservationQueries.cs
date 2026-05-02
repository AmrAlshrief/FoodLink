using FoodLink.Application.Features.Reservation.Dtos;
using FoodLink.Application.Common.Interfaces.Services.Queries;
using Microsoft.EntityFrameworkCore;
using FoodLink.Domain.Common.Exceptions;
using FoodLink.Domain.Enums;

namespace FoodLink.Infrastructure.Data.Queries;

public class ReservationQueries(AppDbContext dbContext) : IReservationQueries
{
    public async Task<List<ReservationResponse>> GetMyReservationsAsync(Guid charityId, CancellationToken cancellationToken = default)
    {
        var query = await dbContext.Reservations
            .AsNoTracking()
            .Where(r => r.CharityId == charityId)
            .Select(r => new ReservationResponse
            {
                Id = r.Id,
                Status = r.Status.ToString(),
                ExpiresAt = r.ExpiresAt,
                PickedUpAt = r.PickedUpAt,

                Donation = dbContext.Donations
                    .Where(d => d.Id == r.DonationId)
                    .Select(d => new DonationSummaryDto
                    {
                        Id = d.Id,
                        Title = d.Title,
                        ImageUrl = d.ImageUrl,
                        BusinessName = dbContext.BusinessProfiles
                            .Where(b => b.Id == d.BusinessProfileId)
                            .Select(b => b.BusinessName)
                            .FirstOrDefault() ?? string.Empty
                    })
                    .FirstOrDefault()!,

                Charity = dbContext.CharityProfiles
                    .Where(c => c.Id == r.CharityId)
                    .Select(c => new CharitySummaryDto
                    {
                        Id = c.Id,
                        Name = c.Name
                    })
                    .FirstOrDefault()!,

                Items = r.Items.Select(i => new ReservationItemResponse
                {
                    DonationItemId = i.DonationItemId,
                    ItemName = i.ItemName,
                    Unit = i.Unit,
                    Quantity = i.Quantity
                }).ToList(),

                TotalItems = r.Items.Count,
                TotalQuantity = r.Items.Sum(i => i.Quantity)
            })
            .ToListAsync(cancellationToken);

        return query;
    }

    public async Task<List<ReservationResponse>> GetReservationsAsync(
    Guid charityId,
    ReservationFilterRequest filter,
    CancellationToken cancellationToken = default)
    {
        var now = DateTime.UtcNow;

        var reservationsQuery = dbContext.Reservations
            .AsNoTracking()
            .Where(r => r.CharityId == charityId);

        if (!string.IsNullOrWhiteSpace(filter.Status) &&
            Enum.TryParse<ReservationStatus>(filter.Status, true, out var status))
        {
            reservationsQuery = reservationsQuery.Where(r => r.Status == status);
        }

        if (filter.IsExpired.HasValue)
        {
            reservationsQuery = filter.IsExpired.Value
                ? reservationsQuery.Where(r => r.ExpiresAt <= now)
                : reservationsQuery.Where(r => r.ExpiresAt > now);
        }

        var result = await reservationsQuery
            .Select(r => new ReservationResponse
            {
                Id = r.Id,
                Status = r.Status.ToString(),
                ExpiresAt = r.ExpiresAt,
                PickedUpAt = r.PickedUpAt,

                Donation = dbContext.Donations
                    .Where(d => d.Id == r.DonationId)
                    .Select(d => new DonationSummaryDto
                    {
                        Id = d.Id,
                        Title = d.Title,
                        ImageUrl = d.ImageUrl,
                        BusinessName = dbContext.BusinessProfiles
                            .Where(b => b.Id == d.BusinessProfileId)
                            .Select(b => b.BusinessName)
                            .FirstOrDefault() ?? string.Empty
                    })
                    .FirstOrDefault()!,

                Charity = dbContext.CharityProfiles
                    .Where(c => c.Id == r.CharityId)
                    .Select(c => new CharitySummaryDto
                    {
                        Id = c.Id,
                        Name = c.Name
                    })
                    .FirstOrDefault()!,

                Items = r.Items.Select(i => new ReservationItemResponse
                {
                    DonationItemId = i.DonationItemId,
                    ItemName = i.ItemName,
                    Unit = i.Unit,
                    Quantity = i.Quantity
                }).ToList(),

                TotalItems = r.Items.Count,
                TotalQuantity = r.Items.Sum(i => i.Quantity)
            })
            .ToListAsync(cancellationToken);

        return result;
    }

    public async Task<List<ReservationResponse>> GetReservationsByDonationAsync(
    Guid businessId,
    Guid donationId,
    ReservationFilterRequest filter,
    CancellationToken cancellationToken = default)
    {
        var now = DateTime.UtcNow;

        var donationExists = await dbContext.Donations
            .AnyAsync(d => d.Id == donationId && d.BusinessProfileId == businessId, cancellationToken);

        if (!donationExists)
            throw new DomainException("Donation not found or you are not allowed to access it.");

        var reservationsQuery = dbContext.Reservations
            .AsNoTracking()
            .Where(r => r.DonationId == donationId);

        if (!string.IsNullOrWhiteSpace(filter.Status) &&
            Enum.TryParse<ReservationStatus>(filter.Status, true, out var status))
        {
            reservationsQuery = reservationsQuery.Where(r => r.Status == status);
        }

        if (filter.IsExpired.HasValue)
        {
            reservationsQuery = filter.IsExpired.Value
                ? reservationsQuery.Where(r => r.ExpiresAt <= now)
                : reservationsQuery.Where(r => r.ExpiresAt > now);
        }

        return await reservationsQuery
            .Select(r => new ReservationResponse
            {
                Id = r.Id,
                Status = r.Status.ToString(),
                ExpiresAt = r.ExpiresAt,
                PickedUpAt = r.PickedUpAt,

                Donation = dbContext.Donations
                    .Where(d => d.Id == r.DonationId)
                    .Select(d => new DonationSummaryDto
                    {
                        Id = d.Id,
                        Title = d.Title,
                        ImageUrl = d.ImageUrl,
                        BusinessName = dbContext.BusinessProfiles
                            .Where(b => b.Id == d.BusinessProfileId)
                            .Select(b => b.BusinessName)
                            .FirstOrDefault() ?? string.Empty
                    })
                    .FirstOrDefault()!,

                Charity = dbContext.CharityProfiles
                    .Where(c => c.Id == r.CharityId)
                    .Select(c => new CharitySummaryDto
                    {
                        Id = c.Id,
                        Name = c.Name
                    })
                    .FirstOrDefault()!,

                Items = r.Items.Select(i => new ReservationItemResponse
                {
                    DonationItemId = i.DonationItemId,
                    ItemName = i.ItemName,
                    Unit = i.Unit,
                    Quantity = i.Quantity
                }).ToList(),

                TotalItems = r.Items.Count,
                TotalQuantity = r.Items.Sum(i => i.Quantity)
            })
            .ToListAsync(cancellationToken);
    }

    
}