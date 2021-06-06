using System.Threading;
using System.Threading.Tasks;
using FileManager.Domain.Models.Manager;
using FileManager.Shared.Constants;

namespace FileManager.Application.Interfaces
{
    public interface ICopyFileHandler
    {
        Task<Item> Handle
        (
            string activeDirectory = PathConstants.BaseDirectorySeparatorChar,
            string oldFileName = null,
            string copyFileName = null,
            CancellationToken cancelationToken = default
        );
    }
}
