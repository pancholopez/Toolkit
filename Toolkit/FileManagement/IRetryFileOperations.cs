using System.IO;
using System.Threading.Tasks;

namespace FileManagement
{
    public interface IRetryFileOperations
    {
        Task<int> ReadAsync(Stream input, byte[] buffer);
        Task WriteAsync(FileStream output, byte[] buffer, int count);
    }
}