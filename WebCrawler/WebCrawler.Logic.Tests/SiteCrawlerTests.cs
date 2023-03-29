using System;
using System.Net.Http;
using WebCrawler.Logic.Crawlers;
using Xunit;

namespace WebCrawler.Logic.Tests;

public class SiteCrawlerTests
{
    private readonly SiteCrawler _crawler;
    private readonly HttpClient _httpClient;
    public SiteCrawlerTests()
    {
        _httpClient = new HttpClient();
        _httpClient.Timeout = TimeSpan.FromSeconds(10);
        _httpClient.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/105.0.0.0 Safari/537.36 Edg/105.0.1343.53");
        _httpClient.DefaultRequestHeaders.Add("Accept", "text/html,application/xhtml+xml,application/xml");
        _crawler = new SiteCrawler();
    }

    [Fact]
    public async void GetSitePagesWithTimingsAsync_TestUrl_ListOfLinksFromSiteWithTimings()
    {
        var testUrl = new Uri("https://www.litedb.org/");

        var result = await _crawler.GetSitePagesWithTimingsAsync(testUrl, _httpClient);

        Assert.NotNull(result);
        Assert.NotEmpty(result);
        Assert.Equal(23, result.Count);
    }
}
