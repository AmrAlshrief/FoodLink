using BCrypt.Net;
using FoodLink.Application.Common.Interfaces.Services;

namespace FoodLink.Infrastructure.Services.Authentication;

public class PasswordHasher : IPasswordHasher
{
    public string Hash(string password) 
        => BCrypt.Net.BCrypt.HashPassword(password);

    public bool Verify(string password, string hash) 
        => BCrypt.Net.BCrypt.Verify(password, hash);
}