using Louw.SitemapParser;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebCrawler.Application.Crawlers;
using WebCrawler.Application.Loaders;
using WebCrawler.Domain.Enums;
using Xunit;

namespace WebCrawler.Application.Tests.Crawlers;

public class SiteMapCrawlerTests
{
    private readonly SiteMapCrawler _siteMapCrawler;
    private readonly Mock<SiteMapLoader> _siteMapLoader;
    public SiteMapCrawlerTests()
    {
        _siteMapLoader = new Mock<SiteMapLoader>();
        _siteMapCrawler = new SiteMapCrawler(_siteMapLoader.Object);
    }

    [Fact]
    public async Task CrawlSitemapAsync_Url_ShouldReturnExpectedUrls()
    {
        var testBaseUrl = new Uri("https://jwt.io/");

        var testSitemapData = SitemapLoaderTestData();

        _siteMapLoader.Setup(x => x.LoadAsync(It.IsAny<Uri>())).ReturnsAsync(testSitemapData);

        var result = await _siteMapCrawler.CrawlSitemapAsync(testBaseUrl);

        Assert.Equal(3, result.Count());
        Assert.Contains(result, x => x.Url == new Uri("https://jwt.io/"));
        Assert.Contains(result, x => x.Url == new Uri("https://jwt.io/libraries"));
        Assert.Contains(result, x => x.Url == new Uri("https://jwt.io/introduction"));
    }

    [Fact]
    public async Task CrawlSitemapAsync_Url_ShouldSetCorrectFoundLocation()
    {
        var testBaseUrl = new Uri("https://jwt.io/");

        var testSitemap = SitemapLoaderTestData();

        _siteMapLoader.Setup(x => x.LoadAsync(It.IsAny<Uri>())).ReturnsAsync(testSitemap);

        var result = await _siteMapCrawler.CrawlSitemapAsync(testBaseUrl);

        Assert.True(result.All(x => x.UrlFoundLocation == UrlFoundLocation.Sitemap));
    }

    [Fact]
    public async Task CrawlSitemapAsync_Url_ShouldNotSetResponseTime()
    {
        var testBaseUrl = new Uri("https://jwt.io/");

        var testSitemap = SitemapLoaderTestData();

        _siteMapLoader.Setup(x => x.LoadAsync(It.IsAny<Uri>())).ReturnsAsync(testSitemap);

        var result = await _siteMapCrawler.CrawlSitemapAsync(testBaseUrl);

        Assert.True(result.All(x => !x.ResponseTimeMs.HasValue));
    }

    private Sitemap SitemapLoaderTestData()
    {
        var testSitemapItems = new List<SitemapItem>
        {
            new SitemapItem(new Uri("https://jwt.io/")),
            new SitemapItem(new Uri("https://jwt.io/libraries/")),
            new SitemapItem(new Uri("https://jwt.io/introduction/"))
        };

        return new Sitemap(testSitemapItems);
    }
}
