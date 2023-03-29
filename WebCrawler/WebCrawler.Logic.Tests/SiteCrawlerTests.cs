using System;
using System.Linq;
using System.Net.Http;
using WebCrawler.Logic.Crawlers;
using WebCrawler.Logic.Parsers;
using WebCrawler.Logic.Validators;
using Xunit;

namespace WebCrawler.Logic.Tests;

public class SiteCrawlerTests
{
    private readonly SiteCrawler _crawler;
    private readonly HtmlParser _htmlParser;
    private readonly HttpClient _httpClient;
    private readonly UrlValidator _urlValidator;
    public SiteCrawlerTests()
    {
        _httpClient = new HttpClient();
        _httpClient.Timeout = TimeSpan.FromSeconds(10);
        _httpClient.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/105.0.0.0 Safari/537.36 Edg/105.0.1343.53");
        _httpClient.DefaultRequestHeaders.Add("Accept", "text/html,application/xhtml+xml,application/xml");
        _htmlParser = new HtmlParser();
        _urlValidator = new UrlValidator();
        _crawler = new SiteCrawler(_htmlParser, _urlValidator, _httpClient);
    }

    [Fact]
    public async void GetSitePagesWithTimingsAsync_TestUrl_ListOfLinksFromSiteWithTimings()
    {
        var testUrl = new Uri("https://www.litedb.org/");

        var result = await _crawler.GetUrlsWithResponseTimeAsync(testUrl);

        Assert.NotNull(result);
        Assert.NotEmpty(result);
        Assert.Equal(23, result.Count());
    }
}
