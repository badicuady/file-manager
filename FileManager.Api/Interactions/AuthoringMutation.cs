using System.IO;
using System.Linq;
using FileManager.Api.Types.OutputTypes;
using FileManager.Application.Interfaces;
using FileManager.Shared.Constants;
using GraphQL;
using GraphQL.Attachments;
using GraphQL.Types;

namespace FileManager.Api.Interactions
{
    public class AuthoringMutation : ObjectGraphType
    {
        private readonly IDirectoryRepository _directoryRepository;
        private readonly IFileRepository _fileRepository;

        public AuthoringMutation
        (
            IDirectoryRepository directoryRepository,
            IFileRepository fileRepository
        )
        {
            Name = "AuthoringMutation";
            Description = "Authoring Mutation";

            _directoryRepository = directoryRepository;
            _fileRepository = fileRepository;

            AddCreateDirectory();
            AddDeleteDirectory();
            AddRenameDirectory();

            AddUploadFile();
            AddDeleteFile();
            AddRenameFile();
        }

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

        private void AddUploadFile() =>
            FieldAsync<BooleanGraphType>(
                "uploadFile",
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
                        Name = "uploadFileName",
                        ResolvedType = new StringGraphType()
                    }
                ),
                resolve: async context =>
                {
                    var activeDirectory = context.GetArgument<string>("activeDirectory");
                    var uploadFileName = context.GetArgument<string>("uploadFileName");
                    var incomingAttachments = context.IncomingAttachments();
                    var attachmentStream = incomingAttachments.Values.FirstOrDefault();
                    var stream = CreateMemoryStream(attachmentStream);

                    await _fileRepository.UploadFileHandler.Handle(activeDirectory, uploadFileName ?? attachmentStream.Name, stream, context.CancellationToken);

                    return true;
                });

        private void AddDeleteFile() =>
            FieldAsync<BooleanGraphType>
                (
                    "deleteFile",
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
                            Name = "deleteFileName",
                            ResolvedType = new StringGraphType()
                        }
                    ),
                    resolve: async context =>
                    {
                        var activeDirectory = context.GetArgument<string>("activeDirectory");
                        var deleteFileName = context.GetArgument<string>("deleteFileName");

                        await _fileRepository.DeleteFileHandler.Handle(activeDirectory, deleteFileName, context.CancellationToken);

                        return true;
                    }
                );

        private void AddRenameFile() =>
            FieldAsync<BooleanGraphType>
                (
                    "renameFile",
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
                            Name = "oldFileName",
                            ResolvedType = new StringGraphType()
                        },
                        new QueryArgument<NonNullGraphType<StringGraphType>>
                        {
                            Name = "renameFileName",
                            ResolvedType = new StringGraphType()
                        }
                    ),
                    resolve: async context =>
                    {
                        var activeDirectory = context.GetArgument<string>("activeDirectory");
                        var oldFileName = context.GetArgument<string>("oldFileName");
                        var renameFileName = context.GetArgument<string>("renameFileName");

                        await _fileRepository.RenameFileHandler.Handle(activeDirectory, oldFileName, renameFileName, context.CancellationToken);

                        return true;
                    }
                );

        private static MemoryStream CreateMemoryStream(AttachmentStream attachmentStream)
        {
            var stream = new MemoryStream();
            attachmentStream.CopyTo(stream);
            stream.Position = 0;
            return stream;
        }
    }
}
