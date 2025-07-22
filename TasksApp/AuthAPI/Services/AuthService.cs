namespace AuthAPI.Services;

public class AuthService(IUserRepository userRepository, ITokenService tokenService) : IAuthService
{
    public TokensDto Authenticate(string userName, string password)
    {
        var userFromDb = userRepository.GetUser(userName);

        if (userFromDb.HashPassword != password)
        {
            throw new Exception("Invalid credentials.");
        }

        return GenerateTokens(userFromDb);
    }

    public TokensDto GetTokenByGetRefreshtoken(string refreshToken)
    {
        var (userName, datetimeToken) = tokenService.GetUserNameFromRefreshToken(refreshToken);

        if (string.IsNullOrWhiteSpace(userName))
        {
            throw new Exception("Invalid user name.");
        }

        if (DateTime.UtcNow > datetimeToken)
        {
            throw new Exception("Refresh token expired.");
        }

        var userFromDb = userRepository.GetUser(userName);

        if (string.IsNullOrWhiteSpace(userFromDb?.RefreshToken) || refreshToken != userFromDb.RefreshToken)
        {
            throw new Exception("Invalid refresh token.");
        }

        return GenerateTokens(userFromDb);
    }

    private TokensDto GenerateTokens(UserEntity user)
    {
        var token = tokenService.GenerateJwtToken(user);
        var refreshToken = tokenService.GenerateRefreshToken(user.Username);
        SaveRefreshToken(user.Username, refreshToken);

        return new TokensDto()
        {
            Token = token,
            RefreshToken = refreshToken
        };
    }

    public void SaveRefreshToken(string userName, string refreshToken)
    {
        userRepository.UpdateRefreshToken(userName, refreshToken);
    }

    public void CleanRefreshToken(string userName)
    {
        userRepository.UpdateRefreshToken(userName, string.Empty);
    }
}