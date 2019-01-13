using System;
using System.IO;
using System.Threading.Tasks;
using FileManagement.Core;

namespace FileManagement
{
    public class RetryFileStreamCopyService : IFileCopyService
    {
        private static async Task<int> RetryAsync(Func<Task<int>> operation)
        {
            var retryCount = 0;
            var retryLimit = 3;
            while (true)
            {
                try
                {
                    return await operation();
                }
                catch (Exception exception)
                {
                    Console.WriteLine(exception);
                    retryCount++;
                    await Task.Delay(1000);
                }
                finally
                {
                    if (retryCount > retryLimit)
                        throw new RetryLimitException("Retry limit reached.");
                }
            }
        }

        public async Task<IFileCopySummary> CopyAsync(FileItem source, FileItem destination)
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
                    bytesRead = await RetryAsync(async () => 
                        await input.ReadAsync(buffer, 0, buffer.Length).ConfigureAwait(false));

                    var bytesToWrite = bytesRead;
                    totalBytesWritten += await RetryAsync(async () =>
                    {
                        await output.WriteAsync(buffer, 0, bytesToWrite).ConfigureAwait(false);
                        return bytesToWrite;
                    });
                } while (bytesRead > 0);
            }
            finally
            {
                input.Close();
                output.Close();
            }
            return FileCopySummary.Create(source, destination, totalBytesWritten);
        }
    }
}