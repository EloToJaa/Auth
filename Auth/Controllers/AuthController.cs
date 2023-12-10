using Auth.Models.Dtos;
using Auth.Services.Interface;
using Microsoft.AspNetCore.Mvc;

namespace Auth.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly IAuthService _authService;

        public AuthController(IConfiguration configuration, IAuthService authService)
        {
            _configuration = configuration;
            _authService = authService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] UserRegisterRequestDto request)
        {
            var errors = await _authService.RegisterUser(request);
            if (errors.Any())
            {
                return BadRequest(errors);
            }
            return Ok("User registered");
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] UserLoginRequestDto request)
        {
            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                var user = await _authService.LoginUser(request);

                var refreshTokenExpiresAt = DateTime.UtcNow.AddDays(7);
                var accessTokenExpiresAt = DateTime.UtcNow.AddMinutes(5);

                var refreshToken = await _authService.GenerateJwtToken(user, refreshTokenExpiresAt);
                var accessToken = await _authService.GenerateJwtToken(user, accessTokenExpiresAt);

                Response.Cookies.Append("refreshToken", refreshToken, new CookieOptions
                {
                    HttpOnly = true,
                    Secure = true,
                    Expires = refreshTokenExpiresAt
                });

                return Ok(new
                {
                    AccessToken = accessToken,
                    Message = "Login successful"
                });
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
    }
}
