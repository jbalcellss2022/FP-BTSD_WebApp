using DataAccessLayer.Interfaces;
using DataAccessLayer.Repositories;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class DALInjectionExtensions
	{

		public static IServiceCollection AddDALInjectionExtensions(this IServiceCollection services)
		{
			// REPOSITORIES
			services.AddScoped<IUserRepository, UserRepository>();

			return services;
		}
	}
}
