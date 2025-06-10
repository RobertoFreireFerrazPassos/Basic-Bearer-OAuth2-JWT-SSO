namespace AuthAPI.Services;

public class UserService : IUserService
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

    public UserDto Authenticate(string userName, string password)
    {
        var userFromDb = UserDatabase.FirstOrDefault(userDb => userDb.Value.Username == userName).Value;

        if (userFromDb is null || userFromDb.HashPassword != password)
        {
            return null;
        }

        return new UserDto()
        {
            Username = userFromDb.Username,
            Role = userFromDb.Role,
        };
    }

    public UserDto GetUser(string userName)
    {
        var userFromDb = UserDatabase.FirstOrDefault(userDb => userDb.Value.Username == userName).Value;

        if (userFromDb is null)
        {
            return null;
        }

        return new UserDto()
        {
            Username = userFromDb.Username,
            Role = userFromDb.Role,
        };
    }

    public void SaveRefreshToken(string userName, string refreshToken)
    {
        var userFromDb = UserDatabase.FirstOrDefault(userDb => userDb.Value.Username == userName).Value;

        if (userFromDb is null)
        {
            throw new Exception("User doesn't exist");
        }

        userFromDb.RefreshToken = refreshToken;
    }

    public string GetRefreshToken(string userName, DateTime datetimeToken)
    {
        var utcNow = DateTime.UtcNow;

        if (utcNow > datetimeToken)
        {
            throw new Exception("Refresh Token expired");
        }

        var userFromDb = UserDatabase.FirstOrDefault(userDb => userDb.Value.Username == userName).Value;

        if (userFromDb is null)
        {
            throw new Exception("User doesn't exist");
        }

        return userFromDb.RefreshToken;
    }

    public void CleanRefreshToken(string userName)
    {
        var userFromDb = UserDatabase.FirstOrDefault(userDb => userDb.Value.Username == userName).Value;

        if (userFromDb is null)
        {
            throw new Exception("User doesn't exist");
        }

        userFromDb.RefreshToken = string.Empty;
    }
}