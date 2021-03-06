﻿using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using FileManagement.Core;

namespace FileManagement
{
    public sealed class RetryService : IRetryService
    {
        private readonly RetrySettings _settings;

        public RetryService(RetrySettings settings)
        {
            _settings = settings;
        }

        public async Task Write<T>(Task<T> operation)
        {

        }

        public async Task<int> ReadAsync(Stream input, byte[] buffer, CancellationToken cancellationToken)
        {
            return await Retry(() => input.ReadAsync(buffer, 0, buffer.Length, cancellationToken), cancellationToken);
        }

        public async Task WriteAsync(FileStream output, byte[] buffer, int count, CancellationToken cancellationToken)
        {
            await Retry(async () =>
            {
                await output.WriteAsync(buffer, 0, count, cancellationToken);
                return await Task.FromResult(count);
            }, cancellationToken);
        }

        private async Task<int> Retry(Func<Task<int>> operation, CancellationToken cancellationToken)
        {
            var retryCount = 0;
            while (true)
            {
                try
                {
                    cancellationToken.ThrowIfCancellationRequested();
                    return await operation();
                }
                catch (TaskCanceledException)   //propagate if task was canceled
                {
                    throw;
                }
                catch (Exception exception)   //any other exception we just retry
                {
                    if (retryCount++ > _settings.Limit)
                        throw new RetryException("Retry operation limit reached.", exception);   //throw if we reached the retry limit
                    await Task.Delay(_settings.ElapsedMilliseconds, cancellationToken);
                }
            }
        }
    }
}