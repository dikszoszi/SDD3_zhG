using Moq;
using NUnit.Framework;
using System.Collections.Generic;
using System.Xml.Linq;

[assembly: System.CLSCompliant(false)]
namespace ImageDownloader.TEST
{
    [TestFixture]
    public class ImageCollectorTests
    {
        [Test]
        public void WhenGettingImagesAndThereAreImgTagsCollectsAll()
        {
            XDocument page = XDocument.Parse("<html>" +
                                                "<body>" +
                                                    "<img src=\"teszt1.jpg\" />" +
                                                    "<img src=\"teszt2.jpg\" />" +
                                                "</body>" +
                                             "</html>");
            string testUrl = "http://any/page/anypage.html";
            System.Uri testUri = new ("http://any/page/anypage.html");

            Mock<IWebpageProvider> providerMock1 = new ();
            providerMock1.Setup(m => m.Download(testUri)).Returns(page);

            Mock<IWebpageProvider> providerMock2 = new();
            providerMock2.Setup(m => m.Download(testUrl)).Returns(page);

            ImageCollector collector1 = new (providerMock1.Object);
            IList<ImageData> result1 = collector1.GetImages(testUri);
            ImageCollector collector2 = new(providerMock2.Object);
            IList<ImageData> result2 = collector2.GetImages(testUrl);

            Assert.IsTrue(result1.Count == result2.Count && result1.Count + result2.Count == 4);
            Assert.That(result1[0].FileName, Is.EqualTo("teszt1.jpg"));
            Assert.That(result2[0].URL, Is.EqualTo("http://any/page/teszt1.jpg"));
            Assert.That(result2[1].FileName, Is.EqualTo("teszt2.jpg"));
            Assert.That(result1[1].URL, Is.EqualTo("http://any/page/teszt2.jpg"));
        }
    }
}
