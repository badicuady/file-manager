using System.IO;
using FileManager.Domain.Models.Enums;
using FileManager.Shared.Extensions;

namespace FileManager.Domain.Models.Manager
{
    public abstract class Item
    {
        public string Id { get => Name.Sha256(); }

        public string Name { get; set; }

        public IconType Icon { get; set; }

        public FileSystemInfo Metadata { get; set; }

        public long Size { get; set; }
    }
}
