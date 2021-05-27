using FileManager.Application.Interfaces;

namespace FileManager.Application.Handlers.Repositories
{
    public class DirectoryRepository : IDirectoryRepository
    {
        private readonly ICreateDirectoryHandler _createDirectoryHandler;
        private readonly IDeleteDirectoryHandler _deleteDirectoryHandler;
        private readonly IRenameDirectoryHandler _renameDirectoryHandler;

        public DirectoryRepository
        (
            ICreateDirectoryHandler createDirectoryHandler,
            IDeleteDirectoryHandler deleteDirectoryHandler,
            IRenameDirectoryHandler renameDirectoryHandler
        )
        {
            _createDirectoryHandler = createDirectoryHandler;
            _deleteDirectoryHandler = deleteDirectoryHandler;
            _renameDirectoryHandler = renameDirectoryHandler;
        }

        public ICreateDirectoryHandler CreateDirectoryHandler => _createDirectoryHandler;

        public IDeleteDirectoryHandler DeleteDirectoryHandler => _deleteDirectoryHandler;

        public IRenameDirectoryHandler RenameDirectoryHandler => _renameDirectoryHandler;
    }
}
