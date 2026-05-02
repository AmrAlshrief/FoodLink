using FoodLink.Application.Features.Reservation.Dtos;

namespace FoodLink.Application.Common.Interfaces.Services.Queries;

public interface IReservationQueries
{
    Task<List<ReservationResponse>> GetMyReservationsAsync(Guid charityId, CancellationToken cancellationToken = default);
    Task<List<ReservationResponse>> GetReservationsAsync(Guid charityId, ReservationFilterRequest filter, CancellationToken cancellationToken = default);
    Task<List<ReservationResponse>> GetReservationsByDonationAsync(Guid businessId, Guid donationId, ReservationFilterRequest filter, CancellationToken cancellationToken = default);
}