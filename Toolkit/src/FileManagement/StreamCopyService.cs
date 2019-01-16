using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using FileManagement.Core;

namespace FileManagement
{
    public class StreamCopyService : IFileCopyService
    {
        private readonly IRetryService _retry;

        public StreamCopyService(IRetryService retry)
        {
            _retry = retry;
        }

        public async Task CopyAsync(FileItem source, FileItem destination, 
            IProgress<long> progress, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();   //exit before reading
            var buffer = new byte[128 * 1024]; //4k minimum, 128k recommended
            var totalBytesWritten = 0L;
            using (var input = new FileStream(source.FilePath, FileMode.Open, FileAccess.Read))
            using (var output = new FileStream(destination.FilePath, FileMode.Create, FileAccess.Write))
            {
                int bytesRead;
                while ((bytesRead = await _retry.ReadAsync(input, buffer, cancellationToken).ConfigureAwait(false)) > 0)
                {
                    await _retry.WriteAsync(output, buffer, bytesRead, cancellationToken);
                    totalBytesWritten += bytesRead;
                    progress?.Report(totalBytesWritten);
                    cancellationToken.ThrowIfCancellationRequested(); //exit after writing and before reading next chunk
                }
            }
        }
    }
}