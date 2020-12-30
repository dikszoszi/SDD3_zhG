namespace ImageDownloader
{
    public class WebpageProvider : IWebpageProvider
    {
        public System.Xml.Linq.XDocument Download(string url)
        {
            // or can be done with WebClient 
            return System.Xml.Linq.XDocument.Load(url);
        }
    }
}
