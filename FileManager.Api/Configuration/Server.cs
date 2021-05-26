using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace FileManager.Api.Configuration
{
    public static class Server
    {
        public static IServiceCollection ConfigureServer(this IServiceCollection services) =>
            services.Configure<KestrelServerOptions>(options => options.AllowSynchronousIO = true)
                .Configure<IISServerOptions>(options => options.AllowSynchronousIO = true);

        public static IServiceCollection ConfigureServerCapabilities(this IServiceCollection services) =>
            services.AddHsts(options => options.Preload = true)
                .AddMemoryCache(options => options.ExpirationScanFrequency = new TimeSpan(0, 30, 0))
                .AddHttpClient()
                .AddLogging(builder => builder.AddConsole());
    }
}
