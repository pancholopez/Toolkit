using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace FileManagement.Core
{
    public interface IRetryService
    {
        Task<int> ReadAsync(Stream input, byte[] buffer, CancellationToken cancellationToken);
        Task WriteAsync(FileStream output, byte[] buffer, int count, CancellationToken cancellationToken);
    }
}