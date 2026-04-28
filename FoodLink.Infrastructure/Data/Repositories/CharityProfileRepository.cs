using FoodLink.Application.Common.Interfaces.Repositories;
using FoodLink.Domain.Entities.Profiles;
using Microsoft.EntityFrameworkCore;

namespace FoodLink.Infrastructure.Data.Repositories;

public class CharityProfileRepository(AppDbContext dbContext) : ICharityProfileRepository
{
    public async Task<List<CharityProfile>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await dbContext.CharityProfiles.ToListAsync(cancellationToken);
    }

    public async Task<CharityProfile?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await dbContext.CharityProfiles
            .FirstOrDefaultAsync(c => c.Id == id, cancellationToken);
    }

    public async Task<CharityProfile?> GetByUserIdAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        return await dbContext.CharityProfiles
            .FirstOrDefaultAsync(c => c.UserId == userId, cancellationToken);
    }

    public void Add(CharityProfile profile)
    {
        dbContext.CharityProfiles.Add(profile);
    }
}