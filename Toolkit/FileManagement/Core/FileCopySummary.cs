namespace FileManagement.Core
{
    public sealed class FileCopySummary : IFileCopySummary
    {
        public string SourceFilePath { get; }
        public string DestinationFilePath { get; }
        public long TotalBytesCopied { get; }

        private FileCopySummary(string sourceFilePath, string destinationFilePath, long totalBytesCopied)
        {
            SourceFilePath = sourceFilePath;
            DestinationFilePath = destinationFilePath;
            TotalBytesCopied = totalBytesCopied;
        }

        public static IFileCopySummary Create(FileItem source, FileItem destination, long totalBytesCopied)
            => new FileCopySummary(source.FilePath, destination.FilePath, totalBytesCopied);
    }
}