using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using FoodLink.Application.Common.Interfaces.Services;
using FoodLink.Domain.Entities;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace FoodLink.Infrastructure.Identity;

public class JwtTokenGenerator(IConfiguration configuration) : IJwtTokenGenerator
{
    public string GenerateToken(User user, Guid? businessProfileId = null, Guid? charityProfileId = null)
    {
        var jwtSettings = configuration.GetSection("JwtSettings");

        var secret = jwtSettings["Secret"]
            ?? throw new InvalidOperationException("JWT Secret is missing");

        var issuer = jwtSettings["Issuer"];
        var audience = jwtSettings["Audience"];

        var expiryMinutes = int.TryParse(jwtSettings["ExpiryMinutes"], out var minutes)
            ? minutes
            : 60;

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret));

        var claims = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
            new(JwtRegisteredClaimNames.Email, user.Email),
            new(ClaimTypes.Name, user.Name),
            new(ClaimTypes.Role, user.Role.ToString()),
            new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };

        if (businessProfileId.HasValue)
            claims.Add(new Claim("businessProfileId", businessProfileId.Value.ToString()));

        if (charityProfileId.HasValue)
            claims.Add(new Claim("charityProfileId", charityProfileId.Value.ToString()));

        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.UtcNow.AddMinutes(expiryMinutes),
            Issuer = issuer,
            Audience = audience,
            SigningCredentials = creds
        };

        var tokenHandler = new JwtSecurityTokenHandler();
        var token = tokenHandler.CreateToken(tokenDescriptor);

        return tokenHandler.WriteToken(token);
    }
}