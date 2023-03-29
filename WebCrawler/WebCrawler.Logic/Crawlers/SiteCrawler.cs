using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using WebCrawler.Logic.Models;
using WebCrawler.Logic.Parsers;
using WebCrawler.Logic.Validators;

namespace WebCrawler.Logic.Crawlers;

public class SiteCrawler
{
    private readonly HtmlParser _htmlParser;
    private readonly UrlValidator _urlValidator;
    private readonly HttpClient _httpClient;

    public SiteCrawler(HtmlParser htmlParser, UrlValidator urlValidator, HttpClient httpClient)
    {
        _htmlParser = htmlParser;
        _urlValidator = urlValidator;
        _httpClient = httpClient;
    }

    public async Task<IList<UrlWithResponseTime>> GetSitePagesWithTimingsAsync(Uri input)
    {
        var crawlResult = new CrawlResult();

        crawlResult.UniqueUrls.Add(input);

        crawlResult = await CrawlUrlAsync(input,crawlResult);

        return crawlResult.Results.OrderBy(x => x.ResponseTime).ToList();
    }

    private async Task<CrawlResult> CrawlUrlAsync(Uri input, CrawlResult crawlResult)
    {
        var htmlStringWithResponseTime = await GetHtmlStringWithResponseTimeAsync(input);

        crawlResult = AddUrlWithResponseTime(crawlResult, input, htmlStringWithResponseTime.ResponseTime);

        var linksFromPage = _htmlParser.GetLinks(input, htmlStringWithResponseTime.HtmlString);

        var validLinks = linksFromPage.Where(x => !_urlValidator.IsDisallowed(x, crawlResult.UniqueUrls, input)).ToArray();

        if (!validLinks.Any())
        {
            return crawlResult;
        }

        foreach (var link in validLinks)
        {
            crawlResult.UniqueUrls.Add(link);
        }

        foreach (var link in validLinks)
        {
            crawlResult = await CrawlUrlAsync(link, crawlResult);
        }

        return crawlResult;
    }

    private async Task<HtmlStringWithResponseTime> GetHtmlStringWithResponseTimeAsync(Uri input)
    {
        var stopwatch = new Stopwatch();
        stopwatch.Restart();

        var htmlString = await _httpClient.GetStringAsync(input);
        stopwatch.Stop();

        var htmlStringWithResponseTime = new HtmlStringWithResponseTime()
        {
            HtmlString = htmlString,
            ResponseTime = stopwatch.ElapsedMilliseconds
        };

        return htmlStringWithResponseTime;
    }

    private CrawlResult AddUrlWithResponseTime(CrawlResult crawlResult, Uri url, long responseTime)
    {
        var urlWithResponse = new UrlWithResponseTime()
        {
            Url = url,
            ResponseTime = responseTime
        };

        crawlResult.Results.Add(urlWithResponse);

        return crawlResult;
    }

}
