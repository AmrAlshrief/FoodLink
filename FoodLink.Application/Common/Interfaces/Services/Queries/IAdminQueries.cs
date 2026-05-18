using FoodLink.Application.Features.Dashboard.Admin.DTOs;
using FoodLink.Application.Common.Models.Pagination;

namespace FoodLink.Application.Common.Interfaces.Services.Queries;
public interface IAdminQueries
{
    Task<AdminDashboardStatsResponse> GetDashboardStatsAsync(
        CancellationToken cancellationToken = default);

    Task<PagedResponse<AdminCharityResponse>> GetCharitiesAsync(
        AdminFilterRequest request,
        CancellationToken cancellationToken = default);

    Task<AdminCharityResponse?> GetCharityByIdAsync(
        Guid charityId,
        CancellationToken cancellationToken = default);

    Task<PagedResponse<AdminBusinessResponse>> GetBusinessesAsync(
        AdminFilterRequest request,
        CancellationToken cancellationToken = default);

    Task<AdminBusinessResponse?> GetBusinessByIdAsync(
        Guid businessId,
        CancellationToken cancellationToken = default);

    Task<PagedResponse<AdminUserResponse>> GetUsersAsync(
        AdminUserFilterRequest request,
        CancellationToken cancellationToken = default);
}