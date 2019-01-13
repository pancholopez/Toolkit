using System.Diagnostics;
using System.Threading.Tasks;
using FileManagement.Core;

namespace FileManagement
{
    public class AuditableFileCopyService : IFileCopyService
    {
        private readonly IFileCopyService _fileCopyService;

        public AuditableFileCopyService(IFileCopyService fileCopyService)
        {
            _fileCopyService = fileCopyService;
        }

        public async Task<IFileCopySummary> CopyAsync(FileItem source, FileItem destination)
        {
            var stopwatch = Stopwatch.StartNew();
            var summary = await _fileCopyService.CopyAsync(source, destination);
            stopwatch.Stop();
            return FileCopyStats.Create(summary, stopwatch.Elapsed);
        }
    }
}