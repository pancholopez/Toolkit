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
            => new CopySummary(source.FilePath, destination.FilePath, totalBytesCopied);
    }
}