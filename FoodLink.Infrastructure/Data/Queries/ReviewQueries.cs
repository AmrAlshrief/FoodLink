using FoodLink.Application.Common.Models.Pagination;
using FoodLink.Application.Features.Reviews.DTOs;
using FoodLink.Application.Features.Reviews.Interfaces;
using FoodLink.Domain.Entities;
using FoodLink.Domain.Entities.Profiles;
using FoodLink.Domain.Enums;
using FoodLink.Domain.Common.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace FoodLink.Infrastructure.Data.Queries;

public class ReviewQueries : IReviewQueries
{
    private readonly AppDbContext _context;

    public ReviewQueries(AppDbContext context)
    {
        _context = context;
    }

    public async Task<PagedResponse<GroupedReviewResponse>> GetBusinessReviewsAsync(
        Guid businessId,
        PaginationRequest request,
        CancellationToken cancellationToken = default)
    {
        var businessUserId = await _context.BusinessProfiles
            .AsNoTracking()
            .Where(b => b.Id == businessId)
            .Select(b => (Guid?)b.UserId)
            .FirstOrDefaultAsync(cancellationToken);

        if (businessUserId is null)
        {
            return new PagedResponse<GroupedReviewResponse>
            {
                Page = request.Page,
                PageSize = request.PageSize,
                TotalCount = 0,
                TotalPages = 0
            };
        }

        var query = _context.Reviews
            .AsNoTracking()
            .Where(r =>
                r.TargetId == businessUserId &&
                r.Type == ReviewType.CharityToBusiness);

        var groupedQuery = query.GroupBy(r => r.ReviewerId);
        var totalCount = await groupedQuery.CountAsync(cancellationToken);

        if (totalCount == 0)
        {
            return new PagedResponse<GroupedReviewResponse>
            {
                Page = request.Page,
                PageSize = request.PageSize,
                TotalCount = 0,
                TotalPages = 0
            };
        }

        var reviewerIdsForPage = await groupedQuery
            .OrderBy(g => g.Key)
            .Skip((request.Page - 1) * request.PageSize)
            .Take(request.PageSize)
            .Select(g => g.Key)
            .ToListAsync(cancellationToken);

        var rawReviews = await query
            .Where(r => reviewerIdsForPage.Contains(r.ReviewerId))
            .OrderByDescending(r => r.CreatedAtUtc)
            .ToListAsync(cancellationToken);

        var charityDetails = await _context.CharityProfiles
            .Where(c => reviewerIdsForPage.Contains(c.UserId))
            .Select(c => new { c.UserId, c.Name })
            .ToListAsync(cancellationToken);

        var userDetails = await _context.Users
            .Where(u => reviewerIdsForPage.Contains(u.Id))
            .Select(u => new { u.Id, u.ProfileImage })
            .ToListAsync(cancellationToken);

        var items = rawReviews
            .GroupBy(r => r.ReviewerId)
            .Select(g => new GroupedReviewResponse
            {
                ReviewerId = g.Key,
                ReviewerName = charityDetails.FirstOrDefault(c => c.UserId == g.Key)?.Name ?? string.Empty,
                ReviewerLogo = userDetails.FirstOrDefault(u => u.Id == g.Key)?.ProfileImage,
                ReviewCount = g.Count(),
                Reviews = g.Select(r => new ReviewResponse
                {
                    Id = r.Id,
                    Rating = r.Rating,
                    Comment = r.Comment,
                    ReviewerName = charityDetails.FirstOrDefault(c => c.UserId == g.Key)?.Name ?? string.Empty,
                    ReviewerLogo = userDetails.FirstOrDefault(u => u.Id == g.Key)?.ProfileImage,
                    Type = r.Type.ToString(),
                    CreatedAt = r.CreatedAtUtc
                }).ToList()
            })
            .ToList();

        return new PagedResponse<GroupedReviewResponse>
        {
            Items = items,
            Page = request.Page,
            PageSize = request.PageSize,
            TotalCount = totalCount,
            TotalPages = (int)Math.Ceiling(totalCount / (double)request.PageSize)
        };
    }

