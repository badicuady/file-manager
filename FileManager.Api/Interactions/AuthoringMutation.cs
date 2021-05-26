using FileManager.Api.Types.OutputTypes;
using FileManager.Application.Interfaces;
using FileManager.Shared.Constants;
using GraphQL;
using GraphQL.Types;

namespace FileManager.Api.Interactions
{
    public class AuthoringMutation : ObjectGraphType
    {
        public AuthoringMutation
        (
            ICreateDirectoryHandler createDirectoryHandler,
            IDeleteDirectoryHandler deleteDirectoryHandler,
            IRenameDirectoryHandler renameDirectoryHandler
        )
        {
            Name = "AuthoringMutation";
            Description = "Authoring Mutation";

            FieldAsync<ItemType>
                (
                    "createDirectory",
                    arguments: new QueryArguments
                    (
                        new QueryArgument<StringGraphType>
                        {
                            Name = "activeDirectory",
                            DefaultValue = PathConstants.BaseDirectorySeparatorChar,
                            ResolvedType = new StringGraphType()
                        },
                        new QueryArgument<NonNullGraphType<StringGraphType>>
                        {
                            Name = "createdDirectoryName",
                            ResolvedType = new StringGraphType()
                        }
                    ),
                    resolve: async context =>
                    {
                        var activeDirectory = context.GetArgument<string>("activeDirectory");
                        var createdDirectoryName = context.GetArgument<string>("createdDirectoryName");

                        return await createDirectoryHandler.Handle(activeDirectory, createdDirectoryName, context.CancellationToken);
                    }
                );

            FieldAsync<ItemType>
                (
                    "deleteDirectory",
                    arguments: new QueryArguments
                    (
                        new QueryArgument<StringGraphType>
                        {
                            Name = "activeDirectory",
                            DefaultValue = PathConstants.BaseDirectorySeparatorChar,
                            ResolvedType = new StringGraphType()
                        },
                        new QueryArgument<NonNullGraphType<StringGraphType>>
                        {
                            Name = "deletedDirectoryName",
                            ResolvedType = new StringGraphType()
                        },
                        new QueryArgument<BooleanGraphType>
                        {
                            Name = "forced",
                            ResolvedType = new BooleanGraphType(),
                            DefaultValue = true,
                            Description = "If true, this will delete recursively all contents of the directory, inclusive the directory."
                        }
                    ),
                    resolve: async context =>
                    {
                        var activeDirectory = context.GetArgument<string>("activeDirectory");
                        var deletedDirectoryName = context.GetArgument<string>("deletedDirectoryName");
                        var forced = context.GetArgument<bool>("forced");

                        await deleteDirectoryHandler.Handle(activeDirectory, deletedDirectoryName, forced, context.CancellationToken);

                        return true;
                    }
                );

            FieldAsync<ItemType>
                (
                    "renameDirectory",
                    arguments: new QueryArguments
                    (
                        new QueryArgument<StringGraphType>
                        {
                            Name = "activeDirectory",
                            DefaultValue = PathConstants.BaseDirectorySeparatorChar,
                            ResolvedType = new StringGraphType()
                        },
                        new QueryArgument<NonNullGraphType<StringGraphType>>
                        {
                            Name = "renameDirectoryName",
                            ResolvedType = new StringGraphType()
                        }
                    ),
                    resolve: async context =>
                    {
                        var activeDirectory = context.GetArgument<string>("activeDirectory");
                        var renameDirectoryName = context.GetArgument<string>("renameDirectoryName");

                        await renameDirectoryHandler.Handle(activeDirectory, renameDirectoryName, context.CancellationToken);

                        return true;
                    }
                );
        }
    }
}
