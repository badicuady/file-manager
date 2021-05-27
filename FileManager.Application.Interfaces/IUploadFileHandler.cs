using System.IO;
using System.Threading;
using System.Threading.Tasks;
using FileManager.Shared.Constants;

namespace FileManager.Application.Interfaces
{
    public interface IUploadFileHandler
    {
        Task Handle
        (
            string activeDirectory = PathConstants.BaseDirectorySeparatorChar,
            string uploadFileName = null,
            MemoryStream uploadFile = null,
            CancellationToken cancelationToken = default
        );
    }
}
