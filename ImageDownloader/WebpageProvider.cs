namespace ImageDownloader
{
    public class WebpageProvider : IWebpageProvider
    {
        public System.Xml.Linq.XDocument Download(string url)
        {
            // or can be done with WebClient 
            return System.Xml.Linq.XDocument.Load(url);
        }

        public System.Xml.Linq.XDocument Download(System.Uri uri)
        {
            if (uri is null) throw new System.ArgumentNullException(nameof(uri));
            return System.Xml.Linq.XDocument.Load(uri.AbsoluteUri);
        }
    }
}
