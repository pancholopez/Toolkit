using System;
using System.IO;
using System.Threading.Tasks;
using FileManagement.Core;

namespace FileManagement
{
    public class RetryFileStreamCopyService : IFileCopyService
    {
        private async Task<int> ReadRetryAsync(Stream input, byte[] buffer, Stream output)
        {
            int totalBytesRead = 0;
            int bytesRead;

            do
            {
                //this can fail, so retry
                bytesRead = await input.ReadAsync(buffer, 0, buffer.Length).ConfigureAwait(false);

                totalBytesRead += bytesRead;

                await WriteRetryAsync(output, buffer, bytesRead);
            } while (bytesRead > 0);

            return totalBytesRead;
        }

        private async Task WriteRetryAsync(Stream output, byte[] buffer, int bytesToWrite)
        {
            var retryCount = 0;
            var retryLimit = 3;
            var retry = true;
            while (retry)
            {
                try
                {
                    await output.WriteAsync(buffer, 0, bytesToWrite).ConfigureAwait(false);
                    retry = false;
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
                    {
                        retry = false;
                        //trow retryException
                    }
                }
            }//end retryWrite
        }

        public async Task<IFileCopySummary> CopyAsync(FileItem source, FileItem destination)
        {
            var buffer = new byte[32 * 1024];   //4k minimum
            var input = new FileStream(source.FilePath, FileMode.Open, FileAccess.Read);
            var output = new FileStream(destination.FilePath, FileMode.Create, FileAccess.Write);
            var totalBytesRead = 0;
            var retryCount = 0;
            var retryLimit = 3;
            try
            {
                totalBytesRead = await ReadRetryAsync(input, buffer, output);
            }
            catch (Exception exception)
            {
                //if there is a write exception abort
                //otherwise increment readRetry counter
                Console.WriteLine(exception);
            }
            finally
            {
                input.Close();
                output.Close();
            }
            return FileCopySummary.Create(source, destination, totalBytesRead);
        }
    }
}