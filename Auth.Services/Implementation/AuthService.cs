using Auth.Models.Dtos;
using Auth.Services.Interface;
using Microsoft.AspNetCore.Identity;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Configuration;

namespace Auth.Services.Implementation
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IConfiguration _configuration;

        public AuthService(UserManager<IdentityUser> userManager, IConfiguration configuration)
        {
            _userManager = userManager;
            _configuration = configuration;
        }

        public async Task<IEnumerable<IdentityError>> RegisterUser(UserRegisterRequestDto request)
        {
            var user = new IdentityUser
            {
                UserName = request.Username,
                Email = request.Email
            };

            var result = await _userManager.CreateAsync(user, request.Password);
            return result.Errors;
        }

        public async Task<IdentityUser> LoginUser(UserLoginRequestDto request)
        {
            var user = await _userManager.FindByNameAsync(request.UsernameOrEmail);
            if (user is null)
            {
                user = await _userManager.FindByEmailAsync(request.UsernameOrEmail);
            }

            if (user is null)
            {
                throw new Exception("Username or password was wrong");
            }

            var result = await _userManager.CheckPasswordAsync(user, request.Password);
            if (!result)
            {
                throw new Exception("Username or password was wrong");
            }

            return user;
        }

        public async Task<string> GenerateJwtToken(IdentityUser user, DateTime expiresAt)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Email, user.Email!),
                new Claim(ClaimTypes.Name, user.UserName!)
            };

            var signingCredentials = new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration.GetSection("Jwt:Key").Value!)), SecurityAlgorithms.HmacSha512Signature);

            SecurityToken securityToken = new JwtSecurityToken(
                claims: claims,
                expires: expiresAt,
                issuer: _configuration.GetSection("Jwt:Issuer").Value,
                audience: _configuration.GetSection("Jwt:Audience").Value,
                signingCredentials: signingCredentials
                );

            string tokenString = new JwtSecurityTokenHandler().WriteToken(securityToken);
            return tokenString;
        }
    }
}
