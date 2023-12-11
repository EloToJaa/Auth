namespace Auth.Models.Dtos
{
    public class UserLoginRequestDto
    {
        public required string UsernameOrEmail { get; set; }
        public required string Password { get; set; }
    }
}
