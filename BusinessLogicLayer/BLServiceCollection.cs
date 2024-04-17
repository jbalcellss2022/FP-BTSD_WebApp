using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace BusinessLogicLayer
{
	public static class BLServiceCollection
	{
		public static IServiceCollection GetServiceCollection(IServiceCollection? services = null, IConfigurationRoot? configuration = null)
		{
			services ??= new ServiceCollection();

			//SERVICES 
			services.AddBLInjectionExtensions(configuration!);

			//REPOSITORIES
			services.AddDALInjectionExtensions(configuration!);

			return services;
		}
	}
}
