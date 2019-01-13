using System.Threading.Tasks;

namespace FileManagement.Core
{
    public interface IFileCopyService
    {
        Task<IFileCopySummary> CopyAsync(FileItem source, FileItem destination);
    }
}
