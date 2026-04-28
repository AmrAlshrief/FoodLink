using FoodLink.Application.Common.Interfaces.Repositories;
using FoodLink.Domain.Entities;
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
            .FirstOrDefaultAsync(d => d.Id == id, cancellationToken);
    }

    public async Task<List<Donation>> GetActiveDonationsAsync(CancellationToken cancellationToken = default)
    {
        var now = DateTime.UtcNow;
        return await dbContext.Donations
            .Include(d => d.Items)
            .Where(d => d.ExpiryDate > now)
            .ToListAsync(cancellationToken);
    }

    public async Task<List<Donation>> GetByBusinessIdAsync(
    Guid businessId,
    CancellationToken cancellationToken = default)
    {
        var donations = await dbContext.Donations
            .Include(d => d.Items)
            .Where(d => d.BusinessProfileId == businessId)
            //.OrderByDescending(d => d.CreatedAtUtc)
            .ToListAsync(cancellationToken);
            
        return donations.OrderByDescending(d => d.CreatedAtUtc).ToList();
    }
}