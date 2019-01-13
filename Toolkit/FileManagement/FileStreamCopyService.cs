using System.IO;
using System.Threading.Tasks;
using FileManagement.Core;

namespace FileManagement
{
    public class FileStreamCopyService : IFileCopyService
    {
        public async Task<IFileCopySummary> CopyAsync(FileItem source, FileItem destination)
        {
            using (var input = new FileStream(source.FilePath, FileMode.Open, FileAccess.Read))
            {
                using (var output = new FileStream(destination.FilePath, FileMode.Create, FileAccess.Write))
                {
                    var buffer = new byte[32 * 1024];   //4k minimum
                    var bytesRead = 0;
                    do
                    {
                        bytesRead = await input.ReadAsync(buffer, 0, buffer.Length).ConfigureAwait(false);
                        await output.WriteAsync(buffer, 0, bytesRead).ConfigureAwait(false);
                    } while (bytesRead > 0);
                }
            }
            return FileCopySummary.Create(source, destination, source.SizeInBytes);
        }
    }
}