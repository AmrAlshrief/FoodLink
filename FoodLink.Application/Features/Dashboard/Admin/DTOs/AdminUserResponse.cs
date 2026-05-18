namespace FoodLink.Application.Features.Dashboard.Admin.DTOs;

public class AdminUserResponse
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }
    public string Role { get; set; }
    public string Phone { get; set; }
    public string? ProfileImage { get; set; }
    public bool IsSuspended { get; set; }
    public DateTime CreatedAt { get; set; }
}