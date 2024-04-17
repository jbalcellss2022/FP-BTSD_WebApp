using BusinessLogicLayer.Interfaces;
using BusinessLogicLayer.Services;
using DataAccessLayer.Classes;
using DataAccessLayer.Contracts;
using DataAccessLayer.Models;
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
				.Returns(new appUser { login = "test@qrfy.es", password = "$2a$11$CSNAnu2ZWlYqnHstR5SWA.snlXhwTpsmUWk/EopLvfPsDsxL/Cg0G" });
			var mockAuthService = new Mock<IAuthService>();
			mockAuthService.Setup(service => service.CheckUserAuth(It.IsAny<LoginUserDTO>())).Returns(true);
			var service = new AuthService(mockUserRepository.Object);
			bool result = service.CheckUserAuth(new LoginUserDTO() { Username = "test@qrfy.es", Password = "fakepassword" });

			Assert.That(result, Is.False);
        }

		[Test()]
		public void CheckUserAuthTest_OK()
		{
			var mockUserRepository = new Mock<IUserRepository>();
			mockUserRepository.Setup(repo => repo.GetUserByEmail("test@qrfy.es"))
				.Returns(new appUser { login = "test@qrfy.es", password = "$2a$11$CSNAnu2ZWlYqnHstR5SWA.snlXhwTpsmUWk/EopLvfPsDsxL/Cg0G" });
			var mockAuthService = new Mock<IAuthService>();
			mockAuthService.Setup(service => service.CheckUserAuth(It.IsAny<LoginUserDTO>())).Returns(true);
			var service = new AuthService(mockUserRepository.Object);
			bool result = service.CheckUserAuth(new LoginUserDTO() { Username = "test@qrfy.es", Password = "*qrfydemo2024" });

			Assert.That(result, Is.True);
		}
	}
}