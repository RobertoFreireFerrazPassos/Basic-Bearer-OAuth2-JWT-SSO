namespace AuthAPI.Services;

public class TokenService(
    IConfiguration _configuration) : ITokenService
{
    private const string _delimiter = "-----";

    public string GenerateJwtToken(UserDto user)
    {
        var claims = new[]
        {
            new Claim(ClaimTypes.NameIdentifier, user.Username),
            new Claim(ClaimTypes.Role, user.Role)
        };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:SecretKey"]));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: _configuration["Jwt:Issuer"],
            audience: _configuration["Jwt:Audience"],
            claims: claims,
            expires: DateTime.UtcNow.AddSeconds(30),
            signingCredentials: creds
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    public string GenerateRefreshToken(string userName)
    {
        byte[] randomBytes = new byte[64];
        using (RandomNumberGenerator rng = RandomNumberGenerator.Create())
        {
            rng.GetBytes(randomBytes);
        }

        var expiredTime = DateTime.UtcNow.AddMinutes(10);

        var newBytes = Encoding.UTF8.GetBytes(userName + _delimiter + expiredTime.ToString() + _delimiter).Concat(randomBytes).ToArray();
        return Convert.ToBase64String(newBytes);
    }

    public (string userName, DateTime utcnow) GetUserNameFromRefreshToken(string refreshToken)
    {
        try
        {
            refreshToken = Encoding.UTF8.GetString(Convert.FromBase64String(refreshToken));
            int separatorIndex = refreshToken.LastIndexOf(_delimiter);

            if (separatorIndex == -1)
            {
                throw new ArgumentException("Invalid refresh token format.");
            }

            var userNamePlusUtcNow = refreshToken.Substring(0, separatorIndex);

            separatorIndex = userNamePlusUtcNow.LastIndexOf(_delimiter);

            if (separatorIndex == -1)
            {
                throw new ArgumentException("Invalid refresh token format.");
            }

            var userName = userNamePlusUtcNow.Substring(0, separatorIndex);
            var UtcNow = userNamePlusUtcNow.Replace(userName + _delimiter, string.Empty);

            return (userName, DateTime.Parse(UtcNow));
        }
        catch (Exception ex)
        {
            // log exception
            throw;
        }
    }
}
