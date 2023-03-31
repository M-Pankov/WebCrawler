using System;
using System.Linq;
using WebCrawler.Logic.Crawlers;
using WebCrawler.Logic.Loaders;
using Xunit;

namespace WebCrawler.Logic.Tests.Crawlers;

public class CrawlerTests
{
    private readonly SiteCrawler _siteCrawler;
    private readonly SitemapCrawler _sitemapCrawler;
    private readonly HtmlLoader _htmlLoader;
    private readonly Crawler _crawler;
    public CrawlerTests()
    {
        _sitemapCrawler = new SitemapCrawler();
        _siteCrawler = new SiteCrawler();
        _htmlLoader = new HtmlLoader();
        _crawler = new Crawler(_siteCrawler, _sitemapCrawler, _htmlLoader);
    }

    [Fact]
    public async void Test1()
    {
        var testUrl = new Uri("https://www.litedb.org/");

        var result = await _crawler.FoundAllPagesWithResponseTime(testUrl);

        Assert.NotNull(result);
        Assert.NotEmpty(result);
        Assert.Equal(25, result.Count());
    }
}
