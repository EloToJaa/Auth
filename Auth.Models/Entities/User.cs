using Microsoft.AspNetCore.Identity;

namespace Auth.Models.Entities
{
    public class User : IdentityUser
    {
        public string TwoFactorSecretKey { get; set; } = string.Empty;
    }
}
