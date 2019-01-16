using System;
using System.Diagnostics;
using FileManagement.Core;
using Xunit;

namespace FileManagement.Tests
{
    public class CopyStatsTests
    {
        private readonly CopySummary _summary;
        public CopyStatsTests()
        {
            _summary = CopySummary.Create(FileItem.Existing, FileItem.Existing, 100);
        }

        [Fact]
        public void Create_SummaryMissing_ThrowsException()
        {
            var error = Assert.Throws<ArgumentNullException>(() => CopyStats.Create(null, null));
            Assert.Equal("summary", error.ParamName);
        }

        [Fact]
        public void Create_AllOk_ReturnsValidInstance()
        {
            var stopwatch = Stopwatch.StartNew();
            stopwatch.Stop();

            var stats = CopyStats.Create(_summary, stopwatch.Elapsed);

            Assert.Equal(stopwatch.ElapsedMilliseconds, stats.ElapsedMilliseconds);
            Assert.Equal((long)(stats.TotalBytesCopied / stopwatch.Elapsed.TotalSeconds), stats.BytesPerSecond);
        }

        [Fact]
        public void Create_MissingElapsed_ReturnsValidInstance()
        {
            var stats = CopyStats.Create(_summary, null);

            Assert.Equal(0, stats.ElapsedMilliseconds);
            Assert.Equal(0, stats.BytesPerSecond);
        }
    }
}