namespace AuthAPI.Entities;

public class UserEntity
{
    public Guid Id { get; set; }
    public string Username { get; set; } = string.Empty;
    public string Role { get; set; } = string.Empty;
    public string HashPassword { get; set; } = string.Empty;
    public string RefreshToken { get; set; } = string.Empty;
}
