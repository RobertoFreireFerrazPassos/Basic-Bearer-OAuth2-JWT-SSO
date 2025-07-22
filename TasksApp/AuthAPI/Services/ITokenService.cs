namespace AuthAPI.Services;

public interface ITokenService
{
    string GenerateJwtToken(UserEntity user);
    string GenerateRefreshToken(string userName);
    (string userName, DateTime utcnow) GetUserNameFromRefreshToken(string refreshToken);
}
