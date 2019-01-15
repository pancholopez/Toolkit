using System;
using System.IO;
using System.Threading.Tasks;
using FileManagement.Core;

namespace FileManagement
{
    public sealed class RetryFileOperations : IRetryFileOperations
    {
        private readonly RetrySettings _settings;

        public RetryFileOperations(RetrySettings settings)
        {
            _settings = settings;
        }

        public async Task<int> ReadAsync(Stream input, byte[] buffer)
        {
            return await RetryLogic(() => input.ReadAsync(buffer, 0, buffer.Length));
        }

        public async Task WriteAsync(FileStream output, byte[] buffer, int count)
        {
            await RetryLogic(async () =>
            {
                await output.WriteAsync(buffer, 0, count);
                return await Task.FromResult(count);
            });
        }

        private async Task<int> RetryLogic(Func<Task<int>> operation)
        {
            var retryCount = 0;
            while (true)
            {
                try
                {
                    return await operation().ConfigureAwait(false);
                }
                catch (Exception exception)
                {
                    Console.WriteLine(exception);
                    retryCount++;
                    await Task.Delay(_settings.ElapsedMilliseconds);
                }
                finally
                {
                    if (retryCount > _settings.Limit)
                        throw new RetryLimitException("Retry limit reached.");
                }
            }
        }
    }
}