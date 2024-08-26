using Microsoft.Extensions.DependencyInjection;

namespace GestranChecklist.Application
{
    public static class ApplicationModule
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            services
                .AddRepositories()
                .AddServices();

            return services;
        }

        /// <summary>
        /// Registro dos Repositórios
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        private static IServiceCollection AddRepositories(this IServiceCollection services)
        {
            services.AddScoped<IChecklistRepository, ChecklistRepository>();

            return services;
        }

        /// <summary>
        /// Registro dos Services
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        private static IServiceCollection AddServices(this IServiceCollection services)
        {
            services.AddScoped<IChecklistService, ChecklistService>();

            return services;
        }
    }
}
