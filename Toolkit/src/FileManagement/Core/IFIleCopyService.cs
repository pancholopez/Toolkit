using System;
using System.Threading.Tasks;

namespace FileManagement.Core
{
    public interface IFileCopyService
    {
        Task<T> CopyAsync<T>(FileItem source, FileItem destination, IProgress<int> progress) 
            where T : CopySummary;
    }
}
