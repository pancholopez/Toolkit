using System.Diagnostics;
using System.Threading.Tasks;
using FileManagement.Core;

namespace FileManagement
{
    public class AuditableCopyServiceDecorator : FileCopyServiceBase
    {
        private readonly IFileCopyService _fileCopyService;

        public AuditableCopyServiceDecorator(IFileCopyService fileCopyService)
        {
            _fileCopyService = fileCopyService;
        }

        public override async Task<T> CopyAsync<T>(FileItem source, FileItem destination)
        {
            var stopwatch = Stopwatch.StartNew();
            var summary = await _fileCopyService.CopyAsync<T>(source, destination);
            stopwatch.Stop();
            return CopyStats.Create(summary, stopwatch.Elapsed) as T;
        }
    }
}