using FileManager.Api.Types.OutputTypes;
using FileManager.Application.Interfaces;
using FileManager.Shared.Constants;
using GraphQL;
using GraphQL.Types;

namespace FileManager.Api.Interactions
{
    public class AuthoringQuery : ObjectGraphType
    {
        public AuthoringQuery(IGetItemsHandler getItemsHandler)
        {
            Name = "AuthoringQuery";
            Description = "Authoring Query";
            FieldAsync<ListGraphType<ItemType>>(
                "items",
                arguments: new QueryArguments
                (
                    new QueryArgument<StringGraphType>
                    {
                        Name = "path",
                        DefaultValue = PathConstants.BaseDirectorySeparatorChar,
                        ResolvedType = new StringGraphType()
                    }
                ),
                resolve: async context => 
                    await getItemsHandler.Handle(context.GetArgument<string>("path"), context.CancellationToken));
        }
    }
}
