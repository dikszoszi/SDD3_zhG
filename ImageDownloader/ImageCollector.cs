namespace ImageDownloader
{
    using System.Linq;

    public class ImageCollector
    {
        private readonly IWebpageProvider webpageProvider;

        public ImageCollector(IWebpageProvider webpageProvider)
        {
            this.webpageProvider = webpageProvider;
        }

        public System.Collections.Generic.IList<ImageData> GetImages(string url)
        {
            System.Xml.Linq.XDocument pageText = webpageProvider.Download(url);

            string pathOfImages = url.Substring(0, url.LastIndexOf('/') + 1);
            return pageText.Root
                .Element("body")
                .Elements("img")
                .Select(imgElement => new ImageData()
                {
                    FileName = imgElement.Attribute("src").Value,
                    URL = pathOfImages + imgElement.Attribute("src").Value
                }).ToList();
        }
    }
}
