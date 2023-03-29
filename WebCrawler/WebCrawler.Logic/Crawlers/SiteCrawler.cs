using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using WebCrawler.Logic.Models;
using WebCrawler.Logic.Parsers;

namespace WebCrawler.Logic.Crawlers;

public class SiteCrawler
{

    private Uri _startUrl;
    private HtmlParser _htmlParser;
    private IList<Uri> _uniqueURLs = new List<Uri>();
    private IList<PageWithTiming> _pagesWithTiming = new List<PageWithTiming>();

    public SiteCrawler()
    {
        _htmlParser = new HtmlParser();
    }

    public async Task<IList<PageWithTiming>> GetSitePagesWithTimingsAsync(Uri input, HttpClient httpClient)
    {
        _startUrl = input;
        _uniqueURLs.Add(input);

        await CrawlUrlAsync(input, httpClient);

        return _pagesWithTiming.OrderBy(x => x.Timing).ToList();
    }

    private async Task CrawlUrlAsync(Uri input, HttpClient httpClient)
    {
        var htmlString = await GetHtmlStringWithTimingAsync(input, httpClient);

        var linksFromPage = _htmlParser.GetLinksFromHtmlString(input, htmlString).Where(x => !IsDisallowed(x));

        if (!linksFromPage.Any())
        {
            return;
        }

        foreach (var link in linksFromPage)
        {
            _uniqueURLs.Add(link);
            await CrawlUrlAsync(link, httpClient);
        }
    }

    private async Task<string> GetHtmlStringWithTimingAsync(Uri input, HttpClient httpClient)
    {

        var stopwatch = new Stopwatch();
        stopwatch.Restart();

        var htmlString = await httpClient.GetStringAsync(input);
        stopwatch.Stop();

        AddPageWithTiming(input, stopwatch);

        return htmlString;
    }

    private void AddPageWithTiming(Uri input, Stopwatch stopwatch)
    {
        var pageWithTiming = new PageWithTiming()
        { 
            PageUrl = input,
            Timing = stopwatch.ElapsedMilliseconds
        };

        _pagesWithTiming.Add(pageWithTiming);
    }

    private bool IsDisallowed(Uri input)
    {
        if (input == null || _uniqueURLs.Contains(input) || input.Host != _startUrl.Host)
        {
            return true;
        }

        if (input.LocalPath.Contains('.'))
        {
            return true;
        }

        return false;
    }
}
