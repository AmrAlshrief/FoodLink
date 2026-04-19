using FoodLink.Application.Features.Authentication.DTOs;

namespace FoodLink.Application.Features.Authentication.Services;

public interface IAuthenticationService
{
    Task<AuthenticationResponse> RegisterBusinessAsync(RegisterBusinessRequest request, CancellationToken cancellationToken = default);
    Task<AuthenticationResponse> RegisterCharityAsync(RegisterCharityRequest request, CancellationToken cancellationToken = default);
    Task<AuthenticationResponse> LoginAsync(LoginRequest request, CancellationToken cancellationToken = default);
    Task<List<UserResponse>> GetAllUsersAsync(CancellationToken cancellationToken = default);
}
