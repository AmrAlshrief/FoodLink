using FoodLink.Application.Common.Models.Pagination;
using FoodLink.Application.Features.Reservation.Dtos;

namespace FoodLink.Application.Features.Reservation.Interfaces;

public interface IReservationQueries
{
    Task<List<ReservationResponse>> GetMyReservationsAsync(Guid charityId, CancellationToken cancellationToken = default);
    Task<PagedResponse<ReservationResponse>> GetReservationsAsync(Guid charityId, ReservationFilterRequest filter, CancellationToken cancellationToken = default);
    Task<PagedResponse<ReservationResponse>> GetReservationsByDonationAsync(Guid? businessId, Guid donationId, ReservationFilterRequest filter, CancellationToken cancellationToken = default);
}