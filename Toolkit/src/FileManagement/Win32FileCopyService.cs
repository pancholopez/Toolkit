using System.IO;
using System.Threading.Tasks;
using FileManagement.Core;

namespace FileManagement
{
    public class Win32FileCopyService : IFileCopyService
    {
        public async Task<T> CopyAsync<T>(FileItem source, FileItem destination) where T : CopySummary
        {
            File.Copy(source.FilePath, destination.FilePath);
            await Task.CompletedTask;
            return (T)CopySummary.Create(source, destination, source.SizeInBytes);
        }
    }
}