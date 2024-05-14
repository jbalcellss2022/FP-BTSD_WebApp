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
            services.AddScoped<IChatService, ChatService>();
            services.AddScoped<IPromptService, PromptService>();
            services.AddScoped<IProfileService, ProfileService>();
            services.AddScoped<IClaimsService, ClaimsService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IBarcodeService, BarcodeService>();

            return services;
        }
    }
}
