using System.IO;
using FileManager.Domain.Models.Enums;

namespace FileManager.Domain.Models.Manager
{
    public abstract class Item
    {
        public string Name { get; set; }

        public IconType Icon { get; set; }

        public FileSystemInfo Metadata { get; set; }
    }
}
