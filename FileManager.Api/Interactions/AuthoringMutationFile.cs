using System;
using System.IO;
using FileManager.Api.Types.OutputTypes;
using FileManager.Shared.Constants;
using GraphQL;
using GraphQL.Types;
using Microsoft.AspNetCore.Http;

namespace FileManager.Api.Interactions
{
    public partial class AuthoringMutation : ObjectGraphType
    {
        private void AddUploadFile() =>
            FieldAsync<ItemType>(
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
                    },
                    new QueryArgument<NonNullGraphType<StringGraphType>>
                    {
                        Name = "file",
                        ResolvedType = new StringGraphType()
                    }
                ),
                resolve: async context =>
                {
                    var activeDirectory = context.GetArgument<string>("activeDirectory");
                    var uploadFileName = context.GetArgument<string>("uploadFileName");
                    var fileAsBase64String = context.GetArgument<string>("file");

                    var fileAsBytes = Convert.FromBase64String(fileAsBase64String);
                    var attachmentStream = new MemoryStream(fileAsBytes);
                    var item = await _fileRepository.UploadFileHandler.Handle(activeDirectory, uploadFileName, attachmentStream, context.CancellationToken);

                    return item;
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
            FieldAsync<ItemType>
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

                        var item = await _fileRepository.CopyFileHandler.Handle(activeDirectory, oldFileName, copyFileName, context.CancellationToken);

                        return item;
                    }
                );

        private static MemoryStream CreateMemoryStream(IFormFile attachmentStream)
        {
            var stream = new MemoryStream();
            attachmentStream.CopyTo(stream);
            stream.Position = 0;
            return stream;
        }
    }
}
