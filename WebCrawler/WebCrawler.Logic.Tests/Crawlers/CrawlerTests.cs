using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using WebCrawler.Logic.Crawlers;
using WebCrawler.Logic.Enums;
using WebCrawler.Logic.Loaders;
using WebCrawler.Logic.Models;
using WebCrawler.Logic.Parsers;
using WebCrawler.Logic.Validators;
using WebCrawler.Logic.Wrappers;
using Xunit;

namespace WebCrawler.Logic.Tests.Crawlers;

public class CrawlerTests
{
    private readonly Mock<SitemapLoaderWrapper> _sitemapLoader;
    private readonly Mock<HttpClientWrapper> _httpClient;
    private readonly Mock<HtmlParser> _htmlParser;
    private readonly Mock<UrlValidator> _urlValidator;
    private readonly Mock<SiteCrawler> _siteCrawler;
    private readonly Mock<SitemapCrawler> _sitemapCrawler;
    private readonly Mock<HtmlLoader> _htmlLoader;
    private readonly Crawler _crawler;
    public CrawlerTests()
    {
        _sitemapLoader = new Mock<SitemapLoaderWrapper>();
        _httpClient = new Mock<HttpClientWrapper>();
        _htmlParser = new Mock<HtmlParser>();
        _urlValidator = new Mock<UrlValidator>();
        _htmlLoader = new Mock<HtmlLoader>(_httpClient.Object);
        _sitemapCrawler = new Mock<SitemapCrawler>(_sitemapLoader.Object);
        _siteCrawler = new Mock<SiteCrawler>(_htmlParser.Object, _urlValidator.Object, _htmlLoader.Object);
        _crawler = new Crawler(_siteCrawler.Object, _sitemapCrawler.Object, _htmlLoader.Object);
    }

    [Fact]
    public async void FindAllPagesWithResponseTime_Url_UrlsWithResponseTime()
    {
        var testUrl = new Uri("https://www.litedb.org/");

        var siteCrawlerTestData = GetSiteCrawlerTestData();

        var sitemapCrawlerTestData = GetSitemapCrawlerTestData();

        _htmlLoader.Setup(x => x.GetHtmlContentWithResponseTimeAsync(It.IsAny<Uri>())).ReturnsAsync(new HtmlContentWithResponseTime() { ResponseTime = 20 });

        _siteCrawler.Setup(x => x.GetUrlsWithResponseTimeAsync(testUrl)).ReturnsAsync(siteCrawlerTestData);

        _sitemapCrawler.Setup(x => x.GetLinksFromSitemapAsync(testUrl)).ReturnsAsync(sitemapCrawlerTestData);

        var result = await _crawler.FindAllPagesWithResponseTime(testUrl);

        Assert.Equal(4, result.Count());
        Assert.True(result.All(x => x.ResponseTime == 20));
        Assert.True(result.Where(x => x.UrlFoundLocation == UrlFoundLocation.SiteAndSitemap).Count() == 2);
        Assert.True(result.Where(x => x.UrlFoundLocation == UrlFoundLocation.Site).Count() == 1);
        Assert.True(result.Where(x => x.UrlFoundLocation == UrlFoundLocation.Sitemap).Count() == 1);
    }

    private IEnumerable<UrlWithResponseTime> GetSiteCrawlerTestData()
    {
        return new List<UrlWithResponseTime>
        {
            new UrlWithResponseTime(){ Url = new Uri("https://www.litedb.org/"), UrlFoundLocation = UrlFoundLocation.Site},
            new UrlWithResponseTime(){ Url = new Uri("https://www.litedb.org/docs"), UrlFoundLocation = UrlFoundLocation.Site},
            new UrlWithResponseTime(){ Url = new Uri("https://www.litedb.org/api"), UrlFoundLocation = UrlFoundLocation.Site}
        };
    }

    private IEnumerable<UrlWithResponseTime> GetSitemapCrawlerTestData()
    {
        return new List<UrlWithResponseTime>
        {
            new UrlWithResponseTime(){ Url = new Uri("https://www.litedb.org/"), UrlFoundLocation = UrlFoundLocation.Sitemap},
            new UrlWithResponseTime(){ Url = new Uri("https://www.litedb.org/docs/getting-started"), UrlFoundLocation = UrlFoundLocation.Sitemap},
            new UrlWithResponseTime(){ Url = new Uri("https://www.litedb.org/api"), UrlFoundLocation = UrlFoundLocation.Sitemap}
        };
    }
}
