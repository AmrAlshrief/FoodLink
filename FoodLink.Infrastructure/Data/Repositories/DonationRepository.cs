using FoodLink.Application.Common.Interfaces.Repositories;
using FoodLink.Domain.Entities;
using FoodLink.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace FoodLink.Infrastructure.Data.Repositories;

public class DonationRepository(AppDbContext context) : IDonationRepository
{
    public async Task AddAsync(Donation donation)
    {
        await context.Donations.AddAsync(donation);
    }

    public void Add(Donation donation)
    {
        context.Donations.Add(donation);
    }

    public void Update(Donation donation)
    {
        context.Donations.Update(donation);
    }

    public async Task<Donation?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await context.Donations
            .Include(d => d.Items)
            .FirstOrDefaultAsync(d => d.Id == id, cancellationToken);
    }

    public async Task<List<Donation>> GetActiveDonationsAsync(CancellationToken cancellationToken = default)
    {
        var now = DateTime.UtcNow;
        return await context.Donations
            .Include(d => d.Items)
            .Where(d => d.ExpiryDate > now)
            .ToListAsync(cancellationToken);
    }
}