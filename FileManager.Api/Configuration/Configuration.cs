using FileManager.Shared.Settings;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace FileManager.Api.Configuration
{
    public static class Configuration
    {
        public static IServiceCollection ConfigureSettings(this IServiceCollection services, IConfiguration configuration) =>
            RegisterConfiguration<ManagerSettings>(services, configuration);

        private static IServiceCollection RegisterConfiguration<TImplementation>(IServiceCollection services, IConfiguration configuration)
            where TImplementation : class, new() =>
                RegisterConfiguration<TImplementation, TImplementation>(services, configuration);

        private static IServiceCollection RegisterConfiguration<TService, TImplementation>(IServiceCollection services, IConfiguration configuration)
            where TService : class
            where TImplementation : class, TService, new()
        {
            services.Configure<TImplementation>(configuration.GetSection(typeof(TImplementation).Name))
                .AddSingleton<TService>(provider => provider.GetRequiredService<IOptions<TImplementation>>().Value);
            return services;
        }
    }
}
