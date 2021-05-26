using System;
using System.Linq;
using System.Reflection;
using AutoMapper;
using Microsoft.Extensions.DependencyInjection;

namespace FileManager.Api.Configuration
{
    public static class AutoMapper
    {
        public static IServiceCollection ConfigureAutoMapper(this IServiceCollection services)
        {
            var profileTypes = Assembly.GetExecutingAssembly()
                .GetTypes()
                .Where(t => typeof(Profile).IsAssignableFrom(t) && t.IsClass)
                .Select(t => (Profile)Activator.CreateInstance(t));

            return services.AddAutoMapper(
                (serviceProvider, options) => options.AddProfiles(profileTypes),
                (Assembly[])null);
        }
    }
}
