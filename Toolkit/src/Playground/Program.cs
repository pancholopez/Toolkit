using System;
using System.Diagnostics;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using FileManagement;
using FileManagement.Core;

namespace Playground
{
    class Program
    {
        private const string SourceFilePath = @"C:\Users\francisco.lopez2\Desktop\deleteme\test.files\dummy.txt";
        private const string DestinationFilePath = @"C:\Users\francisco.lopez2\Desktop\deleteme\test.files\dummy.copy";

        static void Main(string[] args)
        {
            File.Delete(DestinationFilePath);
            Thread.Sleep(1000);

            MainAsync().Wait();

            Console.WriteLine("program ended.");
            Console.ReadKey();
        }

        private static async Task MainAsync()
        {
            IFileCopyService service = new StreamCopyService();
            var source = FileItem.Create(SourceFilePath);
            var destination = FileItem.Create(DestinationFilePath);

            void PrintProgress(int bytesWritten)
            {
                Console.WriteLine(
                    $"Total: {source.SizeInBytes} - Written: {bytesWritten} - Remaining: {source.SizeInBytes - bytesWritten}");
            }

            var progressIndicator = new Progress<int>(PrintProgress);
            var stopwatch = Stopwatch.StartNew();


            await service.CopyAsync<CopySummary>(source, destination, progressIndicator);
            stopwatch.Stop();

            await Task.Delay(100);
            Console.WriteLine($"Elapsed Milliseconds: {stopwatch.ElapsedMilliseconds}");
        }
    }
}
