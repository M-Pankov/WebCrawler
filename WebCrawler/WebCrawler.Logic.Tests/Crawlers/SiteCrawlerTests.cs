using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using WebCrawler.Logic.Crawlers;
using WebCrawler.Logic.Parsers;
using WebCrawler.Logic.Validators;
using Xunit;

namespace WebCrawler.Logic.Tests.Crawlers;

public class SiteCrawlerTests
{
    private readonly SiteCrawler _crawler;
    private readonly Mock<HtmlParser> _htmlParser;
    private readonly HttpClient _httpClient;
    private readonly Mock<UrlValidator> _urlValidator;
    public SiteCrawlerTests()
    {
        _httpClient = new HttpClient();
        _htmlParser = new Mock<HtmlParser>();
        _urlValidator = new Mock<UrlValidator>();
        _crawler = new SiteCrawler(_htmlParser.Object, _urlValidator.Object, _httpClient);
    }

    [Fact]
    public async void GetUrlsWithResponseTimeAsync_TestUrl_ListOfLinksFromSiteWithTimings()
    {
        var testUrl = new Uri("https://www.litedb.org/");

        IList<Uri> testLinksList = new List<Uri>();

        _htmlParser.Setup(x => x.GetLinks(testUrl, It.IsAny<string>())).Returns(testLinksList);

        _urlValidator.Setup(x => x.IsDisallowed(It.IsAny<Uri>(), It.IsAny<Uri>())).Returns(false);


        var result = await _crawler.GetUrlsWithResponseTimeAsync(testUrl);

        Assert.NotNull(result);
        Assert.NotEmpty(result);
        Assert.Equal(1, result.Count());
    }
}
