using System;
using FileManager.Api.Schemas;
using FileManager.Domain.Models.GraphQL;
using GraphQL.Server;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace FileManager.Api.Configuration
{
    public static class GraphQL
    {
        public static IServiceCollection ConfigureGraphQL(this IServiceCollection services, IWebHostEnvironment environment)
        {
            services.AddScoped<AuthoringSchema>()
                .AddGraphQLUpload()
                .AddGraphQL(options =>
                {
                    options.EnableMetrics = environment.IsDevelopment();
                    options.UnhandledExceptionDelegate = ctx => { Console.WriteLine(ctx.OriginalException); };
                })
                .AddGraphTypes(ServiceLifetime.Scoped)
                .AddUserContextBuilder(context => new UserContext { User = context.User })
                .AddNewtonsoftJson()
                .AddDataLoader()
                .AddWebSockets()
                .AddErrorInfoProvider(options =>
                {
                    options.ExposeCode = true;
                    options.ExposeCodes = true;
                    options.ExposeData = true;
                    options.ExposeExceptionStackTrace = true;
                    options.ExposeExtensions = true;
                });

            return services;
        }
    }
}
