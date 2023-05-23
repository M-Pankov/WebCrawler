using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using WebCrawler.Crawlers.Loaders;
using WebCrawler.Crawlers.Parsers;
using WebCrawler.Crawlers.SubCrawlers;
using WebCrawler.Crawlers.Validators;
using WebCrawler.Domain.CrawlerModels;
using WebCrawler.Domain.Enums;
using Xunit;

namespace WebCrawler.Crawlers.Tests.SubCrawlers;

public class SiteCrawlerTests
{
    private readonly SiteCrawler _crawler;
    private readonly HttpClient _httpClient;
    private readonly Mock<HtmlParser> _htmlParser;
    private readonly Mock<HtmlLoader> _htmlLoader;
    private readonly Mock<UrlValidator> _urlValidator;
    public SiteCrawlerTests()
    {
        _httpClient = new HttpClient();
        _htmlLoader = new Mock<HtmlLoader>(_httpClient);
        _htmlParser = new Mock<HtmlParser>();
        _urlValidator = new Mock<UrlValidator>();
        _crawler = new SiteCrawler(_htmlParser.Object, _urlValidator.Object, _htmlLoader.Object);
    }

    [Fact]
    public async void CrawlSiteAsync_Url_ShouldReturnExpectedUrls()
    {
        var testStartUrl = new Uri("https://www.litedb.org/");

        SetupMockObjects();

        var result = await _crawler.CrawlSiteAsync(testStartUrl);

        _htmlParser.Verify(x => x.GetLinks(It.IsAny<Uri>(), It.IsAny<string>()), Times.Exactly(4));
        _htmlLoader.Verify(x => x.GetHttpResponseAsync(It.IsAny<Uri>()), Times.Exactly(4));
        Assert.Equal(4, result.Count());
        Assert.Contains(result, x => x.Url == new Uri("https://www.litedb.org/docs"));
        Assert.Contains(result, x => x.Url == new Uri("https://www.litedb.org/api"));
        Assert.Contains(result, x => x.Url == new Uri("https://www.litedb.org/docs/getting-started"));
    }

    [Fact]
    public async void CrawlSiteAsync_Url_ShouldSetResponseTime()
    {
        var testStartUrl = new Uri("https://www.litedb.org/");

        var testUrls = HtmlParserTestData();

        SetupMockObjects();

        var result = await _crawler.CrawlSiteAsync(testStartUrl);

        Assert.True(result.All(x => x.ResponseTimeMs.HasValue));
    }

    [Fact]
    public async void CrawlSiteAsync_Url_ShouldSetCorrectFoundLocation()
    {
        var testStartUrl = new Uri("https://www.litedb.org/");

        var testUrls = HtmlParserTestData();

        SetupMockObjects();

        var result = await _crawler.CrawlSiteAsync(testStartUrl);

        Assert.True(result.All(x => x.UrlFoundLocation == UrlFoundLocation.Site));
    }

    private void SetupMockObjects()
    {
        var testUrls = HtmlParserTestData();

        _htmlLoader.Setup(x => x.GetHttpResponseAsync(It.IsAny<Uri>()))
            .ReturnsAsync(new HttpResponse()
            {
                ResponseTimeMs = 20
            });

        _htmlParser.SetupSequence(x => x.GetLinks(It.IsAny<Uri>(), It.IsAny<string>()))
            .Returns(testUrls)
            .Returns(new List<Uri>())
            .Returns(new List<Uri>())
            .Returns(new List<Uri>());

        _urlValidator.Setup(x => x.IsAllowed(It.IsAny<Uri>(), It.IsAny<Uri>())).Returns(true);
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
