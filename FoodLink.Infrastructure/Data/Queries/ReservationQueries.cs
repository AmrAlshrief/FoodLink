using FoodLink.Application.Features.Reservation.Dtos;
using FoodLink.Application.Common.Interfaces.Services.Queries;
using FoodLink.Domain.Common.Exceptions;
using FoodLink.Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace FoodLink.Infrastructure.Data.Queries;

public class ReservationQueries(AppDbContext dbContext) : IReservationQueries
{
    public async Task<List<ReservationResponse>> GetMyReservationsAsync(
        Guid charityId,
        CancellationToken cancellationToken = default)
    {
        return await BuildReservationResponseQuery()
            .Where(r => r.Charity.Id == charityId)
            .ToListAsync(cancellationToken);
    }

    public async Task<List<ReservationResponse>> GetReservationsAsync(
        Guid charityId,
        ReservationFilterRequest filter,
        CancellationToken cancellationToken = default)
    {
        var query = BuildReservationResponseQuery()
            .Where(r => r.Charity.Id == charityId);

        query = ApplyFilter(query, filter);

        return await query.ToListAsync(cancellationToken);
    }

    public async Task<List<ReservationResponse>> GetReservationsByDonationAsync(
        Guid? businessId,
        Guid donationId,
        ReservationFilterRequest filter,
        CancellationToken cancellationToken = default)
    {
        var donationExistsQuery = dbContext.Donations.AsNoTracking().Where(d => d.Id == donationId);

        if (businessId.HasValue)
        {
            donationExistsQuery = donationExistsQuery.Where(d => d.BusinessProfileId == businessId.Value);
        }

        var donationExists = await donationExistsQuery.AnyAsync(cancellationToken);

        if (!donationExists)
            throw new DomainException("Donation not found or you are not allowed to access it.");

        var query = BuildReservationResponseQuery()
            .Where(r => r.Donation.Id == donationId);

        query = ApplyFilter(query, filter);

        return await query.ToListAsync(cancellationToken);
    }

    private IQueryable<ReservationResponse> BuildReservationResponseQuery()
    {
        return dbContext.Reservations
            .AsNoTracking()
            .Select(r => new ReservationResponse
            {
                Id = r.Id,
                Status = r.Status.ToString(),
                ExpiresAt = r.ExpiresAt,
                PickedUpAt = r.PickedUpAt,

                // Donation = dbContext.Donations
                //     .Where(d => d.Id == r.DonationId)
                //     .Select(d => new DonationSummaryDto
                //     {
                //         Id = d.Id,
                //         Title = d.Title,
                //         ImageUrl = d.ImageUrl,

                //         BusinessName = dbContext.BusinessProfiles
                //             .Where(b => b.Id == d.BusinessProfileId)
                //             .Select(b => b.BusinessName)
                //             .FirstOrDefault() ?? string.Empty
                //     })
                //     .FirstOrDefault()!,

                // Charity = dbContext.CharityProfiles
                //     .Where(c => c.Id == r.CharityId)
                //     .Select(c => new CharitySummaryDto
                //     {
                //         Id = c.Id,
                //         Name = c.Name
                //     })
                //     .FirstOrDefault()!,

                Donation = new DonationSummaryDto
                {
                    Id = r.Donation.Id,
                    Title = r.Donation.Title,
                    ImageUrl = r.Donation.ImageUrl,
                    BusinessName = r.Donation.BusinessProfile.BusinessName ?? string.Empty
                },
                Charity = new CharitySummaryDto
                {
                    Id = r.Charity.Id,
                    Name = r.Charity.Name
                },

                Items = r.Items.Select(i => new ReservationItemResponse
                {
                    DonationItemId = i.DonationItemId,
                    ItemName = i.ItemName,
                    Unit = i.Unit,
                    Quantity = i.Quantity
                }).ToList(),

                TotalItems = r.Items.Count,
                TotalQuantity = r.Items.Sum(i => i.Quantity)
            });
    }

    private static IQueryable<ReservationResponse> ApplyFilter(
        IQueryable<ReservationResponse> query,
        ReservationFilterRequest filter)
    {
        var now = DateTime.UtcNow;

        if (!string.IsNullOrWhiteSpace(filter.Status))
        {
            query = query.Where(r => r.Status == filter.Status);
        }

        if (filter.IsExpired.HasValue)
        {
            query = filter.IsExpired.Value
                ? query.Where(r => r.ExpiresAt <= now)
                : query.Where(r => r.ExpiresAt > now);
        }

        return query;
    }
}