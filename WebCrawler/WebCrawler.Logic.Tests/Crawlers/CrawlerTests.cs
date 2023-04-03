using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using WebCrawler.Logic.Crawlers;
using WebCrawler.Logic.Enums;
using WebCrawler.Logic.Models;
using WebCrawler.Logic.Parsers;
using WebCrawler.Logic.Services;
using WebCrawler.Logic.Validators;
using Xunit;

namespace WebCrawler.Logic.Tests.Crawlers;

public class CrawlerTests
{
    private readonly Mock<SitemapLoaderService> _sitemapLoader;
    private readonly HttpClient _httpClient;
    private readonly Mock<HtmlParser> _htmlParser;
    private readonly Mock<UrlValidator> _urlValidator;
    private readonly Mock<SiteCrawler> _siteCrawler;
    private readonly Mock<SitemapCrawler> _sitemapCrawler;
    private readonly Mock<HtmlLoaderService> _htmlLoader;
    private readonly Crawler _crawler;
    public CrawlerTests()
    {
        _sitemapLoader = new Mock<SitemapLoaderService>();
        _httpClient = new HttpClient();
        _htmlParser = new Mock<HtmlParser>();
        _urlValidator = new Mock<UrlValidator>();
        _htmlLoader = new Mock<HtmlLoaderService>(_httpClient);
        _sitemapCrawler = new Mock<SitemapCrawler>(_sitemapLoader.Object);
        _siteCrawler = new Mock<SiteCrawler>(_htmlParser.Object, _urlValidator.Object, _htmlLoader.Object);
        _crawler = new Crawler(_siteCrawler.Object, _sitemapCrawler.Object, _htmlLoader.Object);
    }

    [Fact]
    public async Task CrawlUrlAsync_Url_ShouldReturnUrls()
    {
        var testUrl = new Uri("https://www.litedb.org/");

        var siteCrawlerTestData = GetSiteCrawlerTestData();

        var sitemapCrawlerTestData = GetSitemapCrawlerTestData();

        _htmlLoader.Setup(x => x.GetHttpResponseAsync(It.IsAny<Uri>())).ReturnsAsync(new HttpResponse { ResponseTime = 20 });

        _siteCrawler.Setup(x => x.CrawlSiteAsync(testUrl)).ReturnsAsync(siteCrawlerTestData);

        _sitemapCrawler.Setup(x => x.CrawlSitemapAsync(testUrl)).ReturnsAsync(sitemapCrawlerTestData);

        var result = await _crawler.CrawlUrlAsync(testUrl);

        _htmlLoader.Verify(x => x.GetHttpResponseAsync(It.IsAny<Uri>()), Times.Once);
        _siteCrawler.Verify(x => x.CrawlSiteAsync(testUrl), Times.Once);
        _sitemapCrawler.Verify(x => x.CrawlSitemapAsync(testUrl), Times.Once);
        Assert.NotNull(result);
        Assert.Equal(4, result.Count());
    }

    [Fact]
    public async Task CrawlUrlAsync_Url_ShouldReturnUrlsWithResponseTime()
    {
        var testUrl = new Uri("https://www.litedb.org/");

        var siteCrawlerTestData = GetSiteCrawlerTestData();

        var sitemapCrawlerTestData = GetSitemapCrawlerTestData();

        _htmlLoader.Setup(x => x.GetHttpResponseAsync(It.IsAny<Uri>())).ReturnsAsync(new HttpResponse { ResponseTime = 20 });

        _siteCrawler.Setup(x => x.CrawlSiteAsync(testUrl)).ReturnsAsync(siteCrawlerTestData);

        _sitemapCrawler.Setup(x => x.CrawlSitemapAsync(testUrl)).ReturnsAsync(sitemapCrawlerTestData);

        var result = await _crawler.CrawlUrlAsync(testUrl);

        Assert.True(result.All(x => x.ResponseTime != null));
    }

    [Fact]
    public async Task CrawlUrlAsync_Url_ShouldReturnUrlsWithExpectedFoundLocations()
    {
        var testUrl = new Uri("https://www.litedb.org/");

        var siteCrawlerTestData = GetSiteCrawlerTestData();

        var sitemapCrawlerTestData = GetSitemapCrawlerTestData();

        _htmlLoader.Setup(x => x.GetHttpResponseAsync(It.IsAny<Uri>())).ReturnsAsync(new HttpResponse { ResponseTime = 20 });

        _siteCrawler.Setup(x => x.CrawlSiteAsync(testUrl)).ReturnsAsync(siteCrawlerTestData);

        _sitemapCrawler.Setup(x => x.CrawlSitemapAsync(testUrl)).ReturnsAsync(sitemapCrawlerTestData);

        var result = await _crawler.CrawlUrlAsync(testUrl);

        Assert.True(result.Where(x => x.UrlFoundLocation == UrlFoundLocation.Both).Count() == 2);
        Assert.True(result.Where(x => x.UrlFoundLocation == UrlFoundLocation.Site).Count() == 1);
        Assert.True(result.Where(x => x.UrlFoundLocation == UrlFoundLocation.Sitemap).Count() == 1);
    }

    private IEnumerable<CrawledUrl> GetSiteCrawlerTestData()
    {
        return new List<CrawledUrl>
        {
            new CrawledUrl(){ Url = new Uri("https://www.litedb.org/"), UrlFoundLocation = UrlFoundLocation.Site , ResponseTime = 20},
            new CrawledUrl(){ Url = new Uri("https://www.litedb.org/docs"), UrlFoundLocation = UrlFoundLocation.Site , ResponseTime = 20},
            new CrawledUrl(){ Url = new Uri("https://www.litedb.org/api"), UrlFoundLocation = UrlFoundLocation.Site , ResponseTime = 20}
        };
    }

    private IEnumerable<CrawledUrl> GetSitemapCrawlerTestData()
    {
        return new List<CrawledUrl>
        {
            new CrawledUrl(){ Url = new Uri("https://www.litedb.org/"), UrlFoundLocation = UrlFoundLocation.Sitemap},
            new CrawledUrl(){ Url = new Uri("https://www.litedb.org/docs/getting-started"), UrlFoundLocation = UrlFoundLocation.Sitemap},
            new CrawledUrl(){ Url = new Uri("https://www.litedb.org/api"), UrlFoundLocation = UrlFoundLocation.Sitemap}
        };
    }
}
