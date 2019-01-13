using System;
using System.IO;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using FileManagement.Core;

namespace FileManagement
{
    public class RetryFileStreamCopyService : IFileCopyService
    {
        private static async Task<int> RetryAsync(Func<ConfiguredTaskAwaitable<int>> operation)
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
                        throw new RetryLimitException("Retry file read limit reached.");
                }
            }
        }

        private static async Task<int> ReadRetryAsync(Stream input, byte[] buffer)
        {
            var retryCount = 0;
            var retryLimit = 3;
            while (true)
            {
                try
                {
                    return await input.ReadAsync(buffer, 0, buffer.Length).ConfigureAwait(false);
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
                        throw new RetryLimitException("Retry file read limit reached.");
                }
            }
        }

        private static async Task<int> WriteRetryAsync(Stream output, byte[] buffer, int bytesToWrite)
        {
            var retryCount = 0;
            var retryLimit = 3;
            while (true)
            {
                try
                {
                    await output.WriteAsync(buffer, 0, bytesToWrite).ConfigureAwait(false);
                    return bytesToWrite;
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
                        throw new RetryLimitException("Retry file write limit reached.");
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
                    bytesRead = await ReadRetryAsync(input, buffer);
                    totalBytesWritten += await WriteRetryAsync(output, buffer, bytesRead);
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