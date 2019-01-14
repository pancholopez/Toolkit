using System.Threading.Tasks;

namespace FileManagement.Core
{
    public interface IFileCopyService
    {
        Task<T> CopyAsync<T>(FileItem source, FileItem destination) where T : CopySummary;
    }
}
