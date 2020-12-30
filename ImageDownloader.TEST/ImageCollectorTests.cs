namespace ImageDownloader.TEST
{
    using Moq;
    using NUnit.Framework;
    using System.Collections.Generic;
    using System.Xml.Linq;

    [TestFixture]
    public class ImageCollectorTests
    {
        [Test]
        public void WhenGettingImages_AndThereAreImgTags_CollectsAll()
        {
            XDocument page = XDocument.Parse("<html>" +
                                                "<body>" +
                                                    "<img src=\"teszt1.jpg\" />" +
                                                    "<img src=\"teszt2.jpg\" />" +
                                                "</body>" +
                                             "</html>");
            string testUrl = "http://any/page/anypage.html";

            Mock<IWebpageProvider> providerMock = new Mock<IWebpageProvider>();
            providerMock.Setup(m => m.Download(testUrl)).Returns(page);

            ImageCollector collector = new ImageCollector(providerMock.Object);
            IList<ImageData> result = collector.GetImages(testUrl);

            Assert.That(result.Count, Is.EqualTo(2));
            Assert.That(result[0].FileName, Is.EqualTo("teszt1.jpg"));
            Assert.That(result[0].URL, Is.EqualTo("http://any/page/teszt1.jpg"));
            Assert.That(result[1].FileName, Is.EqualTo("teszt2.jpg"));
            Assert.That(result[1].URL, Is.EqualTo("http://any/page/teszt2.jpg"));
        }
    }
}
