namespace AuthAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AuthController(
        IAuthService _authService
    ) : Controller
{

    [HttpPost("logout")]
    public async Task<IActionResult> Logout([FromBody] LogoutRequest model)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(model.Username))
            {
                return BadRequest("Invalid username");
            }

            _authService.CleanRefreshToken(model.Username);
            return Ok();
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
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

            return Ok(_authService.Authenticate(model.Username, model.Password));
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPost("refreshtoken")]
    public async Task<IActionResult> GetAccessToken([FromBody] AccessTokenRequest accessTokenRequest)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(accessTokenRequest?.RefreshToken))
            {
                return BadRequest("Invalid refresh token.");
            }

            return Ok(_authService.GetTokenByGetRefreshtoken(accessTokenRequest.RefreshToken));         
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
}