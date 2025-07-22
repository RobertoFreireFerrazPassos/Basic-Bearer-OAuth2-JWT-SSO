namespace AuthAPI.DataContracts;

public class AccessTokenRequest
{
    public string RefreshToken { get; set; } = string.Empty;
}
