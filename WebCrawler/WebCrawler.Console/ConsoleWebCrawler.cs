using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebCrawler.Application.Crawler;
using WebCrawler.Application.Crawler.Models;
using WebCrawler.Domain.Enums;
using WebCrawler.Presentation.Console.Services;

namespace WebCrawler.Presentation.Console;

public class ConsoleWebCrawler
{
    private readonly CrawlerService _crawlerService;
    private readonly ConsoleService _consoleService;

    public ConsoleWebCrawler(CrawlerService crawlerService, ConsoleService consoleService)
    {
        _consoleService = consoleService;
        _crawlerService = crawlerService;
    }

    public async Task StartCrawlAsync()
    {
        _consoleService.WriteLine("Enter the site address in the format: \"https://example.com\" (enter to exit):");

        var input = _consoleService.ReadLine();

        if (string.IsNullOrEmpty(input))
        {
            return;
        }

        var siteId = await _crawlerService.CrawlSiteAsync(input);

        var result = await _crawlerService.GetCrawledSiteResultsAsync(siteId);

        _consoleService.WriteLine("\nCrawl result saved to DataBase");

        PrintOnlySitemapUrls(result);

        PrintOnlySiteUrls(result);

        PrintAllUrlsWithTimings(result);



        _consoleService.ReadLine();
    }

    private void PrintOnlySitemapUrls(CrawledSiteDto result)
    {
        if (!result.OnlySitemapResults.Any())
        {
            _consoleService.WriteLine("\nUrls list founded in sitemap.xml but not founded after crawling a website is empty.");

            return;
        }

        _consoleService.WriteLine("\nUrls founded in sitemap.xml but not founded after crawling a web site:");

        PrintUrls(result.OnlySitemapResults);
    }

    private void PrintOnlySiteUrls(CrawledSiteDto result)
    {
        if (!result.OnlySiteResults.Any())
        {
            _consoleService.WriteLine("\nUrls list founded by crawling the website but not in sitemap.xml is empty.");

            return;
        }

        _consoleService.WriteLine("\nUrls founded by crawling the website but not in sitemap.xml:");

        PrintUrls(result.OnlySiteResults);
    }

    private void PrintUrls(IEnumerable<CrawledSiteUrlDto> results)
    {
        var counter = 1;

        _consoleService.WriteLine("\nUrl");

        foreach (var result in results)
        {
            _consoleService.WriteLine($"{counter++}) {result.Url}");
        }
    }

    private void PrintAllUrlsWithTimings(CrawledSiteDto result)
    {
        var counter = 1;

        _consoleService.WriteLine("\nUrl : Timing (ms)");

        foreach (var crawledUrl in result.SiteCrawlResults)
        {
            _consoleService.WriteLine($"{counter++}) {crawledUrl.Url} : {crawledUrl.ResponseTimeMs}ms");
        }

        var crawledFromSite = result.SiteCrawlResults.Count(x => x.UrlFoundLocation == UrlFoundLocation.Site
        || x.UrlFoundLocation == UrlFoundLocation.Both);

        _consoleService.WriteLine($"\nUrls (html documents) found after crawling a website: {crawledFromSite}");

        var crawledFromSitemap = result.SiteCrawlResults.Count(x => x.UrlFoundLocation == UrlFoundLocation.Sitemap
        || x.UrlFoundLocation == UrlFoundLocation.Both);

        _consoleService.WriteLine($"\nUrls found in sitemap: {crawledFromSitemap}");
    }
}
