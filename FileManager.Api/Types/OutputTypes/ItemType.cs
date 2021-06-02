using FileManager.Domain.Models.Manager;
using GraphQL.Types;

namespace FileManager.Api.Types.OutputTypes
{
    public class ItemType : ObjectGraphType<Item>
    {
        public ItemType()
        {
            Name = "ItemType";
            Field(t => t.Id);
            Field(t => t.Name);
            Field(t => t.Icon, type: typeof(IconTypeEnumType));
            Field(t => t.Size);
            Field(t => t.Metadata, type: typeof(FileSystemInfoType));
        }
    }
}
