using Moq;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using WebCrawler.Application;
using WebCrawler.Application.Crawlers;
using WebCrawler.Application.Interfaces;
using WebCrawler.Application.Loaders;
using WebCrawler.Application.Models;
using WebCrawler.Application.Parsers;
using WebCrawler.Application.Validators;
using WebCrawler.Domain.Enums;
using WebCrawler.Presentation.Console.Services;
using Xunit;

namespace WebCrawler.Presentation.Console.Tests;

public class ConsoleWebCrawlerTests
{
    private readonly HttpClient _httpClient;
    private readonly Mock<ICrawledSiteRepository> _crawledSitesRepo;
    private readonly Mock<ICrawledSiteUrlRepository> _crawledSiteUrlRepo;
    private readonly Mock<CrawlerService> _crawlerService;
    private readonly Mock<HtmlParser> _htmlParser;
    private readonly Mock<UrlValidator> _urlValidator;
    private readonly Mock<HtmlLoader> _htmlLoader;
    private readonly Mock<SiteMapLoader> _siteMapLoader;
    private readonly Mock<SiteMapCrawler> _siteMapCrawler;
    private readonly Mock<SiteCrawler> _siteCrawler;
    private readonly Mock<ConsoleService> _consoleService;
    private readonly Mock<Crawler> _crawler;
    private readonly ConsoleWebCrawler _consoleWebCrawler;
    public ConsoleWebCrawlerTests()
    {
        _httpClient = new HttpClient();
        _htmlParser = new Mock<HtmlParser>();
        _urlValidator = new Mock<UrlValidator>();
        _htmlLoader = new Mock<HtmlLoader>(_httpClient);
        _siteMapLoader = new Mock<SiteMapLoader>();
        _siteMapCrawler = new Mock<SiteMapCrawler>(_siteMapLoader.Object);
        _siteCrawler = new Mock<SiteCrawler>(_htmlParser.Object, _urlValidator.Object, _htmlLoader.Object);
        _consoleService = new Mock<ConsoleService>();
        _crawler = new Mock<Crawler>(_siteCrawler.Object, _siteMapCrawler.Object, _htmlLoader.Object);
        _crawledSitesRepo = new Mock<ICrawledSiteRepository>();
        _crawledSiteUrlRepo = new Mock<ICrawledSiteUrlRepository>();
        _crawlerService = new Mock<CrawlerService>(_crawledSitesRepo.Object, _crawledSiteUrlRepo.Object, _crawler.Object);
        _consoleWebCrawler = new ConsoleWebCrawler(_crawlerService.Object, _consoleService.Object);
    }

    [Fact]
    public async Task StartCrawlAsync_EmptyString_ShouldReturnWelcomeMessageAndShutDown()
    {
        var testInput = string.Empty;

        _consoleService.Setup(x => x.WriteLine(It.IsAny<string>()));
        _consoleService.Setup(x => x.ReadLine()).Returns(testInput);

        await _consoleWebCrawler.StartCrawlAsync();

        _consoleService.Verify(p => p.WriteLine("Enter the site address in the format: \"https://example.com\" (enter to exit):"), Times.Once);
        _consoleService.Verify(p => p.ReadLine(), Times.Once);
    }

    [Fact]
    public async Task StartCrawlAsync_Url_ShouldReturnUrlFromSite()
    {
        SetupMockObjects();

        await _consoleWebCrawler.StartCrawlAsync();

        _consoleService.Verify(p => p.WriteLine("\nUrls founded by crawling the website but not in sitemap.xml:"), Times.Once);
        _consoleService.Verify(p => p.WriteLine("1) https://www.litedb.org/docs"), Times.Once);
    }

    [Fact]
    public async Task StartCrawlAsync_Url_ShouldReturnUrlFromSitemap()
    {
        SetupMockObjects();

        await _consoleWebCrawler.StartCrawlAsync();

        _consoleService.Verify(p => p.WriteLine("\nUrls founded in sitemap.xml but not founded after crawling a web site:"), Times.Once);
        _consoleService.Verify(p => p.WriteLine("1) https://www.litedb.org/docs/getting-started"), Times.Once);
    }

    [Fact]
    public async Task StartCrawlAsync_Url_ShouldReturnUrlsWithTimings()
    {
        SetupMockObjects();

        await _consoleWebCrawler.StartCrawlAsync();

        _consoleService.Verify(p => p.WriteLine("\nUrl : Timing (ms)"), Times.Once);
        _consoleService.Verify(p => p.WriteLine("1) https://www.litedb.org/ : 20ms"), Times.Once);
        _consoleService.Verify(p => p.WriteLine("2) https://www.litedb.org/docs : 20ms"), Times.Once);
        _consoleService.Verify(p => p.WriteLine("3) https://www.litedb.org/docs/getting-started : 20ms"), Times.Once);
    }

    [Fact]
    public async Task StartCrawlAsync_Url_ShouldReturnSumOfUrlsFromSiteAndSitemap()
    {
        SetupMockObjects();

        await _consoleWebCrawler.StartCrawlAsync();

        _consoleService.Verify(p => p.WriteLine("\nUrls (html documents) found after crawling a website: 2"), Times.Once);
        _consoleService.Verify(p => p.WriteLine("\nUrls found in sitemap: 2"), Times.Once);
    }

    private void SetupMockObjects()
    {
        var testInput = "https://www.litedb.org/";
        var crawlerTestData = GetCrawlerTestData();

        _crawlerService.Setup(x => x.CrawlSiteAsync(It.IsAny<string>())).ReturnsAsync(crawlerTestData.Id);
        _crawlerService.Setup(x => x.GetCrawledSiteResultsAsync(crawlerTestData.Id)).ReturnsAsync(crawlerTestData);
        _consoleService.Setup(x => x.WriteLine(It.IsAny<string>()));
        _consoleService.Setup(x => x.ReadLine()).Returns(testInput);
    }

    private CrawledSiteDto GetCrawlerTestData()
    {
        var firstUrl = new CrawledSiteUrlDto() { Url = new Uri("https://www.litedb.org/"), UrlFoundLocation = UrlFoundLocation.Both, ResponseTimeMs = 20 };
        var secondUrl = new CrawledSiteUrlDto() { Url = new Uri("https://www.litedb.org/docs"), UrlFoundLocation = UrlFoundLocation.Site, ResponseTimeMs = 20 };
        var thirdUrl = new CrawledSiteUrlDto() { Url = new Uri("https://www.litedb.org/docs/getting-started"), UrlFoundLocation = UrlFoundLocation.Sitemap, ResponseTimeMs = 20 };

        var testData = new CrawledSiteDto()
        {
            Id = 1,
        };

        testData.SiteCrawlResults = new List<CrawledSiteUrlDto>() { firstUrl, secondUrl, thirdUrl };
        testData.OnlySitemapResults = new List<CrawledSiteUrlDto>() { secondUrl };
        testData.OnlySiteResults = new List<CrawledSiteUrlDto>() { thirdUrl };

        return testData;
    }
}
