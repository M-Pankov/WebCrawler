
using Moq;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using WebCrawler.Console.Services;
using WebCrawler.Logic.Crawlers;
using WebCrawler.Logic.Enums;
using WebCrawler.Logic.Models;
using WebCrawler.Logic.Parsers;
using WebCrawler.Logic.Services;
using WebCrawler.Logic.Validators;
using Xunit;

namespace WebCrawler.Console.Tests;

public class ConsoleWebCrawlerTests
{
    private readonly HttpClient _httpClient;
    private readonly Mock<HtmlParser> _htmlParser;
    private readonly Mock<UrlValidator> _urlValidator;
    private readonly Mock<HtmlLoaderService> _htmlLoaderService;
    private readonly Mock<SitemapLoaderService> _sitemapLoaderService;
    private readonly Mock<SitemapCrawler> _sitemapCrawler;
    private readonly Mock<SiteCrawler> _siteCrawler;
    private readonly Mock<ConsoleService> _consoleService;
    private readonly Mock<Crawler> _crawler;
    private readonly ConsoleWebCrawler _consoleWebCrawler;
    public ConsoleWebCrawlerTests()
    {
        _httpClient = new HttpClient();
        _htmlParser = new Mock<HtmlParser>();
        _urlValidator = new Mock<UrlValidator>();
        _htmlLoaderService = new Mock<HtmlLoaderService>(_httpClient);
        _sitemapLoaderService = new Mock<SitemapLoaderService>();
        _sitemapCrawler = new Mock<SitemapCrawler>(_sitemapLoaderService.Object);
        _siteCrawler = new Mock<SiteCrawler>(_htmlParser.Object, _urlValidator.Object, _htmlLoaderService.Object);
        _consoleService = new Mock<ConsoleService>();
        _crawler = new Mock<Crawler>(_siteCrawler.Object, _sitemapCrawler.Object, _htmlLoaderService.Object);
        _consoleWebCrawler = new ConsoleWebCrawler(_crawler.Object, _consoleService.Object);
    }

    [Fact]
    public async Task StartCrawlAsync_Url_ShouldReturnWelcomeMessageAndShutDown()
    {
        var testInput = string.Empty;

        _consoleService.Setup(x => x.WriteLine(It.IsAny<string>()));
        _consoleService.Setup(x => x.ReadLine()).Returns(testInput);

        await _consoleWebCrawler.StartCrawlAsync();

        _consoleService.Verify(p => p.WriteLine("Enter the site address in the format: \"https://example.com\" (enter to exit):"), Times.Once);
        _consoleService.Verify(p => p.ReadLine(), Times.Once);
    }

    [Fact]
    public async Task StartCrawlAsync_Url_ShouldReturnTwoMessagesOneWithUrlFromSite()
    {
        var testInput = "https://www.litedb.org/";
        var crawlerTestData = GetCrawlerTestData();

        _crawler.Setup(x => x.CrawlUrlAsync(It.IsAny<Uri>())).ReturnsAsync(crawlerTestData);
        _consoleService.Setup(x => x.WriteLine(It.IsAny<string>()));
        _consoleService.Setup(x => x.ReadLine()).Returns(testInput);

        await _consoleWebCrawler.StartCrawlAsync();

        _consoleService.Verify(p => p.WriteLine("\nUrls founded by crawling the website but not in sitemap.xml:"), Times.Once);
        _consoleService.Verify(p => p.WriteLine("1) https://www.litedb.org/docs"), Times.Once);
    }

    [Fact]
    public async Task StartCrawlAsync_Url_ShouldReturnTwoMessagesOneWithUrlFromSitemap()
    {
        var testInput = "https://www.litedb.org/";
        var crawlerTestData = GetCrawlerTestData();

        _crawler.Setup(x => x.CrawlUrlAsync(It.IsAny<Uri>())).ReturnsAsync(crawlerTestData);
        _consoleService.Setup(x => x.WriteLine(It.IsAny<string>()));
        _consoleService.Setup(x => x.ReadLine()).Returns(testInput);

        await _consoleWebCrawler.StartCrawlAsync();

        _consoleService.Verify(p => p.WriteLine("\nUrls founded in sitemap.xml but not founded after crawling a web site:"), Times.Once);
        _consoleService.Verify(p => p.WriteLine("1) https://www.litedb.org/docs/getting-started"), Times.Once);
    }

    [Fact]
    public async Task StartCrawlAsync_Url_ShouldReturnUrlsWithTimings()
    {
        var testInput = "https://www.litedb.org/";
        var crawlerTestData = GetCrawlerTestData();

        _crawler.Setup(x => x.CrawlUrlAsync(It.IsAny<Uri>())).ReturnsAsync(crawlerTestData);
        _consoleService.Setup(x => x.WriteLine(It.IsAny<string>()));
        _consoleService.Setup(x => x.ReadLine()).Returns(testInput);

        await _consoleWebCrawler.StartCrawlAsync();

        _consoleService.Verify(p => p.WriteLine("\nUrl : Timing (ms)"), Times.Once);
        _consoleService.Verify(p => p.WriteLine("1) https://www.litedb.org/ : 20ms"), Times.Once);
        _consoleService.Verify(p => p.WriteLine("2) https://www.litedb.org/docs : 20ms"), Times.Once);
        _consoleService.Verify(p => p.WriteLine("3) https://www.litedb.org/docs/getting-started : 20ms"), Times.Once);
    }

    [Fact]
    public async Task StartCrawlAsync_Url_ShouldReturnSumOfUrlsFromSiteAndSitemap()
    {
        var testInput = "https://www.litedb.org/";
        var crawlerTestData = GetCrawlerTestData();

        _crawler.Setup(x => x.CrawlUrlAsync(It.IsAny<Uri>())).ReturnsAsync(crawlerTestData);
        _consoleService.Setup(x => x.WriteLine(It.IsAny<string>()));
        _consoleService.Setup(x => x.ReadLine()).Returns(testInput);

        await _consoleWebCrawler.StartCrawlAsync();

        _consoleService.Verify(p => p.WriteLine("\nUrls (html documents) found after crawling a website: 2"), Times.Once);
        _consoleService.Verify(p => p.WriteLine("\nUrls found in sitemap: 2"), Times.Once);
    }

    private IEnumerable<CrawledUrl> GetCrawlerTestData()
    {
        return new List<CrawledUrl>()
        {
            new CrawledUrl(){ Url = new Uri("https://www.litedb.org/"), UrlFoundLocation = UrlFoundLocation.Both , ResponseTime = 20},
            new CrawledUrl(){ Url = new Uri("https://www.litedb.org/docs"), UrlFoundLocation = UrlFoundLocation.Site , ResponseTime = 20},
            new CrawledUrl(){ Url = new Uri("https://www.litedb.org/docs/getting-started"), UrlFoundLocation = UrlFoundLocation.Sitemap, ResponseTime = 20}
        };
    }
}
