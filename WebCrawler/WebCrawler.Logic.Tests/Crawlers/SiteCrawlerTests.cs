using Castle.Components.DictionaryAdapter.Xml;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using WebCrawler.Logic.Crawlers;
using WebCrawler.Logic.Loaders;
using WebCrawler.Logic.Models;
using WebCrawler.Logic.Parsers;
using WebCrawler.Logic.Validators;
using WebCrawler.Logic.Wrappers;
using Xunit;

namespace WebCrawler.Logic.Tests.Crawlers;

public class SiteCrawlerTests
{
    private readonly SiteCrawler _crawler;
    private readonly Mock<HttpClientWrapper> _httpClient;
    private readonly Mock<HtmlParser> _htmlParser;
    private readonly Mock<HtmlLoader> _htmlLoader;
    private readonly Mock<UrlValidator> _urlValidator;
    public SiteCrawlerTests()
    {
        _httpClient = new Mock<HttpClientWrapper>();
        _htmlLoader = new Mock<HtmlLoader>(_httpClient.Object);
        _htmlParser = new Mock<HtmlParser>();
        _urlValidator = new Mock<UrlValidator>();
        _crawler = new SiteCrawler(_htmlParser.Object, _urlValidator.Object, _htmlLoader.Object);
    }

    [Fact]
    public async void GetUrlsWithResponseTimeAsync_Url_SholdReturnUrlWithResponseTime()
    {
        var testStartUrl = new Uri("https://www.litedb.org/");

        var testUrls = HtmlParserTestData();

        _htmlLoader.Setup(x => x.GetHtmlContentWithResponseTimeAsync(It.IsAny<Uri>()))
            .ReturnsAsync( new HtmlContentWithResponseTime()
            {
                ResponseTime = 20
            });

        _htmlParser.SetupSequence(x => x.GetLinks(It.IsAny<Uri>(), It.IsAny<string>()))
            .Returns(testUrls)
            .Returns(new List<Uri>())
            .Returns(new List<Uri>())
            .Returns(new List<Uri>());

        _urlValidator.Setup(x => x.IsAllowed(It.IsAny<Uri>(), It.IsAny<Uri>())).Returns(true);


        var result = await _crawler.GetUrlsWithResponseTimeAsync(testStartUrl);

        _htmlParser.Verify(x => x.GetLinks(It.IsAny<Uri>(),It.IsAny<string>()),Times.Exactly(4));
        _htmlLoader.Verify(x => x.GetHtmlContentWithResponseTimeAsync(It.IsAny<Uri>()), Times.Exactly(4));
        Assert.Equal(4, result.Count());
        Assert.True(result.All(x => x.ResponseTime == 20));
        Assert.True(result.All(x => x.UrlFoundLocation == Enums.UrlFoundLocation.Site));
    }

    private IEnumerable<Uri> HtmlParserTestData()
    {
        return new List<Uri>
        {
            new Uri("https://www.litedb.org/docs"),
            new Uri("https://www.litedb.org/api"),
            new Uri("https://www.litedb.org/docs/getting-started")
        };
    }
}