    public async Task<PagedResponse<GroupedReviewResponse>> GetCharityReviewsAsync(
        Guid charityId,
        PaginationRequest request,
        CancellationToken cancellationToken = default)
    {
        var charityUserId = await _context.CharityProfiles
            .Where(c => c.Id == charityId)
            .Select(c => (Guid?)c.UserId)
            .FirstOrDefaultAsync(cancellationToken);

        if (charityUserId is null)
        {
            return new PagedResponse<GroupedReviewResponse>
            {
                Page = request.Page,
                PageSize = request.PageSize,
                TotalCount = 0,
                TotalPages = 0
            };
        }

        var query = _context.Reviews
            .AsNoTracking()
            .Where(r =>
                r.TargetId == charityUserId &&
                r.Type == ReviewType.BusinessToCharity);

        var groupedQuery = query.GroupBy(r => r.ReviewerId);
        var totalCount = await groupedQuery.CountAsync(cancellationToken);

        if (totalCount == 0)
        {
            return new PagedResponse<GroupedReviewResponse>
            {
                Page = request.Page,
                PageSize = request.PageSize,
                TotalCount = 0,
                TotalPages = 0
            };
        }

        var reviewerIdsForPage = await groupedQuery
            .OrderBy(g => g.Key)
            .Skip((request.Page - 1) * request.PageSize)
            .Take(request.PageSize)
            .Select(g => g.Key)
            .ToListAsync(cancellationToken);

        var rawReviews = await query
            .Where(r => reviewerIdsForPage.Contains(r.ReviewerId))
            .OrderByDescending(r => r.CreatedAtUtc)
            .ToListAsync(cancellationToken);

        var businessDetails = await _context.BusinessProfiles
            .Where(b => reviewerIdsForPage.Contains(b.UserId))
            .Select(b => new { b.UserId, b.BusinessName })
            .ToListAsync(cancellationToken);

        var userDetails = await _context.Users
            .Where(u => reviewerIdsForPage.Contains(u.Id))
            .Select(u => new { u.Id, u.ProfileImage })
            .ToListAsync(cancellationToken);

        var items = rawReviews
            .GroupBy(r => r.ReviewerId)
            .Select(g => new GroupedReviewResponse
            {
                ReviewerId = g.Key,
                ReviewerName = businessDetails.FirstOrDefault(b => b.UserId == g.Key)?.BusinessName ?? string.Empty,
                ReviewerLogo = userDetails.FirstOrDefault(u => u.Id == g.Key)?.ProfileImage,
                ReviewCount = g.Count(),
                Reviews = g.Select(r => new ReviewResponse
                {
                    Id = r.Id,
                    Rating = r.Rating,
                    Comment = r.Comment,
                    ReviewerName = businessDetails.FirstOrDefault(b => b.UserId == g.Key)?.BusinessName ?? string.Empty,
                    ReviewerLogo = userDetails.FirstOrDefault(u => u.Id == g.Key)?.ProfileImage,
                    Type = r.Type.ToString(),
                    CreatedAt = r.CreatedAtUtc
                }).ToList()
            })
            .ToList();

        return new PagedResponse<GroupedReviewResponse>
        {
            Items = items,
            Page = request.Page,
            PageSize = request.PageSize,
            TotalCount = totalCount,
            TotalPages = (int)Math.Ceiling(totalCount / (double)request.PageSize)
        };
    }

    public async Task<RatingSummaryResponse?> GetBusinessRatingSummaryAsync(
        Guid businessId,
        CancellationToken cancellationToken = default)
    {
        return await _context.BusinessProfiles
            .AsNoTracking()
            .Where(b => b.Id == businessId)
            .Select(b => new RatingSummaryResponse
            {
                AverageRating = b.AverageRating,
                TotalRatings = b.RatingCount
            })
            .FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<RatingSummaryResponse?> GetCharityRatingSummaryAsync(
        Guid charityId,
        CancellationToken cancellationToken = default)
    {
        return await _context.CharityProfiles
            .AsNoTracking()
            .Where(c => c.Id == charityId)
            .Select(c => new RatingSummaryResponse
            {
                AverageRating = c.AverageRating,
                TotalRatings = c.RatingCount
            })
            .FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<PagedResponse<GroupedReviewResponse>> GetMyBusinessReviewsAsync(
        Guid userId,
        PaginationRequest request,
        CancellationToken cancellationToken = default)
    {
        var business = await _context.BusinessProfiles
            .AsNoTracking()
            .FirstOrDefaultAsync(b => b.UserId == userId, cancellationToken);

        if (business is null)
            throw new DomainException("Business profile not found.");

        return await GetBusinessReviewsAsync(
            business.Id,
            request,
            cancellationToken);
    }

    public async Task<PagedResponse<GroupedReviewResponse>> GetMyCharityReviewsAsync(
        Guid userId,
        PaginationRequest request,
        CancellationToken cancellationToken = default)
    {
        var charity = await _context.CharityProfiles
            .AsNoTracking()
            .FirstOrDefaultAsync(c => c.UserId == userId, cancellationToken);

        if (charity is null)
            throw new DomainException("Charity profile not found.");

        return await GetCharityReviewsAsync(
            charity.Id,
            request,
            cancellationToken);
    }
}
