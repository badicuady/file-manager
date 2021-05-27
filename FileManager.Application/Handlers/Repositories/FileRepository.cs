using FileManager.Application.Interfaces;

namespace FileManager.Application.Handlers.Repositories
{
    public class FileRepository : IFileRepository
    {
        private readonly IDeleteFileHandler _deleteFileHandler;
        private readonly IRenameFileHandler _renameFileHandler;

        public FileRepository
        (
            IDeleteFileHandler deleteFileHandler,
            IRenameFileHandler renameFileHandler
        )
        {
            _deleteFileHandler = deleteFileHandler;
            _renameFileHandler = renameFileHandler;
        }

        public IDeleteFileHandler DeleteFileHandler => _deleteFileHandler;

        public IRenameFileHandler RenameFileHandler => _renameFileHandler;
    }
}
