using System;
using System.Threading;
using System.Threading.Tasks;

namespace FileManagement.Core
{
    public interface IFileCopyService
    {
        Task CopyAsync(FileItem source, FileItem destination, 
            IProgress<long> progress, CancellationToken cancellationToken);
    }
}
