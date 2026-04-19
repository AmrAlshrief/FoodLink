using FoodLink.Application.Common.Interfaces.Repositories;
using FoodLink.Application.Common.Interfaces;
using FoodLink.Application.Common.Interfaces.Services;
using FoodLink.Application.Features.Authentication.DTOs;
using FoodLink.Domain.Common.Exceptions;
using FoodLink.Domain.Entities;
using FoodLink.Domain.Entities.Profiles;
using FoodLink.Domain.Enums;

namespace FoodLink.Application.Features.Authentication.Services;

public class AuthService(
    IUserRepository userRepository,
    IBusinessProfileRepository businessProfileRepository,
    ICharityProfileRepository charityProfileRepository,
    IPasswordHasher passwordHasher,
    IJwtTokenGenerator jwtTokenGenerator,
    IImageService imageService,
    IUnitOfWork unitOfWork) : IAuthenticationService
{

    public async Task<AuthenticationResponse> RegisterBusinessAsync(
    RegisterBusinessRequest request,
    CancellationToken cancellationToken = default)
    {

        var profileImageUrl = string.Empty;

        if (request.ProfileImage != null)
        {
            profileImageUrl = await imageService.UploadImageAsync(
                request.ProfileImage,
                request.ProfileImageFileName ?? $"{Guid.NewGuid()}.jpg");
        }

        // 1. Normalize email
        var email = request.Email.Trim().ToLower();

        // 2. Check if exists
        if (await userRepository.ExistsByEmailAsync(email, cancellationToken))
            throw new DomainException("Email already exists");

        // 3. Hash password
        var passwordHash = passwordHasher.Hash(request.Password);

        // 4. Create User
        var user = User.Create(
            request.Name,
            email,
            passwordHash,
            UserRole.Business,
            request.Phone
        );

        user.SetProfileImage(profileImageUrl);

        // 5. Create Profile
        var profile = BusinessProfile.Create(
            user.Id,
            request.BusinessName,
            request.Address,
            request.BusinessType
        );

        // 6. Save
        userRepository.Add(user);
        businessProfileRepository.Add(profile);

        await unitOfWork.SaveChangesAsync(cancellationToken);

        // 7. Generate token with business profile ID
        var token = jwtTokenGenerator.GenerateToken(user, businessProfileId: profile.Id);

        return new AuthenticationResponse(
            user.Id,
            user.Name,
            user.Email,
            user.Role.ToString(),
            token);
    }

    public async Task<AuthenticationResponse> RegisterCharityAsync(
    RegisterCharityRequest request,
    CancellationToken cancellationToken = default)
    {
        var profileImageUrl = string.Empty;

        if (request.ProfileImage != null)
        {
            profileImageUrl = await imageService.UploadImageAsync(
                request.ProfileImage,
                request.ProfileImageFileName ?? $"{Guid.NewGuid()}.jpg");
        }

        var email = request.Email.Trim().ToLower();

        if (await userRepository.ExistsByEmailAsync(email, cancellationToken))
            throw new DomainException("Email already exists");

        var passwordHash = passwordHasher.Hash(request.Password);

        var user = User.Create(
            request.Name,
            email,
            passwordHash,
            UserRole.Charity,
            request.Phone
        );

        user.SetProfileImage(profileImageUrl);

        var profile = CharityProfile.Create(
            user.Id,
            request.OrganizationName,
            request.LicenseNumber,
            request.Address
        );

        userRepository.Add(user);
        charityProfileRepository.Add(profile);

        await unitOfWork.SaveChangesAsync(cancellationToken);

        var token = jwtTokenGenerator.GenerateToken(user, charityProfileId: profile.Id);

        return new AuthenticationResponse(
            user.Id,
            user.Name,
            user.Email,
            user.Role.ToString(),
            token);
    }

    public async Task<AuthenticationResponse> LoginAsync(
    LoginRequest request,
    CancellationToken cancellationToken = default)
    {
        var email = request.Email.Trim().ToLower();

        var user = await userRepository.GetByEmailAsync(email, cancellationToken)
            ?? throw new DomainException("Invalid credentials");

        var isValid = passwordHasher.Verify(request.Password, user.PasswordHash);

        if (!isValid)
            throw new DomainException("Invalid credentials");

        // Get the appropriate profile ID based on user role
        Guid? businessProfileId = null;
        Guid? charityProfileId = null;

        if (user.Role == UserRole.Business)
        {
            var businessProfile = await businessProfileRepository.GetByUserIdAsync(user.Id, cancellationToken);
            businessProfileId = businessProfile?.Id;
        }
        else if (user.Role == UserRole.Charity)
        {
            var charityProfile = await charityProfileRepository.GetByUserIdAsync(user.Id, cancellationToken);
            charityProfileId = charityProfile?.Id;
        }

        var token = jwtTokenGenerator.GenerateToken(user, businessProfileId, charityProfileId);

        return new AuthenticationResponse(
            user.Id,
            user.Name,
            user.Email,
            user.Role.ToString(),
            token);
    }

    // public async Task<UserResponse> GetCurrentUserAsync(Guid userId, CancellationToken cancellationToken = default)
    // {
    //     var user = await userRepository.GetByIdAsync(userId, cancellationToken)
    //         ?? throw new DomainException("User not found");

    //     return new UserResponse(
    //         user.Id,
    //         user.Name,
    //         user.Email,
    //         user.Role.ToString(),
    //         user.Phone,
    //         user.ProfileImage);
    // }

    public async Task<List<UserResponse>> GetAllUsersAsync(CancellationToken cancellationToken = default)
    {
        var users = await userRepository.GetAllAsync(cancellationToken);

        return users.Select(user => new UserResponse(
            user.Id,
            user.Name,
            user.Email,
            user.Role.ToString(),
            user.Phone,
            user.ProfileImage)).ToList();
    }


}