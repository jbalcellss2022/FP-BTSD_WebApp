using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace BusinessLogicLayer
{
	public static class BLServiceCollection
	{
		public static IServiceCollection GetServiceCollection(string entityConnString, IServiceCollection services = null, IConfigurationRoot configuration = null)
		{
			if (services == null)
				services = new ServiceCollection();

			//SERVICES 
			services.AddBLInjectionExtensions(configuration);

			//REPOSITORIES
			services.AddDALInjectionExtensions(configuration);

			return services;
		}
	}
}
