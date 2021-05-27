using FileManager.Application.Interfaces;

namespace FileManager.Application.Handlers.Repositories
{
    public class FileRepository : IFileRepository
    {
        private readonly IUploadFileHandler _uploadFileHandler;
        private readonly IDeleteFileHandler _deleteFileHandler;
        private readonly IRenameFileHandler _renameFileHandler;
        private readonly ICopyFileHandler _copyFileHandler;

        public FileRepository
        (
            IUploadFileHandler uploadFileHandler,
            IDeleteFileHandler deleteFileHandler,
            IRenameFileHandler renameFileHandler,
            ICopyFileHandler copyFileHandler
        )
        {
            _uploadFileHandler = uploadFileHandler;
            _deleteFileHandler = deleteFileHandler;
            _renameFileHandler = renameFileHandler;
            _copyFileHandler = copyFileHandler;
        }

        public IUploadFileHandler UploadFileHandler => _uploadFileHandler;

        public IDeleteFileHandler DeleteFileHandler => _deleteFileHandler;

        public IRenameFileHandler RenameFileHandler => _renameFileHandler;

        public ICopyFileHandler CopyFileHandler => _copyFileHandler;
    }
}
