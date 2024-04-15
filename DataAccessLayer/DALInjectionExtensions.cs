using DataAccessLayer.Contracts;
using DataAccessLayer.Models;
using DataAccessLayer.Repositories;
using Microsoft.Extensions.Configuration;

namespace Microsoft.Extensions.DependencyInjection
{
	public static class DALInjectionExtensions
	{

		public static IServiceCollection AddDALInjectionExtensions(this IServiceCollection services, IConfigurationRoot configuration)
		{
			// REPOSITORIES
			services.AddScoped<IUserRepository, UserRepository>();

			return services;
		}
	}
}
