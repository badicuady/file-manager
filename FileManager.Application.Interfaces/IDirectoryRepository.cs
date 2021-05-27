namespace FileManager.Application.Interfaces
{
    public interface IDirectoryRepository
    {
        ICreateDirectoryHandler CreateDirectoryHandler { get; }

        IDeleteDirectoryHandler DeleteDirectoryHandler { get; }

        IRenameDirectoryHandler RenameDirectoryHandler { get; }
    }
}
