using System;
using System.IO;

namespace FileManagement.Core
{
    public sealed class FileItem
    {
        public string FilePath { get; }
        public bool DirectoryExists { get; }
        public long SizeInBytes { get; }
        public bool FileExists { get; }

        private FileItem(string filePath, bool fileExists, bool directoryExists, long sizeInBytes)
        {
            FilePath = filePath;
            DirectoryExists = directoryExists;
            SizeInBytes = sizeInBytes;
            FileExists = fileExists;
        }

        public static FileItem Create(string filePath)
        {
            if (string.IsNullOrWhiteSpace(filePath)) throw new ArgumentNullException(nameof(filePath));
            if (!Path.IsPathRooted(filePath)) throw new ArgumentException("Invalid file path", nameof(filePath));

            var fileInfo = new FileInfo(filePath);
            return new FileItem(filePath, fileInfo.Exists,
                fileInfo.Directory != null && fileInfo.Directory.Exists, fileInfo.Length);
        }

        public static FileItem Null => Create(Path.GetRandomFileName());
    }
}