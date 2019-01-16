using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using FileManagement.Core;

namespace FileManagement
{
    public class StreamCopyService : IFileCopyService
    {
        public async Task CopyAsync(FileItem source, FileItem destination, 
            IProgress<int> progress, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();   //exit before reading
            using (var input = new FileStream(source.FilePath, FileMode.Open, FileAccess.Read))
            {
                using (var output = new FileStream(destination.FilePath, FileMode.Create, FileAccess.Write))
                {
                    var buffer = new byte[32 * 1024];   //4k minimum, 128k recommended
                    var totalBytesWritten = 0;
                    int bytesRead;
                    while ((bytesRead = await input.ReadAsync(buffer, 0, buffer.Length, cancellationToken).ConfigureAwait(false)) > 0)
                    {
                        cancellationToken.ThrowIfCancellationRequested();   //exit before writing
                        await output.WriteAsync(buffer, 0, bytesRead, cancellationToken);
                        totalBytesWritten += bytesRead;
                        progress?.Report(totalBytesWritten);
                        cancellationToken.ThrowIfCancellationRequested(); //exit after writing and before reading next chunk
                    }
                }
            }
        }
    }
}