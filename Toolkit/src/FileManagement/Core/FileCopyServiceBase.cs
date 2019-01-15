using System.IO;
using System.Threading.Tasks;

namespace FileManagement.Core
{
    public abstract class FileCopyServiceBase : IFileCopyService
    {
        public abstract Task<T> CopyAsync<T>(FileItem source, FileItem destination) where T : CopySummary;

        public virtual void Delete(FileItem file)
        {
            File.Delete(file.FilePath);
        }
    }
}