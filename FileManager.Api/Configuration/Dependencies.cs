using FileManager.Application;
using FileManager.Application.Handlers.CommandHandlers;
using FileManager.Application.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace FileManager.Api.Configuration
{
    public static class DependencyInjection
    {
        public static IServiceCollection ConfigureDependencies(this IServiceCollection services)
        {
            services.AddScoped<IGetItemsHandler, GetItemsHandler>()
                .AddScoped<ICreateDirectoryHandler, CreateDirectoryHandler>()
                .AddScoped<IDeleteDirectoryHandler, DeleteDirectoryHandler>()
                .AddScoped<IRenameDirectoryHandler, RenameDirectoryHandler>();

            return services;
        }
    }
}
