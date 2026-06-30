using FoodLink.Application.Features.Donation.Interfaces;
using FoodLink.Domain.Entities;
using FoodLink.Domain.Enums;
using FoodLink.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace FoodLink.Infrastructure.Data.Repositories;

public class DonationRepository(AppDbContext dbContext) : IDonationRepository
{
    public async Task AddAsync(Donation donation)
    {
        await dbContext.Donations.AddAsync(donation);
    }

    public void Add(Donation donation)
    {
        dbContext.Donations.Add(donation);
    }

    public void Update(Donation donation)
    {
        dbContext.Donations.Update(donation);
    }

    public void Remove(Donation donation)
    {
        dbContext.Donations.Remove(donation);
    }

    public async Task<Donation?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await dbContext.Donations
            .Include(d => d.Items)
            .Include(d => d.BusinessProfile).ThenInclude(b => b.User)
            .FirstOrDefaultAsync(d => d.Id == id, cancellationToken);
    }

    public async Task<List<Donation>> GetActiveDonationsAsync(CancellationToken cancellationToken = default)
    {
        var now = DateTime.UtcNow;
        return await dbContext.Donations
            .Include(d => d.Items)
            .Include(d => d.BusinessProfile).ThenInclude(b => b.User)
            .Where(d => d.ExpiryDate > now)
            .ToListAsync(cancellationToken);
    }

    public async Task<List<Donation>> GetByBusinessIdAsync(
    Guid businessId,
    CancellationToken cancellationToken = default)
    {
        var donations = await dbContext.Donations
            .Include(d => d.Items)
            .Include(d => d.BusinessProfile).ThenInclude(b => b.User)
            .Where(d => d.BusinessProfileId == businessId)
            //.OrderByDescending(d => d.CreatedAtUtc)
            .ToListAsync(cancellationToken);
            
        return donations.OrderByDescending(d => d.CreatedAtUtc).ToList();
    }

    public async Task<List<Donation>> GetExpiredActiveDonationsAsync(
    DateTime utcNow,
    CancellationToken cancellationToken = default)
    {
        return await dbContext.Donations
            .Include(d => d.Items)
            .Include(d => d.BusinessProfile).ThenInclude(b => b.User)
            .Where(d => d.ExpiryDate <= utcNow &&
                (d.Status == DonationStatus.Available ||
                d.Status == DonationStatus.PartiallyReserved ||
                d.Status == DonationStatus.FullyReserved))
            .ToListAsync(cancellationToken);
    }

    public async Task<(List<Donation> Items, int TotalCount)> GetDonationsByBusinessIdPagedAsync(
        Guid businessId,
        DonationScope scope,
        string? search,
        DonationStatus? status,
        string? sortBy,
        string? sortDirection,
        int page,
        int pageSize,
        CancellationToken cancellationToken = default)
    {
        var query = dbContext.Donations
            .Include(d => d.Items)
            .Include(d => d.BusinessProfile).ThenInclude(b => b.User)
            .Where(d => d.BusinessProfileId == businessId);

        // Apply Scope
        if (scope == DonationScope.Current)
        {
            query = query.Where(d => 
                d.Status == DonationStatus.Available || 
                d.Status == DonationStatus.PartiallyReserved || 
                d.Status == DonationStatus.FullyReserved);
        }
        else if (scope == DonationScope.History)
        {
            query = query.Where(d => 
                d.Status == DonationStatus.Expired || 
                d.Status == DonationStatus.Completed || 
                d.Status == DonationStatus.Cancelled);
        }

        // Apply Search
        if (!string.IsNullOrWhiteSpace(search))
        {
            var searchTerm = search.ToLower();
            query = query.Where(d => 
                d.Title.ToLower().Contains(searchTerm) || 
                (d.Description != null && d.Description.ToLower().Contains(searchTerm)));
        }

        // Apply Status Filter
        if (status.HasValue)
        {
            query = query.Where(d => d.Status == status.Value);
        }

        var totalCount = await query.CountAsync(cancellationToken);

        // Apply Sorting
        bool isDesc = !string.Equals(sortDirection, "asc", StringComparison.OrdinalIgnoreCase);
        query = sortBy?.ToLower() switch
        {
            "title" => isDesc ? query.OrderByDescending(d => d.Title) : query.OrderBy(d => d.Title),
            "expirationdate" => isDesc ? query.OrderByDescending(d => d.ExpiryDate) : query.OrderBy(d => d.ExpiryDate),
            "status" => isDesc ? query.OrderByDescending(d => d.Status) : query.OrderBy(d => d.Status),
            _ => isDesc ? query.OrderByDescending(d => d.CreatedAtUtc) : query.OrderBy(d => d.CreatedAtUtc)
        };

        var items = await query
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);

        return (items, totalCount);
    }
}