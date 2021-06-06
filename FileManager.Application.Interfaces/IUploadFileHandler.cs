using System.IO;
using System.Threading;
using System.Threading.Tasks;
using FileManager.Domain.Models.Manager;
using FileManager.Shared.Constants;

namespace FileManager.Application.Interfaces
{
    public interface IUploadFileHandler
    {
        Task<Item> Handle
        (
            string activeDirectory = PathConstants.BaseDirectorySeparatorChar,
            string uploadFileName = null,
            MemoryStream uploadFile = null,
            CancellationToken cancelationToken = default
        );
    }
}
