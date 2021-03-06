[assembly: System.CLSCompliant(false)]
namespace ImageDownloader
{
    using System;
    using System.Diagnostics;
    using System.Linq;
    using System.Net;
    using System.Threading.Tasks;

    internal class Program
    {
        private static readonly object consoleLock = new ();

        private static void Main()
        {
            WebpageProvider downloader = new ();
            ImageCollector collector = new (downloader);

            System.Collections.Generic.IList<ImageData> images = collector.GetImages(new Uri(@"http://users.nik.uni-obuda.hu/cseri/zh2_gyakorlo/simplepage.html"));

            int imageCount = images.Count;
            Task[] tasks = new Task[imageCount];
            for (int i = 0; i < imageCount; i++)
            {
                DownloadData data = new ()
                {
                    RowIndex = i,
                    ImageData = images[i],
                    Color = i % 2 == 0 ? ConsoleColor.Red : ConsoleColor.Yellow
                };

                tasks[i] = new Task(() => Download(data));
                tasks[i].Start();
            }

            Task.WaitAll(tasks.ToArray());
            foreach (var image in images)
            {
                Process.Start(new ProcessStartInfo(fileName: image.FileName) { UseShellExecute = true });
            }

            Console.ReadLine();
        }

        static void Download(object data)
        {
            DownloadData downloadData = (DownloadData)data;

            string firstMessage = string.Empty;
            lock (consoleLock)
            {
                Console.ForegroundColor = downloadData.Color;
                Console.CursorTop = downloadData.RowIndex;
                Console.CursorLeft = 0;
                firstMessage = "Downloading " + downloadData.ImageData.URL + "... ";
                Console.Write(firstMessage);
            }

            string secondMessage = "Success";
            WebClient webClient = new ();
            try
            {
                webClient.DownloadFile(downloadData.ImageData.URL, downloadData.ImageData.FileName);
            }
            catch (ArgumentNullException ex)
            {
                secondMessage = ex.Message;
            }

            lock (consoleLock)
            {
                Console.ForegroundColor = downloadData.Color;
                Console.CursorTop = downloadData.RowIndex;
                Console.CursorLeft = firstMessage.Length;
                Console.WriteLine(secondMessage);
            }

            webClient.Dispose();
        }
    }
}
