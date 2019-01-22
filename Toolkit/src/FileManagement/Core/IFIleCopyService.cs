using System;
using System.Threading;
using System.Threading.Tasks;

namespace FileManagement.Core
{
    public interface IFileCopyService
    {
        Task<long> CopyAsync(FileItem source, FileItem destination, 
            IProgress<long> progress, CancellationToken cancellationToken);
    }
}
