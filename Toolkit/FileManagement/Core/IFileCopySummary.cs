namespace FileManagement.Core
{
    public interface IFileCopySummary
    {
        string SourceFilePath { get; }
        string DestinationFilePath { get; }
        long TotalBytesCopied { get; }
    }
}