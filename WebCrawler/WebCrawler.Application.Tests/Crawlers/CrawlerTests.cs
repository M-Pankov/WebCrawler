using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using WebCrawler.Application.Crawlers;
using WebCrawler.Application.Loaders;
using WebCrawler.Application.Parsers;
using WebCrawler.Application.Validators;
using WebCrawler.Domain.CrawlerModels;
using WebCrawler.Domain.CrawlResults;
using WebCrawler.Domain.Enums;
using Xunit;

namespace WebCrawler.Application.Tests.Crawlers;

public class CrawlerTests
{
    private readonly Mock<SiteMapLoader> _siteMapLoader;
    private readonly HttpClient _httpClient;
    private readonly Mock<HtmlParser> _htmlParser;
    private readonly Mock<UrlValidator> _urlValidator;
    private readonly Mock<SiteCrawler> _siteCrawler;
    private readonly Mock<SiteMapCrawler> _siteMapCrawler;
    private readonly Mock<HtmlLoader> _htmlLoader;
    private readonly Crawler _crawler;
    public CrawlerTests()
    {
        _siteMapLoader = new Mock<SiteMapLoader>();
        _httpClient = new HttpClient();
        _htmlParser = new Mock<HtmlParser>();
        _urlValidator = new Mock<UrlValidator>();
        _htmlLoader = new Mock<HtmlLoader>(_httpClient);
        _siteMapCrawler = new Mock<SiteMapCrawler>(_siteMapLoader.Object);
        _siteCrawler = new Mock<SiteCrawler>(_htmlParser.Object, _urlValidator.Object, _htmlLoader.Object);
        _crawler = new Crawler(_siteCrawler.Object, _siteMapCrawler.Object, _htmlLoader.Object);
    }

    [Fact]
    public async Task CrawlUrlsAsync_Url_ShouldReturnExpectedUrls()
    {
        var testUrl = new Uri("https://www.litedb.org/");

        SetupMockObjects();

        var result = await _crawler.CrawlUrlsAsync(testUrl);

        _htmlLoader.Verify(x => x.GetHttpResponseAsync(It.IsAny<Uri>()), Times.Once);
        _siteCrawler.Verify(x => x.CrawlSiteAsync(testUrl), Times.Once);
        _siteMapCrawler.Verify(x => x.CrawlSitemapAsync(testUrl), Times.Once);

        Assert.NotNull(result);
        Assert.Equal(4, result.Count());
        Assert.Contains(result, x => x.Url == new Uri("https://www.litedb.org/"));
        Assert.Contains(result, x => x.Url == new Uri("https://www.litedb.org/docs"));
        Assert.Contains(result, x => x.Url == new Uri("https://www.litedb.org/api"));
        Assert.Contains(result, x => x.Url == new Uri("https://www.litedb.org/docs/getting-started"));
    }

    [Fact]
    public async Task CrawlUrlsAsync_Url_ShouldSetResponseTime()
    {
        var testUrl = new Uri("https://www.litedb.org/");

        SetupMockObjects();

        var result = await _crawler.CrawlUrlsAsync(testUrl);

        Assert.True(result.All(x => x.ResponseTimeMs.HasValue));
    }

    [Fact]
    public async Task CrawlUrlsAsync_Url_ShouldSetCorrectFoundLocations()
    {
        var testUrl = new Uri("https://www.litedb.org/");

        SetupMockObjects();

        var result = await _crawler.CrawlUrlsAsync(testUrl);

        Assert.True(result.Where(x => x.UrlFoundLocation == UrlFoundLocation.Both).Count() == 2);
        Assert.True(result.Where(x => x.UrlFoundLocation == UrlFoundLocation.Site).Count() == 1);
        Assert.True(result.Where(x => x.UrlFoundLocation == UrlFoundLocation.Sitemap).Count() == 1);
    }

    private void SetupMockObjects()
    {
        var siteCrawlerTestData = GetSiteCrawlerTestData();

        var sitemapCrawlerTestData = GetSitemapCrawlerTestData();

        _htmlLoader.Setup(x => x.GetHttpResponseAsync(It.IsAny<Uri>())).ReturnsAsync(new HttpResponse { ResponseTimeMs = 20 });

        _siteCrawler.Setup(x => x.CrawlSiteAsync(It.IsAny<Uri>())).ReturnsAsync(siteCrawlerTestData);

        _siteMapCrawler.Setup(x => x.CrawlSitemapAsync(It.IsAny<Uri>())).ReturnsAsync(sitemapCrawlerTestData);
    }

    private IEnumerable<CrawledSiteUrl> GetSiteCrawlerTestData()
    {
        return new List<CrawledSiteUrl>
        {
            new CrawledSiteUrl(){ Url = new Uri("https://www.litedb.org/"), UrlFoundLocation = UrlFoundLocation.Site , ResponseTimeMs = 20},
            new CrawledSiteUrl(){ Url = new Uri("https://www.litedb.org/docs"), UrlFoundLocation = UrlFoundLocation.Site , ResponseTimeMs = 20},
            new CrawledSiteUrl(){ Url = new Uri("https://www.litedb.org/api"), UrlFoundLocation = UrlFoundLocation.Site , ResponseTimeMs = 20}
        };
    }

    private IEnumerable<CrawledSiteUrl> GetSitemapCrawlerTestData()
    {
        return new List<CrawledSiteUrl>
        {
            new CrawledSiteUrl(){ Url = new Uri("https://www.litedb.org/"), UrlFoundLocation = UrlFoundLocation.Sitemap},
            new CrawledSiteUrl(){ Url = new Uri("https://www.litedb.org/docs/getting-started"), UrlFoundLocation = UrlFoundLocation.Sitemap},
            new CrawledSiteUrl(){ Url = new Uri("https://www.litedb.org/api"), UrlFoundLocation = UrlFoundLocation.Sitemap}
        };
    }
}
