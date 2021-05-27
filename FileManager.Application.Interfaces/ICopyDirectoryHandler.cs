using System.Threading;
using System.Threading.Tasks;
using FileManager.Shared.Constants;

namespace FileManager.Application.Interfaces
{
    public interface ICopyDirectoryHandler
    {
        Task Handle
        (
            string activeDirectory = PathConstants.BaseDirectorySeparatorChar,
            string copyDirectoryName = null,
            CancellationToken cancelationToken = default
        );
    }
}
