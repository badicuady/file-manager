using FileManager.Application.Interfaces;

namespace FileManager.Application.Handlers.Repositories
{
    public class FileRepository : IFileRepository
    {
        private readonly IUploadFileHandler _uploadFileHandler;
        private readonly IDeleteFileHandler _deleteFileHandler;
        private readonly IRenameFileHandler _renameFileHandler;

        public FileRepository
        (
            IUploadFileHandler uploadFileHandler,
            IDeleteFileHandler deleteFileHandler,
            IRenameFileHandler renameFileHandler
        )
        {
            _uploadFileHandler = uploadFileHandler;
            _deleteFileHandler = deleteFileHandler;
            _renameFileHandler = renameFileHandler;
        }

        public IUploadFileHandler UploadFileHandler => _uploadFileHandler;

        public IDeleteFileHandler DeleteFileHandler => _deleteFileHandler;

        public IRenameFileHandler RenameFileHandler => _renameFileHandler;
    }
}
