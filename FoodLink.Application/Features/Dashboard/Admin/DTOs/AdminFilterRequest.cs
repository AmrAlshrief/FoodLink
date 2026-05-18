using FoodLink.Application.Common.Models.Pagination;

namespace FoodLink.Application.Features.Dashboard.Admin.DTOs;

public class AdminFilterRequest : PaginationRequest
{
    public string? Search { get; set; }

    public bool? IsSuspended { get; set; }
}
