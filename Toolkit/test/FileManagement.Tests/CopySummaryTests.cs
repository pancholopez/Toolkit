using System;
using FileManagement.Core;
using Xunit;

namespace FileManagement.Tests
{
    public class CopySummaryTests
    {
        public CopySummaryTests()
        {

        }

        [Fact]
        public void Create_AllOk_ReturnsValidInstance()
        {
            var source = FileItem.Existing;
            var destination = FileItem.Existing;
            var instance = CopySummary.Create(source, destination, 1);

            Assert.NotNull(instance);
            Assert.Equal(source.FilePath, instance.SourceFilePath);
            Assert.Equal(destination.FilePath, instance.DestinationFilePath);
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
            var error = Assert.Throws<ArgumentNullException>(() => CopySummary.Create(FileItem.Existing, null, 0));
            Assert.Equal("destination", error.ParamName);
        }

        [Fact]
        public void Create_TotalBytesCopiedNegative_ThrowsException()
        {
            var error = Assert.Throws<ArgumentOutOfRangeException>(() => CopySummary.Create(FileItem.Existing, FileItem.Existing, -1));
            Assert.Equal("totalBytesCopied", error.ParamName);
        }
    }
}
