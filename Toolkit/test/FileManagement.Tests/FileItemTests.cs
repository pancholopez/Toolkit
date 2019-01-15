using System;
using System.IO;
using System.Reflection;
using FileManagement.Core;
using Xunit;

namespace FileManagement.Tests
{
    public class FileItemTests
    {
        private readonly string _baseDirectory = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
        private readonly string _testDataFilePath;

        public FileItemTests()
        {
            //todo: generate and destroy this file programatically
            var testDataDirectory = Path.GetFullPath(Path.Combine(_baseDirectory, @"..\..\..\testdata"));
            _testDataFilePath = Path.Combine(testDataDirectory, "delete.me");
        }

        [Fact]
        public void Create_ExistingFile_ReturnValidInstance()
        {
            var fileItem = FileItem.Create(_testDataFilePath);

            Assert.True(fileItem.DirectoryExists);
            Assert.True(fileItem.FileExists);
            Assert.True(fileItem.SizeInBytes > 0);
            Assert.Equal(_testDataFilePath, fileItem.FilePath);
        }

        [Fact]
        public void Create_NonExistingFileExistingDirectory_ReturnValidInstance()
        {
            var randomFilePath = Path.Combine(_baseDirectory, Path.GetRandomFileName());

            var fileItem = FileItem.Create(randomFilePath);

            Assert.True(fileItem.DirectoryExists);
            Assert.False(fileItem.FileExists);
            Assert.Equal(0, fileItem.SizeInBytes);
            Assert.Equal(randomFilePath, fileItem.FilePath);
        }

        [Fact]
        public void Create_NonExistingFileNonExistingDirectory_ReturnValidInstance()
        {
            var randomDirectory = Path.Combine(_baseDirectory,
                $"fakedir_{DateTime.Now.Millisecond}\\{Path.GetRandomFileName()}");

            var fileItem = FileItem.Create(randomDirectory);

            Assert.False(fileItem.DirectoryExists);
            Assert.False(fileItem.FileExists);
            Assert.Equal(0, fileItem.SizeInBytes);
            Assert.Equal(randomDirectory, fileItem.FilePath);
        }

        [Theory]
        [InlineData("")]
        [InlineData("    ")]
        [InlineData(default(string))]
        public void Create_InvalidPath_ThrowsException(string path)
        {
            var error = Assert.Throws<ArgumentNullException>(() => FileItem.Create(path));
            Assert.Equal("filePath", error.ParamName);
        }

        [Theory]
        [InlineData("notValidPath")]
        [InlineData(@"not\rooted\folder")]
        public void Create_NotExistingPath_ThrowsException(string path)
        {
            var error = Assert.Throws<ArgumentException>(() => FileItem.Create(path));
            Assert.Equal("filePath", error.ParamName);
        }
    }
}