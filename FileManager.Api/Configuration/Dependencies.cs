using FileManager.Application;
using FileManager.Application.Handlers.CommandHandlers;
using FileManager.Application.Handlers.Repositories;
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
                .AddScoped<IRenameDirectoryHandler, RenameDirectoryHandler>()
                .AddScoped<ICopyDirectoryHandler, CopyDirectoryHandler>()
                .AddScoped<IUploadFileHandler, UploadFileHandler>()
                .AddScoped<IDeleteFileHandler, DeleteFileHandler>()
                .AddScoped<IRenameFileHandler, RenameFileHandler>()
                .AddScoped<ICopyFileHandler, CopyFileHandler>()
                .AddScoped<IDirectoryRepository, DirectoryRepository>()
                .AddScoped<IFileRepository, FileRepository>();

            return services;
        }
    }
}
