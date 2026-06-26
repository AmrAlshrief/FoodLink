using FoodLink.Application.Features.Charities;
using FoodLink.Application.Features.Charities.Interfaces;
using FoodLink.Domain.Entities.Profiles;
using Microsoft.EntityFrameworkCore;

namespace FoodLink.Infrastructure.Data.Repositories;

public class CharityRepository(AppDbContext dbContext) : ICharityRepository
{
    public async Task<CharityProfile?> GetByIdAsync(Guid charityProfileId, CancellationToken ct = default)
    {
        return await dbContext.CharityProfiles
            .Include(c => c.User)
            .FirstOrDefaultAsync(c => c.Id == charityProfileId, ct);
    }

    public Task UpdateAsync(CharityProfile charity, CancellationToken ct = default)
    {
        return Task.CompletedTask;
    }
}
