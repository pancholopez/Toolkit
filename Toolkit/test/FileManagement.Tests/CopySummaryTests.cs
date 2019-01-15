using System;
using FileManagement.Core;
using Xunit;

namespace FileManagement.Tests
{
    public class CopySummaryTests
    {
        private readonly FileItem _source;
        private readonly FileItem _destination;

        public CopySummaryTests()
        {
            _source = FileItem.Existing;
            _destination = FileItem.Existing;
        }

        [Fact]
        public void Create_AllOk_ReturnsValidInstance()
        {
            var instance = CopySummary.Create(_source, _destination, 1);

            Assert.NotNull(instance);
            Assert.Equal(_source.FilePath, instance.SourceFilePath);
            Assert.Equal(_destination.FilePath, instance.DestinationFilePath);
            Assert.Equal(1, instance.TotalBytesCopied);
        }

        [Fact]
        public void Create_SourceFileItemNull_ThrowsException()
        {
            var error = Assert.Throws<ArgumentNullException>(() => CopySummary.Create(null, null, 0));
            Assert.Equal("source", error.ParamName);
        }

        [Fact]
        public void Create_DestinationFileItemNull_ThrowsException()
        {
            var error = Assert.Throws<ArgumentNullException>(() => CopySummary.Create(_source, null, 0));
            Assert.Equal("destination", error.ParamName);
        }

        [Fact]
        public void Create_TotalBytesCopiedNegative_ThrowsException()
        {
            var error = Assert.Throws<ArgumentOutOfRangeException>(() => CopySummary.Create(_source, _destination, -1));
            Assert.Equal("totalBytesCopied", error.ParamName);
        }
    }
}
