using Entities.DTOs;
using NUnit.Framework;
using System.IdentityModel.Tokens.Jwt;
using System.Threading.Tasks;

namespace AuthServiceIntegrationTests
{
    public class AuthServiceAuthTests : BaseAuthServiceTests
    {
        [Test]
        public async Task CheckUserAuth_WithValidAuthToken_ReturnsTrue()
        {
            // Arrange
            var loginUserDTO = new LoginUserDTO { AuthToken = "valid_token" };
            var jwtToken = new JwtSecurityToken();
            // Setup your token and repository as needed (for example, you might need to add some setup in your services)

            // Act
            var result = await AuthService.CheckUserAuth(loginUserDTO);

            // Assert
            Assert.That(result, Is.True);
        }

        [Test]
        public async Task CheckUserAuth_WithInvalidAuthToken_ReturnsFalse()
        {
            // Arrange
            var loginUserDTO = new LoginUserDTO { AuthToken = "invalid_token" };

            // Act
            var result = await AuthService.CheckUserAuth(loginUserDTO);

            // Assert
            Assert.That(result, Is.False);
        }
    }
}
