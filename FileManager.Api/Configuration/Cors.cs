using FileManager.Shared.Settings;
using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace FileManager.Api.Configuration
{
    public static class Cors
    {
        public static IServiceCollection ConfigureCors(
            this IServiceCollection services,
            IWebHostEnvironment env,
            IConfiguration configuration,
            string policyName) =>
            services.AddCors(options =>
            {
                options.AddPolicy(
                    name: policyName,
                    builder =>
                    {
                        if (env.IsDevelopment())
                        {
                            ConfigureDevCors(builder);
                        }
                        else
                        {
                            ConfigureNonDevCors(configuration, builder);
                        }
                    });
            });

        private static void ConfigureDevCors(CorsPolicyBuilder builder) =>
            builder.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod();

        private static void ConfigureNonDevCors(IConfiguration configuration, CorsPolicyBuilder builder)
        {
            var corsSettings = configuration
                .GetSection(nameof(FileManagerCorsSettings))
                .Get<FileManagerCorsSettings>();

            if (corsSettings.HostOrigins?.Count > 0)
            {
                builder.WithOrigins(corsSettings.HostOrigins.ToArray());
            }

            if (corsSettings.AllowedMethods?.Count > 0)
            {
                builder.WithMethods(corsSettings.AllowedMethods.ToArray());
            }

            if (corsSettings.AllowedHeaders?.Count > 0)
            {
                builder.WithHeaders(corsSettings.AllowedHeaders.ToArray());
            }
        }
    }
}
