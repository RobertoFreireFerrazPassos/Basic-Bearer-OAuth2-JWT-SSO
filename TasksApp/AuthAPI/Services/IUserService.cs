namespace AuthAPI.Services;

public interface IUserService
{
    UserDto Authenticate(string username, string password);

    UserDto GetUser(string userName);

    void SaveRefreshToken(string userName, string refreshToken);

    string GetRefreshToken(string userName, DateTime datetimeToke);

    void CleanRefreshToken(string userName);
}