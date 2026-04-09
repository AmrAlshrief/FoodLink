using FoodLink.Application.Common.Interfaces.Repositories;
using FoodLink.Application.Common.Interfaces.Services;
using FoodLink.Application.Features.Authentication.DTOs;
using FoodLink.Domain.Common.Exceptions;
using FoodLink.Domain.Entities;
using FoodLink.Domain.Enums;

namespace FoodLink.Application.Features.Authentication.Services;

public class AuthenticationService(
    IJwtTokenGenerator jwtTokenGenerator,
    IPasswordHasher passwordHasher,
    IUserRepository userRepository,
    IUnitOfWork unitOfWork) : IAuthenticationService
{
    public async Task<AuthenticationResponse> RegisterAsync(RegisterRequest request, CancellationToken cancellationToken = default)
    {
        // 1. Check if user already exists
        if (await userRepository.GetByEmailAsync(request.Email, cancellationToken) is not null)
        {
            throw new DomainException("User with given email already exists");
        }

        // 2. Hash Password
        var passwordHash = passwordHasher.Hash(request.Password);

        // 3. Parse Role (default to User if invalid)
        if (!Enum.TryParse<UserRole>(request.Role, true, out var role))
        {
            role = UserRole.User;
        }

        // 4. Create User entity
        var user = User.Create(
            request.Name, 
            request.Email, 
            passwordHash, 
            role, 
            request.Phone, 
            request.ProfileImage);

        userRepository.Add(user);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        // 5. Generate JWT token
        var token = jwtTokenGenerator.GenerateToken(user);

        return new AuthenticationResponse(
            user.Id,
            user.Name,
            user.Email,
            user.Role.ToString(),
            token);
    }

    public async Task<AuthenticationResponse> LoginAsync(LoginRequest request, CancellationToken cancellationToken = default)
    {
        // 1. Validate the user exists
        if (await userRepository.GetByEmailAsync(request.Email, cancellationToken) is not User user)
        {
            throw new DomainException("Invalid credentials");
        }

        // 2. Validate the password
        if (!passwordHasher.Verify(request.Password, user.PasswordHash))
        {
            throw new DomainException("Invalid credentials");
        }

        // 3. Generate JWT token
        var token = jwtTokenGenerator.GenerateToken(user);

        return new AuthenticationResponse(
            user.Id,
            user.Name,
            user.Email,
            user.Role.ToString(),
            token);
    }
}
