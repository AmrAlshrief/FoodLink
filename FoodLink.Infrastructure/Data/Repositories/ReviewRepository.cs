using FoodLink.Application.Features.Reviews.Interfaces;
using FoodLink.Domain.Entities;
using FoodLink.Domain.Enums;
using FoodLink.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace FoodLink.Infrastructure.Repositories;

public class ReviewRepository(AppDbContext dbContext)
    : IReviewRepository
{
    public async Task AddAsync(
        Review review,
        CancellationToken cancellationToken = default)
    {
        await dbContext.Reviews.AddAsync(
            review,
            cancellationToken);
    }

    public Task<bool> ExistsAsync(
        Guid reservationId,
        Guid reviewerId,
        ReviewType type,
        CancellationToken cancellationToken = default)
    {
        return dbContext.Reviews.AnyAsync(
            r =>
                r.ReservationId == reservationId &&
                r.ReviewerId == reviewerId &&
                r.Type == type,
            cancellationToken);
    }
}