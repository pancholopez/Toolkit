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

            Console.Write("program ended.");
            Console.ReadKey();
        }

        private static async Task MainAsync()
        {
            var settings = new RetrySettings(3, 2000);
            var retry = new RetryService(settings);
            IFileCopyService service = new StreamCopyService(retry);
            var source = FileItem.Create(SourceFilePath);
            var destination = FileItem.Create(DestinationFilePath);

            

            void PrintProgress(long bytesWritten)
            {
                var remainingBytes = source.SizeInBytes - bytesWritten;
                Console.WriteLine(
                    $"Total: {source.SizeInBytes} - Written: {bytesWritten} - Remaining: {remainingBytes}");
            }

            var progressIndicator = new Progress<long>(PrintProgress);
            var cst = new CancellationTokenSource();

            var stopwatch = Stopwatch.StartNew();

            try
            {
                await service.CopyAsync(source, destination, progressIndicator, cst.Token)
                    .ContinueWith(task => Console.WriteLine("Copy file completed!"), cst.Token);
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception.Message);
            }
            finally
            {
                stopwatch.Stop();
            }

            Console.WriteLine($"Elapsed Milliseconds: {stopwatch.ElapsedMilliseconds}");
        }
    }
}
