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

    public async Task<IEnumerable<UrlWithResponseTime>> GetUrlsWithResponseTimeAsync(Uri input)
    {
        var startUrl = new UrlWithResponseTime()
        {
            Url = input
        };

        var crawlResult = await CrawlUrlAsync(startUrl);

        return crawlResult.OrderBy(x => x.ResponseTime);
    }

    private async Task<IEnumerable<UrlWithResponseTime>> CrawlUrlAsync(UrlWithResponseTime input)
    {
        IList<UrlWithResponseTime> currentUrls = new List<UrlWithResponseTime>
        {
            input
        };

        while (input != null)
        {
            var htmlContentWithResponseTime = await GetHtmlContentWithResponseTimeAsync(input.Url);

            input.ResponseTime = htmlContentWithResponseTime.ResponseTime;

            currentUrls = AddNewLinksFromHtmlContent(currentUrls, input, htmlContentWithResponseTime.HtmlContent);

            input = currentUrls.FirstOrDefault(x => !x.ResponseTime.HasValue);
        }

        return currentUrls;
    }

    private async Task<HtmlContentWithResponseTime> GetHtmlContentWithResponseTimeAsync(Uri input)
    {
        var stopwatch = new Stopwatch();
        stopwatch.Restart();

        var htmlString = await _httpClient.GetStringAsync(input);
        stopwatch.Stop();

        return new HtmlContentWithResponseTime()
        {
            HtmlContent = htmlString,
            ResponseTime = stopwatch.ElapsedMilliseconds
        };
    }

    private IList<UrlWithResponseTime> AddNewLinksFromHtmlContent(IList<UrlWithResponseTime> currentLinks, UrlWithResponseTime input, string htmlContent)
    {
        var newLinks = GetNewValidLinksFromHtmlContent(currentLinks, input, htmlContent);

        foreach (var link in newLinks)
        {
            currentLinks.Add(link);
        }

        return currentLinks;
    }

    private IEnumerable<UrlWithResponseTime> GetNewValidLinksFromHtmlContent(IEnumerable<UrlWithResponseTime> currentLinks, UrlWithResponseTime input, string htmlContent)
    {
        var currentUrls = currentLinks.Select(x => x.Url);

        var vaidUrlsFromPage = _htmlParser.GetLinks(input.Url, htmlContent)
           .Where(x => !_urlValidator.IsDisallowed(x, input.Url));

        return vaidUrlsFromPage.Where(x => !currentUrls.Contains(x))
            .Select(x => new UrlWithResponseTime()
            {
                Url = x
            });
    }
}
