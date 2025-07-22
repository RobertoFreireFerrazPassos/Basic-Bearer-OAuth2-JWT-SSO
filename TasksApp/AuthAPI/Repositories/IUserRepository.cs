namespace AuthAPI.Repositories;

public interface IUserRepository
{
    UserEntity GetUser(string userName);

    void UpdateRefreshToken(string userName, string refreshToken);
}
