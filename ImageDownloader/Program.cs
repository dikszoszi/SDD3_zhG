namespace ImageDownloader
{
    using System;
    using System.Diagnostics;
    using System.Linq;
    using System.Net;
    using System.Threading.Tasks;

    class Program
    {
        private static readonly object consoleLock = new object();

        static void Main(string[] args)
        {
            WebpageProvider downloader = new WebpageProvider();
            ImageCollector collector = new ImageCollector(downloader);

            System.Collections.Generic.IList<ImageData> images = collector.GetImages(@"http://users.nik.uni-obuda.hu/cseri/zh2_gyakorlo/simplepage.html");

            int imageCount = images.Count();
            Task[] tasks = new Task[imageCount];
            for (int i = 0; i < imageCount; i++)
            {
                DownloadData data = new DownloadData()
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
                Process p = new Process();
                p.StartInfo.FileName = image.FileName;
                p.StartInfo.UseShellExecute = true;
                p.Start();
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
            WebClient webClient = new WebClient();
            try
            {
                webClient.DownloadFile(downloadData.ImageData.URL, downloadData.ImageData.FileName);
            }
            catch (Exception ex)
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
        }
    }
}
