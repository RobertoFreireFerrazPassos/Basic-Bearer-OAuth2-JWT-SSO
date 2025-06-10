namespace AuthAPI.Models;

public class AccessTokenRequest
{
    public string RefreshToken { get; set; } = string.Empty;
}
