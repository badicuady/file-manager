using System.Threading;
using System.Threading.Tasks;
using FileManager.Shared.Constants;

namespace FileManager.Application.Interfaces
{
    public interface ICopyFileHandler
    {
        Task Handle
        (
            string activeDirectory = PathConstants.BaseDirectorySeparatorChar,
            string oldFileName = null,
            string copyFileName = null,
            CancellationToken cancelationToken = default
        );
    }
}
