using FileManager.Api.Configuration;
using FileManager.Api.Schemas;
using FileManager.Shared.Constants;
using GraphQL.Server.Ui.Playground;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace FileManager.Api
{
    public class Startup
    {
        private readonly IConfiguration _configuration;
        private readonly IWebHostEnvironment _environment;

        public Startup(IConfiguration configuration, IWebHostEnvironment environment)
        {
            _configuration = configuration;
            _environment = environment;
        }

        public void ConfigureServices(IServiceCollection services) =>
            services
                .ConfigureServer()
                .ConfigureServerCapabilities()
                .ConfigureAutoMapper()
                .ConfigureDependencies()
                .ConfigureCors(_environment, _configuration, PathConstants.AllowedSpecificOrigins)
                .ConfigureSettings(_configuration)
                .ConfigureGraphQL(_environment);

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment()) { app.UseDeveloperExceptionPage(); }
            else { app.UseHsts(); }

             app.UseCors(cors =>
                cors
                    .AllowAnyHeader()
                    .AllowAnyMethod()
                    .AllowAnyOrigin())
                .UseWebSockets()
                .UseGraphQLUpload<AuthoringSchema>() // for GraphQL Upload
                .UseGraphQL<AuthoringSchema>() // for GraphQL
                .UseGraphQLPlayground(new PlaygroundOptions()); // for GraphQL
        }
    }
}
