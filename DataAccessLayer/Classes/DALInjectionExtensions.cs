using DataAccessLayer.Interfaces;
using DataAccessLayer.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace DataAccessLayer.Classes
{
    public static class DALInjectionExtensions
    {

        public static IServiceCollection AddDALInjectionExtensions(this IServiceCollection services)
        {
            // REPOSITORIES
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IChatRepository, ChatRepository>();
            services.AddScoped<IBarcodeRepository, BarcodeRepository>();

            return services;
        }
    }
}
