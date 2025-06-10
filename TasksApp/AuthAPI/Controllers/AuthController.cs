namespace AuthAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AuthController(
        IUserService _userService,
        ITokenService _tokenService
    ) : Controller
{

    [HttpPost("logout")]
    public async Task<IActionResult> Logout([FromBody] LogoutRequest model)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(model.Username))
            {
                return BadRequest("Invalid logout request.");
            }

            _userService.CleanRefreshToken(model.Username);
            return Ok();
        }
        catch (Exception ex)
        {
            // Log exception
            return StatusCode(StatusCodes.Status500InternalServerError, null);
        }
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequest model)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(model.Username) || string.IsNullOrWhiteSpace(model.Password))
            {
                return BadRequest("Invalid login request.");
            }

            var user = _userService.Authenticate(model.Username, model.Password);

            if (user == null)
            {
                return Unauthorized("Invalid credentials.");
            }

            var token = _tokenService.GenerateJwtToken(user);
            var refreshToken = _tokenService.GenerateRefreshToken(user.Username);
            _userService.SaveRefreshToken(user.Username, refreshToken);

            return Ok(new { Token = token, RefreshToken = refreshToken });
        }
        catch (Exception ex)
        {
            // Log exception
            return StatusCode(StatusCodes.Status500InternalServerError, null);
        }
    }

    [HttpPost("refreshtoken")]
    public async Task<IActionResult> GetAccessToken([FromBody] AccessTokenRequest accessTokenRequest)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(accessTokenRequest?.RefreshToken))
            {
                return BadRequest("Invalid");
            }

            var (userName, datetimeToken) = _tokenService.GetUserNameFromRefreshToken(accessTokenRequest?.RefreshToken);

            if (string.IsNullOrWhiteSpace(userName))
            {
                return BadRequest("Invalid");
            }

            var refreshTokenFromDb = _userService.GetRefreshToken(userName, datetimeToken);

            if (string.IsNullOrWhiteSpace(refreshTokenFromDb) || accessTokenRequest.RefreshToken != refreshTokenFromDb)
            {
                return BadRequest("Invalid");
            }

            var user = _userService.GetUser(userName);

            if (user == null)
            {
                return BadRequest("Invalid");
            }

            var newToken = _tokenService.GenerateJwtToken(user);

            return Ok(new { Token = newToken });
        }
        catch (Exception ex)
        {
            // Log exception
            return StatusCode(StatusCodes.Status500InternalServerError, null);
        }
    }
}