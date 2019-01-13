using System.IO;
using System.Threading.Tasks;
using FileManagement.Core;

namespace FileManagement
{
    public class Win32FileCopyService : IFileCopyService
    {
        public async Task<IFileCopySummary> CopyAsync(FileItem source, FileItem destination)
        {
            File.Copy(source.FilePath, destination.FilePath);
            await Task.CompletedTask;
            return FileCopySummary.Create(source, destination);
        }
    }
}