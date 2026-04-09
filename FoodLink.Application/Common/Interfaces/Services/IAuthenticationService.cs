using FoodLink.Application.Features.Authentication.DTOs;

namespace FoodLink.Application.Features.Authentication.Services;

public interface IAuthenticationService
{
    Task<AuthenticationResponse> RegisterAsync(RegisterRequest request, CancellationToken cancellationToken = default);
    Task<AuthenticationResponse> LoginAsync(LoginRequest request, CancellationToken cancellationToken = default);
}
