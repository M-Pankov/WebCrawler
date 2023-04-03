using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebCrawler.Logic.Enums;
using WebCrawler.Logic.Models;
using WebCrawler.Logic.Parsers;
using WebCrawler.Logic.Services;
using WebCrawler.Logic.Validators;

namespace WebCrawler.Logic.Crawlers;

public class SiteCrawler
{
    private readonly HtmlParser _htmlParser;
    private readonly UrlValidator _urlValidator;
    private readonly HtmlLoaderService _htmlLoaderService;

    public SiteCrawler(HtmlParser htmlParser, UrlValidator urlValidator, HtmlLoaderService htmlLoaderService)
    {
        _htmlParser = htmlParser;
        _urlValidator = urlValidator;
        _htmlLoaderService = htmlLoaderService;
    }

    public virtual async Task<IEnumerable<CrawledUrl>> CrawlSiteAsync(Uri input)
    {
        var startUrl = new CrawledUrl()
        {
            Url = input,
            UrlFoundLocation = UrlFoundLocation.Site
        };

        return await CrawlUrlAsync(startUrl);
    }

    private async Task<IEnumerable<CrawledUrl>> CrawlUrlAsync(CrawledUrl urlToCrawl)
    {
        var crawledUrls = new List<CrawledUrl>
        {
            urlToCrawl
        };

        while (urlToCrawl != null)
        {
            var httpResponse = await _htmlLoaderService.GetHttpResponseAsync(urlToCrawl.Url);

            urlToCrawl.ResponseTime = httpResponse.ResponseTime;

            var newUrls = GetNewUrls(crawledUrls, urlToCrawl, httpResponse.HtmlContent);

            crawledUrls.AddRange(newUrls);

            urlToCrawl = crawledUrls.FirstOrDefault(x => !x.ResponseTime.HasValue);
        }

        return crawledUrls;
    }

    private IEnumerable<CrawledUrl> GetNewUrls(IEnumerable<CrawledUrl> crawledUrls, CrawledUrl urlToCrawl, string htmlContent)
    {
        var vaidUrlsFromPage = _htmlParser.GetLinks(urlToCrawl.Url, htmlContent)
           .Where(x => _urlValidator.IsAllowed(x, urlToCrawl.Url));

        return vaidUrlsFromPage.Where(x => !crawledUrls.Any(y => y.Url == x))
            .Select(x => new CrawledUrl
            {
                Url = x,
                UrlFoundLocation = UrlFoundLocation.Site
            });
    }
}
