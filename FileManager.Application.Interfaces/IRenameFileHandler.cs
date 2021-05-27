using System.Threading;
using System.Threading.Tasks;
using FileManager.Shared.Constants;

namespace FileManager.Application.Interfaces
{
    public interface IDeleteFileHandler
    {
        Task Handle
        (
            string activeDirectory = PathConstants.BaseDirectorySeparatorChar,
            string deleteFileName = null,
            CancellationToken cancelationToken = default
        );
    }
}
