using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace BusinessLogicLayer
{
	public static class BLServiceCollection
	{
		public static IServiceCollection GetServiceCollection(IServiceCollection? services = null)
		{
			services ??= new ServiceCollection();

			//SERVICES 
			services.AddBLInjectionExtensions();

			//REPOSITORIES
			services.AddDALInjectionExtensions();

			return services;
		}
	}
}
