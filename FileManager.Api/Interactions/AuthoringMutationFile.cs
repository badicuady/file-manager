using System.IO;
using System.Linq;
using FileManager.Shared.Constants;
using GraphQL;
using GraphQL.Attachments;
using GraphQL.Types;

namespace FileManager.Api.Interactions
{
    public partial class AuthoringMutation : ObjectGraphType
    {
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

        private void AddCopyFile() => 
            FieldAsync<BooleanGraphType>
                (
                    "copyFile",
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
                            Name = "copyFileName",
                            ResolvedType = new StringGraphType()
                        }
                    ),
                    resolve: async context =>
                    {
                        var activeDirectory = context.GetArgument<string>("activeDirectory");
                        var oldFileName = context.GetArgument<string>("oldFileName");
                        var copyFileName = context.GetArgument<string>("copyFileName");

                        await _fileRepository.CopyFileHandler.Handle(activeDirectory, oldFileName, copyFileName, context.CancellationToken);

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
