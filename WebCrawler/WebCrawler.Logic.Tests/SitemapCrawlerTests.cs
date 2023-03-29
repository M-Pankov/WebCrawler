using System;
using System.Threading.Tasks;
using WebCrawler.Logic.Crawlers;
using Xunit;

namespace WebCrawler.Logic.Tests;

public class SitemapCrawlerTests
{
    private readonly SitemapCrawler _sitemapChecker;

    public SitemapCrawlerTests()
    {
        _sitemapChecker = new SitemapCrawler();
    }

    [Fact]
    public async Task GetLinksFromSitemapAsync_TestUrl_ListOfLinksFromSitemap()
    {
        var testUrl = new Uri("https://www.litedb.org/");

        var result = await _sitemapChecker.GetLinksFromSitemapAsync(testUrl);

        Assert.NotNull(result);
        Assert.NotEmpty(result);
        Assert.Equal(25, result.Count);
    }
}
