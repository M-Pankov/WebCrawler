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

    public async Task<IEnumerable<UrlWithResponseTime>> GetSitePagesWithTimingsAsync(Uri input)
    {
        IList<UrlWithResponseTime> startUrls = new List<UrlWithResponseTime>();

         var  startUrl  = new UrlWithResponseTime()
         {
             Url = input,
         };
        
        var crawlResult = await CrawlUrlAsync(startUrl,startUrls);

        return crawlResult;
    }

    private async Task<IEnumerable<UrlWithResponseTime>> CrawlUrlAsync(UrlWithResponseTime input, IEnumerable<UrlWithResponseTime> urls)
    {
        if(input.ResponseTime != null)
        {
            return urls;
        }
         
        var htmlContentWithResponseTime = await GetHtmlContentWithResponseTimeAsync(input.Url);

        input.ResponseTime = htmlContentWithResponseTime.ResponseTime;

        var vaidLinksFromPage = _htmlParser.GetLinks(input.Url,htmlContentWithResponseTime.HtmlContent)
            .Where(x => !_urlValidator.IsDisallowed(x,input.Url));

        urls = AddNewValidUrls(urls, vaidLinksFromPage);

        var notCrawledUrls = urls.Where(x => x.ResponseTime == null);

        foreach(var url in notCrawledUrls)
        {
            urls = await CrawlUrlAsync(url, urls);
        }

        return urls;
    }

    private async Task<HtmlContentWithResponseTime> GetHtmlContentWithResponseTimeAsync(Uri input)
    {
        var stopwatch = new Stopwatch();
        stopwatch.Restart();

        var htmlString = await _httpClient.GetStringAsync(input);
        stopwatch.Stop();

        var htmlStringWithResponseTime = new HtmlContentWithResponseTime()
        {
            HtmlContent = htmlString,
            ResponseTime = stopwatch.ElapsedMilliseconds
        };

        return htmlStringWithResponseTime;
    }

    private IEnumerable<UrlWithResponseTime> AddNewValidUrls(IEnumerable<UrlWithResponseTime> urls, IEnumerable<Uri> validLinks)
    {
        var currentLinks = urls.Select(x => x.Url);

        var newValidLinks = validLinks.Where(x => !currentLinks.Contains(x));

        foreach(var link in newValidLinks)
        {
            var urlWithResponse = new UrlWithResponseTime()
            {
                Url = link
            };
            urls = urls.Append(urlWithResponse);
        }

        return urls;
    }
}
