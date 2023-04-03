using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebCrawler.Console.Services;
using WebCrawler.Logic.Crawlers;
using WebCrawler.Logic.Enums;
using WebCrawler.Logic.Models;

namespace WebCrawler.Console;

public class ConsoleWebCrawler
{
    private readonly Crawler _crawler;
    private readonly ConsoleService _consoleService;

    public ConsoleWebCrawler(Crawler crawler, ConsoleService consoleService)
    {
        _crawler = crawler;
        _consoleService = consoleService;
    }

    public async Task StartCrawlAsync()
    {
        _consoleService.WriteLine("Enter the site address in the format: \"https://example.com\" (enter to exit):");

        var input = _consoleService.ReadLine();

        if (string.IsNullOrEmpty(input))
        {
            return;
        }

        var urlInput = new Uri(input);

        var results = await _crawler.CrawlUrlAsync(urlInput);

        var printWithTiming = true;

        PrintOnlySitemapUrls(results);

        PrintOnlySiteUrls(results);

        PrintUrls(results, printWithTiming);

        _consoleService.ReadLine();
    }

    private void PrintOnlySitemapUrls(IEnumerable<CrawledUrl> results)
    {
        var onlySitemapUrls = results.Where(x => x.UrlFoundLocation == UrlFoundLocation.Sitemap);

        var printWithTiming = false;

        if (!onlySitemapUrls.Any())
        {
            _consoleService.WriteLine("\nUrls list founded in sitemap.xml but not founded after crawling a website is empty.");

            return;
        }

        _consoleService.WriteLine("\nUrls founded in sitemap.xml but not founded after crawling a web site:");

        PrintUrls(onlySitemapUrls, printWithTiming);
    }

    private void PrintOnlySiteUrls(IEnumerable<CrawledUrl> results)
    {
        var onlySiteUrls = results.Where(x => x.UrlFoundLocation == UrlFoundLocation.Site);

        var printWithTiming = false;

        if (!onlySiteUrls.Any())
        {
            _consoleService.WriteLine("\nUrls list founded by crawling the website but not in sitemap.xml is empty.");

            return;
        }

        _consoleService.WriteLine("\nUrls founded by crawling the website but not in sitemap.xml:");

        PrintUrls(onlySiteUrls, printWithTiming);
    }

    private void PrintUrls(IEnumerable<CrawledUrl> results, bool printWithTiming)
    {
        var counter = 1;

        if (!printWithTiming)
        {
            _consoleService.WriteLine("\nUrl");

            foreach (var result in results)
            {
                _consoleService.WriteLine($"{counter++}) {result.Url}");
            }

            return;
        }

        results = results.OrderBy(x => x.ResponseTime);

        _consoleService.WriteLine("\nUrl : Timing (ms)");

        foreach (var result in results)
        {
            _consoleService.WriteLine($"{counter++}) {result.Url} : {result.ResponseTime}ms");
        }

        var crawledFromSite = results.Where(x => x.UrlFoundLocation == UrlFoundLocation.Site || x.UrlFoundLocation == UrlFoundLocation.Both).Count();

        _consoleService.WriteLine($"\nUrls (html documents) found after crawling a website: {crawledFromSite}");

        var crawledFromSitemap = results.Where(x => x.UrlFoundLocation == UrlFoundLocation.Sitemap || x.UrlFoundLocation == UrlFoundLocation.Both).Count();

        _consoleService.WriteLine($"\nUrls found in sitemap: {crawledFromSitemap}");
    }
}
