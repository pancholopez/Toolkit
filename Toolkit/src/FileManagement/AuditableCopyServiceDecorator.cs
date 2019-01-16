using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using FileManagement.Core;

namespace FileManagement
{
    public class AuditableCopyServiceDecorator //: IFileCopyService
    {
        private readonly IFileCopyService _fileCopyService;

        public AuditableCopyServiceDecorator(IFileCopyService fileCopyService)
        {
            _fileCopyService = fileCopyService;
        }

        public async Task<T> CopyAsync<T>(FileItem source, FileItem destination) where T : CopySummary
        {
            var stopwatch = Stopwatch.StartNew();
            await _fileCopyService.CopyAsync(source, destination, null, CancellationToken.None);
            stopwatch.Stop();
            return CopyStats.Create(null, stopwatch.Elapsed) as T;
        }
    }
}