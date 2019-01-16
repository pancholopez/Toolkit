using System.IO;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using FileManagement.Core;

namespace FileManagement
{
    public class RetryStreamCopyService //: IFileCopyService
    {
        private readonly RetryService _retryService;

        public RetryStreamCopyService(RetryService retryService)
        {
            _retryService = retryService;
        }

        public async Task<T> CopyAsync<T>(FileItem source, FileItem destination) where T : CopySummary
        {
            var buffer = new byte[32 * 1024];   //4k minimum - 128k recommended
            var input = new FileStream(source.FilePath, FileMode.Open, FileAccess.Read);
            var output = new FileStream(destination.FilePath, FileMode.Create, FileAccess.Write);
            var totalBytesWritten = 0;
            try
            {
                int bytesRead;
                do
                {
                    bytesRead = await _retryService.ReadAsync(input, buffer, CancellationToken.None).ConfigureAwait(false);
                    await _retryService.WriteAsync(output, buffer, bytesRead, CancellationToken.None).ConfigureAwait(false);
                    totalBytesWritten += bytesRead;
                } while (bytesRead > 0);
            }
            finally
            {
                input.Close();
                output.Close();
            }
            return (T)CopySummary.Create(source, destination, totalBytesWritten);
        }
    }
}