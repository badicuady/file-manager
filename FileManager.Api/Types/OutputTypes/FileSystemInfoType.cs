using System.IO;
using FileManager.Shared.Extensions;
using GraphQL.Types;

namespace FileManager.Api.Types.OutputTypes
{
    public class FileSystemInfoType : ObjectGraphType<FileSystemInfo>
    {
        public FileSystemInfoType()
        {
            Name = "FileSystemInfoType";
            Field(t => t.Name);
            Field(t => t.CreationTime);
            Field(t => t.CreationTimeUtc);
            Field(t => t.Extension);
            Field(t => t.FullName);
            Field(t => t.LastAccessTime);
            Field(t => t.LastAccessTimeUtc);
            Field(t => t.LastWriteTime);
            Field(t => t.LastWriteTimeUtc);
            Field(name: "Attributes",
                type: typeof(ListGraphType<EnumerationGraphType<FileAttributes>>),
                resolve: fa => fa.Source.Attributes.FromFlags());
        }
    }
}
