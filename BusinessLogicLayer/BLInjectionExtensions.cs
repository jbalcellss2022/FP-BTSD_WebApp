using BusinessLogicLayer.Interfaces;
using BusinessLogicLayer.Services;
using Microsoft.Extensions.Configuration;

namespace Microsoft.Extensions.DependencyInjection
{
	public static class BLInjectionExtensions
	{
		public static IServiceCollection AddBLInjectionExtensions(this IServiceCollection services, IConfigurationRoot configuration)
		{
			// SERVICES 
			services.AddScoped<IAuthService, AuthService>();
            //services.AddScoped<IUserDDService, UserDDService>();

            return services;
		}
	}
}
