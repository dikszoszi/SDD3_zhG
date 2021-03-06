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

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Usage", "CA2234:Pass system uri objects instead of strings", Justification = "<Pending>")]
        public System.Collections.Generic.IList<ImageData> GetImages(string url)
        {
            if (url is null) throw new System.ArgumentNullException(nameof(url));
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

        public System.Collections.Generic.IList<ImageData> GetImages(System.Uri uri)
        {
            if (uri is null) throw new System.ArgumentNullException(nameof(uri));
            System.Xml.Linq.XDocument pageText = webpageProvider.Download(uri);

            string pathOfImages = uri.OriginalString.Substring(0, uri.OriginalString.LastIndexOf('/') + 1);
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
