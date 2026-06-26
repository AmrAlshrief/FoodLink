using FoodLink.Application.Features.Charities;
using FoodLink.Application.Features.Charities.DTOs;
using FoodLink.Application.Features.Charities.Interfaces;
using FoodLink.Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace FoodLink.Infrastructure.Data.Queries;

public class CharityQueries(AppDbContext dbContext) : ICharityQueries
{
    public async Task<CharityPublicProfileResponse?> GetPublicProfileAsync(Guid charityProfileId, CancellationToken ct = default)
    {
        return await dbContext.CharityProfiles
            .AsNoTracking()
            .Where(c => c.Id == charityProfileId)
            .Select(c => new CharityPublicProfileResponse
            {
                Id = c.Id,
                Name = c.User.Name,
                OrganizationName = c.Name,
                LicenseNumber = c.LicenseNumber,
                Address = c.Address,
                ProfileImage = c.User.ProfileImage,
                TotalReservations = c.Reservations.Count,
                PickedUpCount = c.Reservations.Count(r => r.Status == ReservationStatus.PickedUp),
                NoShowCount = c.Reservations.Count(r => r.Status == ReservationStatus.NoShow),
                CancelledCount = c.Reservations.Count(r => r.Status == ReservationStatus.Cancelled),
                AverageRating = c.AverageRating,
                RatingCount = c.RatingCount
            })
            .FirstOrDefaultAsync(ct);
    }

    public async Task<CharityPrivateProfileResponse?> GetPrivateProfileAsync(Guid charityProfileId, CancellationToken ct = default)
    {
        return await dbContext.CharityProfiles
            .AsNoTracking()
            .Where(c => c.Id == charityProfileId)
            .Select(c => new CharityPrivateProfileResponse
            {
                Id = c.Id,
                Name = c.User.Name,
                OrganizationName = c.Name,
                LicenseNumber = c.LicenseNumber,
                Address = c.Address,
                ProfileImage = c.User.ProfileImage,
                TotalReservations = c.Reservations.Count,
                PickedUpCount = c.Reservations.Count(r => r.Status == ReservationStatus.PickedUp),
                NoShowCount = c.Reservations.Count(r => r.Status == ReservationStatus.NoShow),
                CancelledCount = c.Reservations.Count(r => r.Status == ReservationStatus.Cancelled),
                AverageRating = c.AverageRating,
                RatingCount = c.RatingCount,
                Email = c.User.Email,
                Phone = c.User.Phone,
                IsSuspended = c.User.IsSuspended,
                CreatedAt = c.CreatedAtUtc,
                ActiveReservations = c.Reservations.Count(r => r.Status == ReservationStatus.Pending)
            })
            .FirstOrDefaultAsync(ct);
    }
}
