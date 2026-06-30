using FoodLink.Application.Common.Models.Pagination;
using FoodLink.Domain.Enums;

namespace FoodLink.Application.Features.Donations.Dtos;
public class CreateDonationRequest
{
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    public DateTimeOffset? ExpiryDate { get; set; }
    public Stream? Image { get; set; }
    public string? ImageFileName { get; set; }
}

public class DonationFilterRequest : PaginationRequest
{
    public DonationScope Scope { get; set; } = DonationScope.Current;

    public string? Status { get; set; }

    public string? Search { get; set; }

    public string? SortBy { get; set; }

    public string? SortDirection { get; set; } = "desc";
}

