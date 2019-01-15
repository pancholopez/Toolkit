namespace FileManagement.Core
{
    public sealed class RetrySettings
    {
        public int Limit { get; }
        public int ElapsedMilliseconds { get; }

        public RetrySettings(int limit, int elapsedMilliseconds)
        {
            Limit = limit;
            ElapsedMilliseconds = elapsedMilliseconds;
        }
    }
}