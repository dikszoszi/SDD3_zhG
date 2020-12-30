namespace ImageDownloader
{
    public interface IWebpageProvider
    {
        System.Xml.Linq.XDocument Download(string url);
    }
}
