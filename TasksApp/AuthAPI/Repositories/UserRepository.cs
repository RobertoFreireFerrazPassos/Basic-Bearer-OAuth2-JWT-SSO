namespace AuthAPI.Repositories;

public class UserRepository : IUserRepository
{
    public Dictionary<string, UserEntity> UserDatabase = new Dictionary<string, UserEntity>()
    {
        {
            "adminuser",
            new UserEntity()
            {
                Id = Guid.Parse("53f5e9c0-7b84-4ced-a9b0-861bab2539a1"),
                Username = "adminuser",
                Role = "Admin",
                HashPassword = "password",
                RefreshToken = string.Empty,
            }
        },
        {
            "basicuser",
            new UserEntity()
            {
                Id = Guid.Parse("98649599-d0b2-42a7-80cc-79d8dd8ba2a7"),
                Username = "basicuser",
                Role = "Basic",
                HashPassword = "password",
                RefreshToken = string.Empty,
            }
        },
    };

    public UserEntity GetUser(string userName)
    {
        var userFromDb = UserDatabase.FirstOrDefault(userDb => userDb.Value.Username == userName).Value;

        if (userFromDb is null)
        {
            throw new Exception("User doesn't exist");
        }

        return userFromDb;
    }

    public void UpdateRefreshToken(string userName, string refreshToken)
    {
        var userFromDb = GetUser(userName);
        userFromDb.RefreshToken = refreshToken;
    }
}
