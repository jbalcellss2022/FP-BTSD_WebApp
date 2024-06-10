using BusinessLogicLayer.Interfaces;
using BusinessLogicLayer.Services;
using BusinessLogicLayer.Helpers;
using DataAccessLayer.Interfaces;
using DataAccessLayer.Repositories;
using Entities.Models;
using Microsoft.Extensions.DependencyInjection;

namespace AuthServiceIntegrationTests
{
    public abstract class BaseAuthServiceTests
    {
        protected ServiceProvider? ServiceProvider;
        protected IAuthService AuthService;
        protected IUserRepository? UserRepository;

        [SetUp]
        public void BaseSetUp()
        {
            var serviceCollection = new ServiceCollection();

            // Register your services and repositories here
            serviceCollection.AddSingleton<IUserRepository, UserRepository>(); 
            serviceCollection.AddSingleton<IEncryptionService, EncryptionService>();
            serviceCollection.AddSingleton<IUserDDService, UserDDService>();
            serviceCollection.AddSingleton<INotificationService, NotificationService>();
            serviceCollection.AddSingleton<IHelpersService, EmailBodyHelper>();
            serviceCollection.AddSingleton<IAuthService, AuthService>();

            ServiceProvider = serviceCollection.BuildServiceProvider();
            AuthService = ServiceProvider.GetService<IAuthService>()!;
            UserRepository = ServiceProvider.GetService<IUserRepository>()!;

            // Seed initial data
            SeedData();
        }

        [TearDown]
        public void BaseTearDown()
        {
            ServiceProvider?.Dispose();
        }

        private void SeedData()
        {
            var user = new AppUser
            {
                Login = "user@example.com",
                Password = "hashed_password",
                IsBlocked = false,
                Retries = 0
            };
            UserRepository!.CreateAccount(user.Login, user.Name, user.Password).Wait();
        }
    }
}
