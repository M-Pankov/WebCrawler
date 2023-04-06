using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebCrawler.Console.Services;
using WebCrawler.Logic.Crawlers;
using WebCrawler.Logic.Enums;
using WebCrawler.Logic.Models;
using WebCrawler.Model.Entities;
using WebCrawler.Repository;

namespace WebCrawler.Console;

public class ConsoleWebCrawler
{
    private readonly Crawler _crawler;
    private readonly ConsoleService _consoleService;
    private readonly IUnitOfWork _unitOfWork;

    public ConsoleWebCrawler(Crawler crawler, ConsoleService consoleService, IUnitOfWork unitOfWork)
    {
        _crawler = crawler;
        _consoleService = consoleService;
        _unitOfWork = unitOfWork;
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

        var results = await _crawler.CrawlUrlsAsync(urlInput);

        PrintOnlySitemapUrls(results);

        PrintOnlySiteUrls(results);

        PrintAllUrlsWithTimings(results);

        SaveCrawlResult(urlInput, results);

        _consoleService.ReadLine();
    }

    private void PrintOnlySitemapUrls(IEnumerable<CrawledUrl> results)
    {
        var onlySitemapUrls = results.Where(x => x.UrlFoundLocation == UrlFoundLocation.Sitemap);

        if (!onlySitemapUrls.Any())
        {
            _consoleService.WriteLine("\nUrls list founded in sitemap.xml but not founded after crawling a website is empty.");

            return;
        }

        _consoleService.WriteLine("\nUrls founded in sitemap.xml but not founded after crawling a web site:");

        PrintUrls(onlySitemapUrls);
    }

    private void PrintOnlySiteUrls(IEnumerable<CrawledUrl> results)
    {
        var onlySiteUrls = results.Where(x => x.UrlFoundLocation == UrlFoundLocation.Site);

        if (!onlySiteUrls.Any())
        {
            _consoleService.WriteLine("\nUrls list founded by crawling the website but not in sitemap.xml is empty.");

            return;
        }

        _consoleService.WriteLine("\nUrls founded by crawling the website but not in sitemap.xml:");

        PrintUrls(onlySiteUrls);
    }

    private void PrintUrls(IEnumerable<CrawledUrl> results)
    {
        var counter = 1;

        _consoleService.WriteLine("\nUrl");

        foreach (var result in results)
        {
            _consoleService.WriteLine($"{counter++}) {result.Url}");
        }
    }

    private void PrintAllUrlsWithTimings(IEnumerable<CrawledUrl> results)
    {
        var counter = 1;

        results = results.OrderBy(x => x.ResponseTimeMs);

        _consoleService.WriteLine("\nUrl : Timing (ms)");

        foreach (var result in results)
        {
            _consoleService.WriteLine($"{counter++}) {result.Url} : {result.ResponseTimeMs}ms");
        }

        var crawledFromSite = results.Count(x => x.UrlFoundLocation == UrlFoundLocation.Site || x.UrlFoundLocation == UrlFoundLocation.Both);

        _consoleService.WriteLine($"\nUrls (html documents) found after crawling a website: {crawledFromSite}");

        var crawledFromSitemap = results.Count(x => x.UrlFoundLocation == UrlFoundLocation.Sitemap || x.UrlFoundLocation == UrlFoundLocation.Both);

        _consoleService.WriteLine($"\nUrls found in sitemap: {crawledFromSitemap}");

    }

    private void SaveCrawlResult(Uri uriInput, IEnumerable<CrawledUrl> results)
    {
        var crawlResult = new SiteCrawlResult()
        {
            Url = uriInput,
            CrawledUrls = results.Select(x => new UrlCrawlResult()
            {
                Url = x.Url,
                ResponseTimeMs = x.ResponseTimeMs,
                UrlFoundLocation = x.UrlFoundLocation

            }).ToList()
        };

        _unitOfWork.SiteCrawlResults.Add(crawlResult);
        _unitOfWork.Complete();

        _consoleService.WriteLine("Crawl result saved");
    }
}
