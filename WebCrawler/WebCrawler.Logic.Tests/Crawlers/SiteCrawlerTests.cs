using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using WebCrawler.Logic.Crawlers;
using WebCrawler.Logic.Loaders;
using WebCrawler.Logic.Models;
using WebCrawler.Logic.Parsers;
using WebCrawler.Logic.Validators;
using WebCrawler.Logic.Wrappers;
using Xunit;

namespace WebCrawler.Logic.Tests.Crawlers;

public class SiteCrawlerTests
{
    private readonly SiteCrawler _crawler;
    private readonly Mock<HtmlParser> _htmlParser;
    private readonly Mock<HtmlLoader> _htmlLoader;
    private readonly Mock<UrlValidator> _urlValidator;
    public SiteCrawlerTests()
    {
        _htmlLoader = new Mock<HtmlLoader>();
        _htmlParser = new Mock<HtmlParser>();
        _urlValidator = new Mock<UrlValidator>();
        _crawler = new SiteCrawler(_htmlParser.Object, _urlValidator.Object, _htmlLoader.Object);
    }

    [Fact]
    public async void GetUrlsWithResponseTimeAsync_TestUrl_ListOfLinksFromSiteWithTimings()
    {
        var testUrl = new Uri("https://www.litedb.org/");

        IList<Uri> testLinksList = new List<Uri>();

        var testHtmlContent = new HtmlContentWithResponseTime();

        _htmlLoader.Setup(x => x.GetHtmlContentWithResponseTimeAsync(It.IsAny<Uri>())).ReturnsAsync(testHtmlContent);

        _htmlParser.Setup(x => x.GetLinks(testUrl, It.IsAny<string>())).Returns(testLinksList);

        _urlValidator.Setup(x => x.IsDisallowed(It.IsAny<Uri>(), It.IsAny<Uri>())).Returns(false);


        var result = await _crawler.GetUrlsWithResponseTimeAsync(testUrl);

        Assert.NotNull(result);
        Assert.NotEmpty(result);
        Assert.Equal(1, result.Count());
    }
}
