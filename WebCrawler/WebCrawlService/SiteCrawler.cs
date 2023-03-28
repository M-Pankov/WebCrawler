using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace WebCrawlService;

public class SiteCrawler
{
    private Uri _startUrl;
    private IList<Uri> _uniqueURLs = new List<Uri>();
    private IDictionary<string, long> _pagesWithTiming = new Dictionary<string, long>();
    private IList<string> _disallowedFormats = new List<string>() { ".gif", ".jpg", ".jpeg",
        ".png",".ico",".tiff",".webp",".eps", ".ttf", ".wav", ".zip", ".crt", ".traineddata"
        ,".svg",".psd",".indd",".cdr",".ai", ".xlsx", ".docx", ".msi", ".crl"
        ,".raw",".txt",".xml",".pdf",".dib", ".snippet", ".pfx" };

    public async Task<IList<KeyValuePair<string, long>>> StartCrawl(Uri input, HttpClient httpClient)
    {
        _startUrl = input;

        await CrawlUrl(input, httpClient);

        return _pagesWithTiming.OrderBy(x => x.Value).ToList();
    }

    private async Task CrawlUrl(Uri input, HttpClient httpClient)
    {
        var htmlString = await GetHtmlStringWithTiming(input, httpClient);

        var linksFromPage = await GetLinksFromPage(htmlString);

        if (!linksFromPage.Any())
        {
            return;
        }

        var filteredLinks = await FilterLinks(input, linksFromPage);

        foreach (var link in filteredLinks)
        {
            await CrawlUrl(link, httpClient);
        }
    }

    private async Task<string> GetHtmlStringWithTiming(Uri input, HttpClient httpClient)
    {

        var watch = new Stopwatch();
        watch.Restart();

        var htmlString = await httpClient.GetStringAsync(input);
        watch.Stop();

        _pagesWithTiming.TryAdd(input.ToString().ToLower(), watch.ElapsedMilliseconds);

        return htmlString;
    }

    private async Task<IEnumerable<string>> GetLinksFromPage(string htmlString)
    {
        HtmlDocument htmlDocument = new HtmlDocument();

        htmlDocument.LoadHtml(htmlString);

        var linksFromPage = htmlDocument.DocumentNode
                .Descendants("a")
                .Select(a => a.GetAttributeValue("href", null))
                .Where(u => !string.IsNullOrEmpty(u))
                .Distinct();

        return linksFromPage;
    }

    private async Task<IList<Uri>> FilterLinks(Uri scannedURL, IEnumerable<string> linksFromPage)
    {
        IList<Uri> filteredLinks = new List<Uri>();

        foreach (var link in linksFromPage)
        {
            var standartLink = await StandartiseLink(scannedURL, link);

            if (await IsDisallowed(standartLink))
            {
                continue;
            }

            _uniqueURLs.Add(standartLink);
            filteredLinks.Add(standartLink);
        }

        return filteredLinks;
    }

    private async Task<Uri> StandartiseLink(Uri scannedUrl, string link)
    {
        var lowerLink = link.ToLower();

        if (lowerLink.StartsWith("/"))
        {
            return new Uri(scannedUrl, lowerLink);
        }

        Uri.TryCreate(lowerLink, UriKind.Absolute, out Uri uriResult);
        return uriResult;
    }

    private async Task<bool> IsDisallowed(Uri input)
    {
        if (input == null)
        {
            return true;
        }

        if (_uniqueURLs.Contains(input) || input.Host != _startUrl.Host)
        {
            return true;
        }

        foreach (var format in _disallowedFormats)
        {
            if (input.LocalPath.EndsWith(format))
            {
                return true;
            }
        }

        return false;
    }
}
