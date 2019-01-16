using System;

namespace FileManagement.Core
{
    public sealed class CopyStats : CopySummary
    {
        public long ElapsedMilliseconds { get; }
        public long BytesPerSecond { get; }

        private CopyStats(CopySummary summary, TimeSpan? elapsed)
            :base(summary.SourceFilePath,summary.DestinationFilePath,summary.TotalBytesCopied)
        {
            ElapsedMilliseconds = (long) (elapsed?.TotalMilliseconds ?? 0);
            BytesPerSecond = (long) (summary.TotalBytesCopied / elapsed?.TotalSeconds ?? 0);
        }

        public static CopyStats Create(CopySummary summary, TimeSpan? elapsed)
        {
            if(summary == null) throw new ArgumentNullException(nameof(summary));
            return new CopyStats(summary, elapsed);
        }
    }
}