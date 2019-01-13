using System;

namespace FileManagement.Core
{
    public sealed class FileCopyStats : IFileCopySummary
    {
        private readonly IFileCopySummary _summary;
        public string SourceFilePath => _summary.SourceFilePath;
        public string DestinationFilePath => _summary.DestinationFilePath;
        public long TotalBytesCopied => _summary.TotalBytesCopied;
        public double ElapsedMilliseconds { get; }
        public double BytesPerSecond { get; }

        private FileCopyStats(IFileCopySummary summary, TimeSpan? elapsed)
        {
            _summary = summary;
            ElapsedMilliseconds = elapsed?.TotalMilliseconds ?? 0;
            BytesPerSecond = summary.TotalBytesCopied / elapsed?.TotalSeconds ?? 0;
        }

        public static IFileCopySummary Create(IFileCopySummary summary, TimeSpan? elapsed)
            => new FileCopyStats(summary, elapsed);
    }
}