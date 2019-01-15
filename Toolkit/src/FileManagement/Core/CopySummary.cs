using System;

namespace FileManagement.Core
{
    public class CopySummary
    {
        public string SourceFilePath { get; }
        public string DestinationFilePath { get; }
        public long TotalBytesCopied { get; }

        protected CopySummary(string sourceFilePath, string destinationFilePath, long totalBytesCopied)
        {
            SourceFilePath = sourceFilePath;
            DestinationFilePath = destinationFilePath;
            TotalBytesCopied = totalBytesCopied;
        }

        public static CopySummary Create(FileItem source, FileItem destination, long totalBytesCopied)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            if (destination == null) throw new ArgumentNullException(nameof(destination));
            if (totalBytesCopied < 0) throw new ArgumentOutOfRangeException(nameof(totalBytesCopied));
            return new CopySummary(source.FilePath, destination.FilePath, totalBytesCopied);
        }
    }
}