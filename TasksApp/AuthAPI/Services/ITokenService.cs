namespace AuthAPI.Services;

public interface ITokenService
{
    string GenerateJwtToken(UserDto user);
    string GenerateRefreshToken(string userName);
    (string userName, DateTime utcnow) GetUserNameFromRefreshToken(string refreshToken);
}
