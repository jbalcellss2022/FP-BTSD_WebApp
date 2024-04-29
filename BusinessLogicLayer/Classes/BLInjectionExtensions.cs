using BusinessLogicLayer.Helpers;
using BusinessLogicLayer.Interfaces;
using BusinessLogicLayer.Services;
using Microsoft.Extensions.DependencyInjection;

namespace BusinessLogicLayer.Classes
{
    public static class BLInjectionExtensions
    {
        public static IServiceCollection AddBLInjectionExtensions(this IServiceCollection services)
        {
            // SERVICES 
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<IEncryptionService, EncryptionService>();
            services.AddScoped<INotificationService, NotificationService>();
            services.AddScoped<IHelpersService, EmailBodyHelper>();
            services.AddScoped<IPromptService, PromptService>();

            return services;
        }
    }
}
