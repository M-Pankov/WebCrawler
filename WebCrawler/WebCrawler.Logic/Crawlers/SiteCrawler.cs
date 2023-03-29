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
        IList<Uri> uniqueURLs = new List<Uri>
        {
            input
        };

        IList<UrlWithResponseTime> urlsWithResponseTime = new List<UrlWithResponseTime>();

        urlsWithResponseTime = await CrawlUrlAsync(input, uniqueURLs, urlsWithResponseTime);

        return urlsWithResponseTime.OrderBy(x => x.ResponseTime).ToList();
    }

    private async Task<IList<UrlWithResponseTime>> CrawlUrlAsync(Uri input, IList<Uri> uniqueURLs, IList<UrlWithResponseTime> urlsWithResponseTime)
    {
        var htmlStringWithResponseTime = await GetHtmlStringWithResponseTimeAsync(input);

        urlsWithResponseTime = AddUrlWithResponseTime(urlsWithResponseTime, input, htmlStringWithResponseTime.ResponseTime);

        var linksFromPage = _htmlParser.GetLinks(input, htmlStringWithResponseTime.HtmlString);

        var validLinks = linksFromPage.Where(x => !_urlValidator.IsDisallowed(x, uniqueURLs, input)).ToArray();

        if (!validLinks.Any())
        {
            return urlsWithResponseTime;
        }

        foreach (var link in validLinks)
        {
            uniqueURLs.Add(link);
        }

        foreach (var link in validLinks)
        {
            urlsWithResponseTime = await CrawlUrlAsync(link, uniqueURLs, urlsWithResponseTime);
        }

        return urlsWithResponseTime;
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

    private IList<UrlWithResponseTime> AddUrlWithResponseTime(IList<UrlWithResponseTime> urlsWithResponseTime, Uri url, long responseTime)
    {
        var urlWithResponse = new UrlWithResponseTime()
        {
            Url = url,
            ResponseTime = responseTime
        };

        urlsWithResponseTime.Add(urlWithResponse);

        return urlsWithResponseTime;
    }

}
