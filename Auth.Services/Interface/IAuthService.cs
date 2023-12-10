using Auth.Models.Dtos;
using Microsoft.AspNetCore.Identity;

namespace Auth.Services.Interface
{
    public interface IAuthService
    {
        Task<string> GenerateJwtToken(IdentityUser user, DateTime expiresAt);
        Task<IdentityUser> LoginUser(UserLoginRequestDto request);
        Task<IEnumerable<IdentityError>> RegisterUser(UserRegisterRequestDto request);
    }
}