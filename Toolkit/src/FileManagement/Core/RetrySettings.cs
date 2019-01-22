using System;

namespace FileManagement.Core
{
    public sealed class RetrySettings
    {
        public int Limit { get; }
        public int ElapsedMilliseconds { get; }

        public RetrySettings(int limit, int elapsedMilliseconds)
        {
            if (limit < 0)
                throw new ArgumentOutOfRangeException(nameof(limit), "retry limit should not be less than zero.");
            if (elapsedMilliseconds < 0)
                throw new ArgumentOutOfRangeException(nameof(elapsedMilliseconds), "elapsed milliseconds should not be less than zero.");

            Limit = limit;
            ElapsedMilliseconds = elapsedMilliseconds;
        }

        public static RetrySettings Default=>new RetrySettings(3,2000);
    }
}