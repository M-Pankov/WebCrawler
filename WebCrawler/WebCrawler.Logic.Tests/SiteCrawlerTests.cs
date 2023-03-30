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

        _htmlParser = new HtmlParser();
        _urlValidator = new UrlValidator();
        _crawler = new SiteCrawler(_htmlParser, _urlValidator, _httpClient);
    }

    [Fact]
    public async void GetUrlsWithResponseTimeAsync_TestUrl_ListOfLinksFromSiteWithTimings()
    {
        var testUrl = new Uri("https://www.litedb.org/");

        var result = await _crawler.GetUrlsWithResponseTimeAsync(testUrl);

        Assert.NotNull(result);
        Assert.NotEmpty(result);
        Assert.Equal(23, result.Count());
    }
}
