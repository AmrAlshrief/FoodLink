using FoodLink.Application.Features.Dashboard.Admin.DTOs;

using FoodLink.Application.Common.Interfaces.Services.Queries;
using FoodLink.Application.Common.Models.Pagination;
using FoodLink.Domain.Enums;
using FoodLink.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace FoodLink.Infrastructure.Data.Queries;

public class AdminQueries(AppDbContext dbContext) : IAdminQueries
{
    public async Task<AdminDashboardStatsResponse> GetDashboardStatsAsync(
    CancellationToken cancellationToken = default)
    {
        var reservationCounts = await dbContext.Reservations
            .GroupBy(r => r.Status)
            .Select(g => new { Status = g.Key, Count = g.Count() })
            .ToListAsync(cancellationToken);

        var donationCounts = await dbContext.Donations
            .GroupBy(d => d.Status)
            .Select(g => new { Status = g.Key, Count = g.Count() })
            .ToListAsync(cancellationToken);

        var totalReservations = reservationCounts.Sum(x => x.Count);
        var totalDonations = donationCounts.Sum(x => x.Count);

        return new AdminDashboardStatsResponse
        {
            TotalCharities  = await dbContext.CharityProfiles.CountAsync(cancellationToken),
            TotalBusinesses = await dbContext.BusinessProfiles.CountAsync(cancellationToken),

            TotalReservations  = totalReservations,
            PendingReservations = reservationCounts
                .FirstOrDefault(x => x.Status == ReservationStatus.Pending)?.Count ?? 0,
            PickedUpReservations = reservationCounts
                .FirstOrDefault(x => x.Status == ReservationStatus.PickedUp)?.Count ?? 0,
            NoShowReservations = reservationCounts
                .FirstOrDefault(x => x.Status == ReservationStatus.NoShow)?.Count ?? 0,

            TotalDonations  = totalDonations,
            ActiveDonations = donationCounts
                .Where(x => x.Status is DonationStatus.Available or DonationStatus.PartiallyReserved)
                .Sum(x => x.Count),
            ExpiredDonations = donationCounts
                .FirstOrDefault(x => x.Status == DonationStatus.Expired)?.Count ?? 0,
        };
    }

    public async Task<PagedResponse<AdminCharityResponse>> GetCharitiesAsync(
    AdminFilterRequest request,
    CancellationToken cancellationToken = default)
    {
        
        var query = dbContext.CharityProfiles
            .AsNoTracking();

        if (!string.IsNullOrWhiteSpace(request.Search))
        {
            var search = request.Search.ToLower();

            query = query.Where(c =>
                c.Name.ToLower().Contains(search) ||
                c.User.Email.ToLower().Contains(search));
        }

        if (request.IsSuspended.HasValue)
        {
            query = query.Where(c =>
                c.User.IsSuspended == request.IsSuspended.Value);
        }
        
        var totalCount = await query.CountAsync(cancellationToken);

        query = query
            .Skip((request.Page - 1) * request.PageSize)
            .Take(request.PageSize);

        var items = await query
            .Select(c => new AdminCharityResponse
            {
                Id = c.Id,
                UserId = c.UserId,

                Name = c.Name,
                LicenseNumber = c.LicenseNumber,
                Address = c.Address,
                Email = c.User.Email,
                Phone = c.User.Phone,

                ProfileImage = c.User.ProfileImage,

                IsSuspended = c.User.IsSuspended,

                TotalReservations = c.Reservations.Count,

                PickedUpReservations = c.Reservations
                    .Count(r => r.Status == ReservationStatus.PickedUp),

                NoShowReservations = c.Reservations
                    .Count(r => r.Status == ReservationStatus.NoShow),

                CreatedAt = c.CreatedAtUtc.DateTime
            })
            .ToListAsync(cancellationToken);

        return new PagedResponse<AdminCharityResponse>
        {
            Items = items,
            Page = request.Page,
            PageSize = request.PageSize,
            TotalCount = totalCount,
            TotalPages = (int)Math.Ceiling(
                totalCount / (double)request.PageSize)
        };

    }

