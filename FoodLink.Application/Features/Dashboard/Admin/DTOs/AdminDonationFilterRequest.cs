using FoodLink.Application.Common.Models.Pagination;

namespace FoodLink.Application.Features.Dashboard.Admin.DTOs;

public class AdminDonationFilterRequest : PaginationRequest
{
    public string? Search { get; set; }
    public string? Status { get; set; }
    public string? SortBy { get; set; }
    public string? SortDirection { get; set; } = "desc";
}
