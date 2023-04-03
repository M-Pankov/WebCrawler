using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using WebCrawler.Logic.Crawlers;
using WebCrawler.Logic.Models;
using WebCrawler.Logic.Parsers;
using WebCrawler.Logic.Services;
using WebCrawler.Logic.Validators;
using Xunit;

namespace WebCrawler.Logic.Tests.Crawlers;

public class SiteCrawlerTests
{
    private readonly SiteCrawler _crawler;
    private readonly HttpClient _httpClient;
    private readonly Mock<HtmlParser> _htmlParser;
    private readonly Mock<HtmlLoaderService> _htmlLoader;
    private readonly Mock<UrlValidator> _urlValidator;
    public SiteCrawlerTests()
    {
        _httpClient = new HttpClient();
        _htmlLoader = new Mock<HtmlLoaderService>(_httpClient);
        _htmlParser = new Mock<HtmlParser>();
        _urlValidator = new Mock<UrlValidator>();
        _crawler = new SiteCrawler(_htmlParser.Object, _urlValidator.Object, _htmlLoader.Object);
    }

    [Fact]
    public async void CrawlSiteAsync_Url_SholdReturnUrls()
    {
        var testStartUrl = new Uri("https://www.litedb.org/");

        var testUrls = HtmlParserTestData();

        _htmlLoader.Setup(x => x.GetHttpResponseAsync(It.IsAny<Uri>()))
            .ReturnsAsync(new HttpResponse()
            {
                ResponseTime = 20
            });

        _htmlParser.SetupSequence(x => x.GetLinks(It.IsAny<Uri>(), It.IsAny<string>()))
            .Returns(testUrls)
            .Returns(new List<Uri>())
            .Returns(new List<Uri>())
            .Returns(new List<Uri>());

        _urlValidator.Setup(x => x.IsAllowed(It.IsAny<Uri>(), It.IsAny<Uri>())).Returns(true);


        var result = await _crawler.CrawlSiteAsync(testStartUrl);

        _htmlParser.Verify(x => x.GetLinks(It.IsAny<Uri>(), It.IsAny<string>()), Times.Exactly(4));
        _htmlLoader.Verify(x => x.GetHttpResponseAsync(It.IsAny<Uri>()), Times.Exactly(4));
        Assert.Equal(4, result.Count());
    }

    [Fact]
    public async void CrawlSiteAsync_Url_SholdReturnUrlsWithResponseTime()
    {
        var testStartUrl = new Uri("https://www.litedb.org/");

        var testUrls = HtmlParserTestData();

        _htmlLoader.Setup(x => x.GetHttpResponseAsync(It.IsAny<Uri>()))
            .ReturnsAsync(new HttpResponse()
            {
                ResponseTime = 20
            });

        _htmlParser.SetupSequence(x => x.GetLinks(It.IsAny<Uri>(), It.IsAny<string>()))
            .Returns(testUrls)
            .Returns(new List<Uri>())
            .Returns(new List<Uri>())
            .Returns(new List<Uri>());

        _urlValidator.Setup(x => x.IsAllowed(It.IsAny<Uri>(), It.IsAny<Uri>())).Returns(true);


        var result = await _crawler.CrawlSiteAsync(testStartUrl);

        Assert.True(result.All(x => x.ResponseTime != null));
    }

    [Fact]
    public async void CrawlSiteAsync_Url_SholdReturnUrlsWithExpectedFoundLocation()
    {
        var testStartUrl = new Uri("https://www.litedb.org/");

        var testUrls = HtmlParserTestData();

        _htmlLoader.Setup(x => x.GetHttpResponseAsync(It.IsAny<Uri>()))
            .ReturnsAsync(new HttpResponse()
            {
                ResponseTime = 20
            });

        _htmlParser.SetupSequence(x => x.GetLinks(It.IsAny<Uri>(), It.IsAny<string>()))
            .Returns(testUrls)
            .Returns(new List<Uri>())
            .Returns(new List<Uri>())
            .Returns(new List<Uri>());

        _urlValidator.Setup(x => x.IsAllowed(It.IsAny<Uri>(), It.IsAny<Uri>())).Returns(true);


        var result = await _crawler.CrawlSiteAsync(testStartUrl);

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
