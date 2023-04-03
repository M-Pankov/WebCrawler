using System;
using WebCrawler.Logic.Validators;
using Xunit;

namespace WebCrawler.Logic.Tests.Validators;

public class UrlValidatorTests
{
    private readonly UrlValidator _urlValidator;

    public UrlValidatorTests()
    {
        _urlValidator = new UrlValidator();
    }

    [Fact]
    public void IsAllowed_TwoOneHostedUrls_ShouldReturnTrue()
    {
        var testBaseUrl = new Uri("https://www.litedb.org/");

        var testInputUrl = new Uri("https://www.litedb.org/docs/");

        var result = _urlValidator.IsAllowed(testInputUrl, testBaseUrl);

        Assert.True(result);
    }

    [Fact]
    public void IsAllowed_TwoDifferentHostedUrls_ShouldReturnFalse()
    {
        var testBaseUrl = new Uri("https://www.litedb.org/");

        var testInputUrl = new Uri("https://jwt.io");

        var result = _urlValidator.IsAllowed(testInputUrl, testBaseUrl);

        Assert.False(result);
    }

    [Fact]
    public void IsAllowed_UrlAndLinkToFile_ShouldReturnFalse()
    {
        var testBaseUrl = new Uri("https://www.litedb.org/");

        var testInputUrl = new Uri("https://www.litedb.org/images/logo_litedb.svg");

        var result = _urlValidator.IsAllowed(testInputUrl, testBaseUrl);

        Assert.False(result);
    }

    [Fact]
    public void IsAllowed_UrlAndNull_ShouldReturnFalse()
    {
        var testBaseUrl = new Uri("https://www.litedb.org/");

        Uri testInputUrl = null;

        var result = _urlValidator.IsAllowed(testInputUrl, testBaseUrl);

        Assert.False(result);
    }
}
