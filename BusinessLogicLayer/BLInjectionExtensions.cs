using BusinessLogicLayer.Interfaces;
using BusinessLogicLayer.Services;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class BLInjectionExtensions
	{
		public static IServiceCollection AddBLInjectionExtensions(this IServiceCollection services)
		{
			// SERVICES 
			services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<IEncryptionService, EncryptionService>();
            services.AddScoped<INotificationService, NotificationService>();

            return services;
		}
	}
}
