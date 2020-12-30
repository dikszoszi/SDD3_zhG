namespace ImageDownloader
{
    using System;

    public class DownloadData
    {
        public ImageData ImageData { get; set; }
        public int RowIndex { get; set; }
        public ConsoleColor Color { get; set; }
        public string FileName { get; set; }
    }
}
