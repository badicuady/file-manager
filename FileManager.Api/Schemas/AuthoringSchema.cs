using System;
using FileManager.Api.Interactions;
using GraphQL.Types;
using GraphQL.Utilities;
using Microsoft.Extensions.DependencyInjection;

namespace FileManager.Api.Schemas
{
    public class AuthoringSchema : Schema
    {
        public AuthoringSchema(IServiceProvider serviceProvider) : base(serviceProvider)
        {
            Query = serviceProvider.GetRequiredService<AuthoringQuery>();
            Mutation = serviceProvider.GetRequiredService<AuthoringMutation>();
        }
    }
}