    public async Task<AdminCharityResponse?> GetCharityByIdAsync(
    Guid charityId,
    CancellationToken cancellationToken = default)
    {
        return await dbContext.CharityProfiles
            .Where(c => c.Id == charityId)
            .Select(c => new AdminCharityResponse
            {
                Id = c.Id,
                UserId = c.UserId,
                Name = c.Name,
                LicenseNumber = c.LicenseNumber,
                Address = c.Address,
                Email = c.User.Email,
                Phone = c.User.Phone,
                ProfileImage = c.User.ProfileImage,
                IsSuspended = c.User.IsSuspended,
                TotalReservations = c.Reservations.Count,
                PickedUpReservations = c.Reservations
                    .Count(r => r.Status == ReservationStatus.PickedUp),
                NoShowReservations = c.Reservations
                    .Count(r => r.Status == ReservationStatus.NoShow),
                CreatedAt = c.CreatedAtUtc.DateTime
            })
            .FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<PagedResponse<AdminBusinessResponse>> GetBusinessesAsync(
        AdminFilterRequest request,
        CancellationToken cancellationToken = default)
    {
        var query = dbContext.BusinessProfiles.AsNoTracking();

        if (!string.IsNullOrWhiteSpace(request.Search))
        {
            var search = request.Search.ToLower();

            query = query.Where(b =>
                b.BusinessName.ToLower().Contains(search) ||
                b.User.Email.ToLower().Contains(search));
        }

        if (request.IsSuspended.HasValue)
        {
            query = query.Where(b =>
                b.User.IsSuspended == request.IsSuspended.Value);
        }

        var totalCount = await query.CountAsync(cancellationToken);

        query = query
            .Skip((request.Page - 1) * request.PageSize)
            .Take(request.PageSize);

        var items = await query
            .Select(b => new AdminBusinessResponse
            {
                Id = b.Id,
                UserId = b.UserId,
                BusinessName = b.BusinessName,
                BusinessType = b.BusinessType,
                Address = b.Address,
                Email = b.User.Email,
                Phone = b.User.Phone,
                ProfileImage = b.User.ProfileImage,
                IsSuspended = b.User.IsSuspended,
                TotalDonations = dbContext.Donations.Count(d => d.BusinessProfileId == b.Id),
                ActiveDonations = dbContext.Donations.Count(d => d.BusinessProfileId == b.Id && 
                    (d.Status == DonationStatus.Available || d.Status == DonationStatus.PartiallyReserved)),
                ExpiredDonations = dbContext.Donations.Count(d => d.BusinessProfileId == b.Id && d.Status == DonationStatus.Expired),
                CreatedAt = b.CreatedAtUtc.DateTime
            })
            .ToListAsync(cancellationToken);

        return new PagedResponse<AdminBusinessResponse>
        {
            Items = items,
            Page = request.Page,
            PageSize = request.PageSize,
            TotalCount = totalCount,
            TotalPages = (int)Math.Ceiling(totalCount / (double)request.PageSize)
        };
    }

    public async Task<AdminBusinessResponse?> GetBusinessByIdAsync(
        Guid businessId,
        CancellationToken cancellationToken = default)
    {
        return await dbContext.BusinessProfiles
            .Where(b => b.Id == businessId)
            .Select(b => new AdminBusinessResponse
            {
                Id = b.Id,
                UserId = b.UserId,
                BusinessName = b.BusinessName,
                BusinessType = b.BusinessType,
                Address = b.Address,
                Email = b.User.Email,
                Phone = b.User.Phone,
                ProfileImage = b.User.ProfileImage,
                IsSuspended = b.User.IsSuspended,
                TotalDonations = dbContext.Donations.Count(d => d.BusinessProfileId == b.Id),
                ActiveDonations = dbContext.Donations.Count(d => d.BusinessProfileId == b.Id && 
                    (d.Status == DonationStatus.Available || d.Status == DonationStatus.PartiallyReserved)),
                ExpiredDonations = dbContext.Donations.Count(d => d.BusinessProfileId == b.Id && d.Status == DonationStatus.Expired),
                CreatedAt = b.CreatedAtUtc.DateTime
            })
            .FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<PagedResponse<AdminUserResponse>> GetUsersAsync(
    AdminUserFilterRequest request,
    CancellationToken cancellationToken = default)
    {
        var query = dbContext.Users
            .Where(u => u.Role != 0) // exclude admins
            .AsQueryable();

        // filters
        if (request.Role.HasValue)
            query = query.Where(u => u.Role == request.Role.Value);

        if (request.IsSuspended.HasValue)
            query = query.Where(u => u.IsSuspended == request.IsSuspended.Value);

        if (!string.IsNullOrWhiteSpace(request.Search))
            query = query.Where(u => u.Name.Contains(request.Search) ||
                                    u.Email.Contains(request.Search));

        var totalCount = await query.CountAsync(cancellationToken);

        var items = await query
            .OrderBy(u => u.Name)
            .Skip((request.Page - 1) * request.PageSize)
            .Take(request.PageSize)
            .Select(u => new AdminUserResponse
            {
                Id = u.Id,
                Name = u.Name,
                Email = u.Email,
                Role = u.Role.ToString(),
                Phone = u.Phone,
                ProfileImage = string.IsNullOrEmpty(u.ProfileImage) ? null : u.ProfileImage,
                IsSuspended = u.IsSuspended,
                CreatedAt = u.CreatedAtUtc.Date
            })
            .ToListAsync(cancellationToken);

        return new PagedResponse<AdminUserResponse>
        {
            Items = items,
            Page = request.Page,
            PageSize = request.PageSize,
            TotalCount = totalCount,
            TotalPages = (int)Math.Ceiling(totalCount / (double)request.PageSize)
        };
    }
}