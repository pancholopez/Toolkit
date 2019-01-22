using System;
using System.Threading;
using System.Threading.Tasks;

namespace FileManagement.Core
{
    /// <summary>
    /// The responsibility of the copy service is to copy one file from one location to another.
    /// </summary>
    public interface IFileCopyService
    {
        /// <summary>
        /// Copy a file from one location to another
        /// </summary>
        /// <param name="source">source file path</param>
        /// <param name="destination">destination file path</param>
        /// <param name="progress">file copy progress report</param>
        /// <param name="cancellationToken">task cancellation token</param>
        /// <returns>Total bytes copied</returns>
        Task<long> CopyAsync(FileItem source, FileItem destination, 
            IProgress<long> progress, CancellationToken cancellationToken);
    }
}
