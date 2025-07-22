namespace AuthAPI.Services;

public interface IAuthService
{
    TokensDto Authenticate(string username, string password);

    TokensDto GetTokenByGetRefreshtoken(string refreshToken);

    void SaveRefreshToken(string userName, string refreshToken);

    void CleanRefreshToken(string userName);
}