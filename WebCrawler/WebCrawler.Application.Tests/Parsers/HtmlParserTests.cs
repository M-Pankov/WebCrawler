using System.Collections.Generic;
using System;
using WebCrawler.Application.Parsers;
using Xunit;
using System.Linq;

namespace WebCrawler.Application.Tests.Parsers;

public class HtmlParserTests
{
    private readonly HtmlParser _htmlParser;
    public HtmlParserTests()
    {
        _htmlParser = new HtmlParser();
    }

    [Fact]
    public void GetLinks_UrlAndHtmlContentWithTwoLocalPathUrls_ShouldReturnTwoUrls()
    {
        var testHtmlContent = "<a href=\"/docs/\"><span>DOCS</span></a> <a href=\"/api/\"><span>API</span></a>";

        var testBaseUrl = new Uri("https://www.litedb.org/");

        var expected = new List<Uri>()
        {
            new Uri("https://www.litedb.org/docs"),
            new Uri("https://www.litedb.org/api")
        };

        var result = _htmlParser.GetLinks(testBaseUrl, testHtmlContent);

        Assert.NotNull(result);
        Assert.Equal(expected, result);
    }

    [Fact]
    public void GetLinks_UrlAndHtmlContentWithTwoAbsoluteUrls_ShouldReturnTwoUrls()
    {
        var testHtmlContent = " <a href=\"https://github.com/mbdavid/litedb\">Fork me on GitHub</a>" +
            "<a href=\"https://www.nuget.org/packages/LiteDB\"><span>DOWNLOAD</span></a>";

        var testBaseUrl = new Uri("https://www.litedb.org/");

        var expected = new List<Uri>()
        {
            new Uri("https://github.com/mbdavid/litedb"),
            new Uri("https://www.nuget.org/packages/litedb")
        };

        var result = _htmlParser.GetLinks(testBaseUrl, testHtmlContent);

        Assert.NotNull(result);
        Assert.Equal(expected, result);
    }

    [Fact]
    public void GetLinks_UrlAndEmptyHtmlContent_ShouldReturnZeroUrls()
    {
        var testHtmlContent = string.Empty;

        var testBaseUrl = new Uri("https://www.litedb.org/");

        var result = _htmlParser.GetLinks(testBaseUrl, testHtmlContent);

        Assert.NotNull(result);
        Assert.Equal(0, result.Count());
    }
}
