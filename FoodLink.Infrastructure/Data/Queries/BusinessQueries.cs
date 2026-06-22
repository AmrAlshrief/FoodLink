using FoodLink.Application.Features.Businesses;
using FoodLink.Application.Features.Businesses.DTOs;
using FoodLink.Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace FoodLink.Infrastructure.Data.Queries;

public class BusinessQueries(AppDbContext dbContext) : IBusinessQueries
{
    public async Task<BusinessPublicProfileResponse?> GetPublicProfileAsync(Guid businessProfileId, CancellationToken ct = default)
    {
        return await dbContext.BusinessProfiles
            .AsNoTracking()
            .Where(b => b.Id == businessProfileId)
            .Select(b => new BusinessPublicProfileResponse
            {
                Id = b.Id,
                Name = b.User.Name,
                BusinessName = b.BusinessName,
                BusinessType = b.BusinessType,
                Address = b.Address,
                ProfileImage = b.User.ProfileImage,
                TotalDonations = dbContext.Donations.Count(d => d.BusinessProfileId == b.Id),
                ActiveDonations = dbContext.Donations.Count(d => d.BusinessProfileId == b.Id && 
                    (d.Status == DonationStatus.Available || d.Status == DonationStatus.PartiallyReserved)),
                CompletedDonations = dbContext.Donations.Count(d => d.BusinessProfileId == b.Id && d.Status == DonationStatus.Completed),
                CancelledDonations = dbContext.Donations.Count(d => d.BusinessProfileId == b.Id && d.Status == DonationStatus.Cancelled),
                TotalItemsDonated = dbContext.Donations
                    .Where(d => d.BusinessProfileId == b.Id)
                    .SelectMany(d => d.Items)
                    .Sum(i => i.TotalQuantity)
            })
            .FirstOrDefaultAsync(ct);
    }

    public async Task<BusinessPrivateProfileResponse?> GetPrivateProfileAsync(Guid businessProfileId, CancellationToken ct = default)
    {
        return await dbContext.BusinessProfiles
            .AsNoTracking()
            .Where(b => b.Id == businessProfileId)
            .Select(b => new BusinessPrivateProfileResponse
            {
                Id = b.Id,
                Name = b.User.Name,
                BusinessName = b.BusinessName,
                BusinessType = b.BusinessType,
                Address = b.Address,
                ProfileImage = b.User.ProfileImage,
                TotalDonations = dbContext.Donations.Count(d => d.BusinessProfileId == b.Id),
                ActiveDonations = dbContext.Donations.Count(d => d.BusinessProfileId == b.Id && 
                    (d.Status == DonationStatus.Available || d.Status == DonationStatus.PartiallyReserved)),
                CompletedDonations = dbContext.Donations.Count(d => d.BusinessProfileId == b.Id && d.Status == DonationStatus.Completed),
                CancelledDonations = dbContext.Donations.Count(d => d.BusinessProfileId == b.Id && d.Status == DonationStatus.Cancelled),
                TotalItemsDonated = dbContext.Donations
                    .Where(d => d.BusinessProfileId == b.Id)
                    .SelectMany(d => d.Items)
                    .Sum(i => i.TotalQuantity),
                Email = b.User.Email,
                Phone = b.User.Phone,
                IsSuspended = b.User.IsSuspended,
                CreatedAt = b.CreatedAtUtc.DateTime,
                TotalReservationsReceived = dbContext.Reservations.Count(r => r.Donation.BusinessProfileId == b.Id),
                PickedUpCount = dbContext.Reservations.Count(r => r.Donation.BusinessProfileId == b.Id && r.Status == ReservationStatus.PickedUp),
                NoShowCount = dbContext.Reservations.Count(r => r.Donation.BusinessProfileId == b.Id && r.Status == ReservationStatus.NoShow)
            })
            .FirstOrDefaultAsync(ct);
    }
}
