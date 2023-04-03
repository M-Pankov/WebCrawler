using Louw.SitemapParser;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebCrawler.Logic.Crawlers;
using WebCrawler.Logic.Enums;
using WebCrawler.Logic.Models;
using WebCrawler.Logic.Wrappers;
using Xunit;

namespace WebCrawler.Logic.Tests.Crawlers;

public class SitemapCrawlerTests
{
    private readonly SitemapCrawler _sitemapCrawler;
    private readonly Mock<SitemapLoaderWrapper> _sitemapLoaderWrapper;
    public SitemapCrawlerTests()
    {
        _sitemapLoaderWrapper = new Mock<SitemapLoaderWrapper>();
        _sitemapCrawler = new SitemapCrawler(_sitemapLoaderWrapper.Object);
    }

    [Fact]
    public async Task GetLinksFromSitemapAsync_Url_ShouldReturnUrlsFromSitemap()
    {
        var testBaseUrl = new Uri("https://jwt.io/");

        var testSitemap = SitemapLoaderTestData(); 

        _sitemapLoaderWrapper.Setup(x => x.LoadAsync(It.IsAny<Uri>())).ReturnsAsync(testSitemap);

        var result = await _sitemapCrawler.GetLinksFromSitemapAsync(testBaseUrl);

        Assert.NotNull(result);
        Assert.Equal(3, result.Count());
        Assert.True(result.All(x => x.UrlFoundLocation == UrlFoundLocation.Sitemap));
        Assert.True(result.All(x => x.ResponseTime == null));
        Assert.Contains(result, x => x.Url == new Uri("https://jwt.io/"));
        Assert.Contains(result, x => x.Url == new Uri("https://jwt.io/libraries"));
        Assert.Contains(result, x => x.Url == new Uri("https://jwt.io/introduction"));
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
