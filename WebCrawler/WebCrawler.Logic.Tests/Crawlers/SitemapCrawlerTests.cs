using Louw.SitemapParser;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebCrawler.Logic.Crawlers;
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
    public async Task GetLinksFromSitemapAsync_TestUrl_ListOfLinksFromSitemap()
    {
        var testUrl = new Uri("https://www.litedb.org/");
        var testSitemapItem = new SitemapItem(testUrl);
        var testSitemapItems = new List<SitemapItem> { testSitemapItem };
        var testSitemap = new Sitemap(testSitemapItems);


        _sitemapLoaderWrapper.Setup(x => x.LoadAsync(It.IsAny<Uri>())).ReturnsAsync(testSitemap);

        var result = await _sitemapCrawler.GetLinksFromSitemapAsync(testUrl);

        Assert.NotNull(result);
        Assert.NotEmpty(result);
        Assert.Equal(1, result.Count());
    }
}
