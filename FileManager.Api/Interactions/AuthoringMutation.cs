using FileManager.Application.Interfaces;

namespace FileManager.Api.Interactions
{
    public partial class AuthoringMutation
    {
        private readonly IDirectoryRepository _directoryRepository;
        private readonly IFileRepository _fileRepository;

        public AuthoringMutation
        (
            IDirectoryRepository directoryRepository,
            IFileRepository fileRepository
        )
        {
            Name = "AuthoringMutation";
            Description = "Authoring Mutation";

            _directoryRepository = directoryRepository;
            _fileRepository = fileRepository;

            AddCreateDirectory();
            AddDeleteDirectory();
            AddRenameDirectory();
            AddCopyDirectory();

            AddUploadFile();
            AddDeleteFile();
            AddRenameFile();
            AddCopyFile();
        }
    }
}
