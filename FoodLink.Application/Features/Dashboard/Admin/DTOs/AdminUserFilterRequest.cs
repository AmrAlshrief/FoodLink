using FoodLink.Application.Common.Models.Pagination;
using FoodLink.Domain.Enums;

namespace FoodLink.Application.Features.Dashboard.Admin.DTOs;

public class AdminUserFilterRequest : PaginationRequest
{
    public string? Search { get; set; }
    public UserRole? Role { get; set; }
    public bool? IsSuspended { get; set; }
}