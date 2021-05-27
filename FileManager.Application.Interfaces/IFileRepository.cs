namespace FileManager.Application.Interfaces
{
    public interface IFileRepository
    {
        IUploadFileHandler UploadFileHandler { get; }

        IDeleteFileHandler DeleteFileHandler { get; }

        IRenameFileHandler RenameFileHandler { get; }
    }
}
