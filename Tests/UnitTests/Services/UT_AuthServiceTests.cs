using BusinessLogicLayer.Interfaces;
using BusinessLogicLayer.Services;
using DataAccessLayer.Interfaces;
using Entities.DTOs;
using Entities.Models;
using Moq;

namespace Tests.UnitTests.Services
{
    [TestFixture()]
    public class UT_AuthServiceTests
    {
        [Test()]
        public void CheckUserAuthTest_KO()
        {
			var mockUserRepository = new Mock<IUserRepository>();
			mockUserRepository.Setup(repo => repo.GetUserByEmail("test@qrfy.es"))
				.Returns(new AppUser { Login = "test@qrfy.es", Password = "$2a$11$CSNAnu2ZWlYqnHstR5SWA.snlXhwTpsmUWk/EopLvfPsDsxL/Cg0G" });

            var mockEncryptionService = new Mock<IEncryptionService>();
            mockEncryptionService.Setup(enc => enc.CheckBCryptPassword(It.IsAny<string>(), It.IsAny<string>())).Returns(true);
            var mockAuthService = new Mock<IAuthService>();
            mockAuthService.Setup(service => service.CheckUserAuth(It.IsAny<LoginUserDTO>())).Returns(true);
            var service = new AuthService(mockUserRepository.Object, mockEncryptionService.Object);
            var result = service.CheckUserAuth(new LoginUserDTO() { Username = "test@qrfy.es", Password = "fakepassword" });

			Assert.That(result, Is.False);
        }

		[Test()]
		public void CheckUserAuthTest_OK()
		{
			var mockUserRepository = new Mock<IUserRepository>();
			mockUserRepository.Setup(repo => repo.GetUserByEmail("test@qrfy.es"))
				.Returns(new AppUser { Login = "test@qrfy.es", Password = "$2a$11$CSNAnu2ZWlYqnHstR5SWA.snlXhwTpsmUWk/EopLvfPsDsxL/Cg0G" });

            var mockEncryptionService = new Mock<IEncryptionService>();
            mockEncryptionService.Setup(enc => enc.CheckBCryptPassword(It.IsAny<string>(), It.IsAny<string>())).Returns(true);
            var mockAuthService = new Mock<IAuthService>();
            mockAuthService.Setup(service => service.CheckUserAuth(It.IsAny<LoginUserDTO>())).Returns(true);
			var service = new AuthService(mockUserRepository.Object, mockEncryptionService.Object);
			bool result = service.CheckUserAuth(new LoginUserDTO() { Username = "test@qrfy.es", Password = "*qrfydemo2024" });

			Assert.That(result, Is.True);
		}
    }
}