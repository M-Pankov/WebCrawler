using System;
using Xunit;

namespace WebCrawlService.Tests;

public class SitemapCrawlerTests
{
    private readonly SitemapCrawler _sitemapChecker;

    public SitemapCrawlerTests()
    {
        _sitemapChecker = new SitemapCrawler();
    }

    [Fact]
    public async void GetLinksFromSitemap_TestUrl_ListOfLinksFromSitemap()
    {
        var testUrl = new Uri("https://www.litedb.org/");

        var result = await _sitemapChecker.GetLinksFromSitemap(testUrl);

        Assert.NotNull(result);
        Assert.NotEmpty(result);
        Assert.Equal(25, result.Count);
    }
}