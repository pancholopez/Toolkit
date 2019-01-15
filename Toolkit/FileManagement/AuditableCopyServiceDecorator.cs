using System.Diagnostics;
using System.Threading.Tasks;
using FileManagement.Core;

namespace FileManagement
{
    public class AuditableCopyServiceDecorator : IFileCopyService
    {
        private readonly IFileCopyService _fileCopyService;

        public AuditableCopyServiceDecorator(IFileCopyService fileCopyService)
        {
            _fileCopyService = fileCopyService;
        }

        public async Task<T> CopyAsync<T>(FileItem source, FileItem destination) where T : CopySummary
        {
            var stopwatch = Stopwatch.StartNew();
            var summary = await _fileCopyService.CopyAsync<T>(source, destination);
            stopwatch.Stop();
            return (T)CopyStats.Create(summary, stopwatch.Elapsed);
        }
    }
}