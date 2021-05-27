using FileManager.Api.Types.OutputTypes;
using FileManager.Shared.Constants;
using GraphQL;
using GraphQL.Types;

namespace FileManager.Api.Interactions
{
    public partial class AuthoringMutation : ObjectGraphType
    {
        private void AddCreateDirectory() =>
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

                        return await _directoryRepository.CreateDirectoryHandler.Handle(activeDirectory, createdDirectoryName, context.CancellationToken);
                    }
                );

        private void AddDeleteDirectory() =>
            FieldAsync<BooleanGraphType>
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

                        await _directoryRepository.DeleteDirectoryHandler.Handle(activeDirectory, deletedDirectoryName, forced, context.CancellationToken);

                        return true;
                    }
                );

        private void AddRenameDirectory() =>
            FieldAsync<BooleanGraphType>
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

                        await _directoryRepository.RenameDirectoryHandler.Handle(activeDirectory, renameDirectoryName, context.CancellationToken);

                        return true;
                    }
                );

        private void AddCopyDirectory() =>
            FieldAsync<BooleanGraphType>
                (
                    "copyDirectory",
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
                            Name = "copyDirectoryName",
                            ResolvedType = new StringGraphType()
                        }
                    ),
                    resolve: async context =>
                    {
                        var activeDirectory = context.GetArgument<string>("activeDirectory");
                        var copyDirectoryName = context.GetArgument<string>("copyDirectoryName");

                        await _directoryRepository.CopyDirectoryHandler.Handle(activeDirectory, copyDirectoryName, context.CancellationToken);

                        return true;
                    }
                );
    }
}
