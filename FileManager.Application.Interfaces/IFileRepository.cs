namespace FileManager.Application.Interfaces
{
    public interface IFileRepository
    {
        IDeleteFileHandler DeleteFileHandler { get; }

        IRenameFileHandler RenameFileHandler { get; }
    }
}
