using Auth.Services.Implementation;
using Auth.Services.Interface;
using Microsoft.Extensions.Configuration;

namespace Auth.Services.Tests.Implementations
{
    internal class AuthServiceTests
    {
        private readonly IAuthService _authService;

        public AuthServiceTests()
        {
            IConfiguration configuration = new ConfigurationBuilder()
                .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
                .AddJsonFile("appsettings.json")
                .Build();

            _authService = new AuthService(configuration);
        }
    }
}
