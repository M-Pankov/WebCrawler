using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebCrawler.Crawlers.Loaders;
using WebCrawler.Crawlers.Parsers;
using WebCrawler.Crawlers.Validators;
using WebCrawler.Domain.CrawlResults;
using WebCrawler.Domain.Enums;

namespace WebCrawler.Crawlers.SubCrawlers;

public class SiteCrawler
{
    private readonly HtmlParser _htmlParser;
    private readonly UrlValidator _urlValidator;
    private readonly HtmlLoader _htmlLoader;

    public SiteCrawler(HtmlParser htmlParser, UrlValidator urlValidator, HtmlLoader htmlLoader)
    {
        _htmlParser = htmlParser;
        _urlValidator = urlValidator;
        _htmlLoader = htmlLoader;
    }

    public virtual async Task<IEnumerable<CrawledSiteUrl>> CrawlSiteAsync(Uri input)
    {
        var startUrl = new CrawledSiteUrl()
        {
            Url = input,
            UrlFoundLocation = UrlFoundLocation.Site
        };

        return await CrawlUrlAsync(startUrl);
    }

    private async Task<IEnumerable<CrawledSiteUrl>> CrawlUrlAsync(CrawledSiteUrl urlToCrawl)
    {
        var crawledUrls = new List<CrawledSiteUrl>
        {
            urlToCrawl
        };

        while (urlToCrawl != null)
        {
            var httpResponse = await _htmlLoader.GetHttpResponseAsync(urlToCrawl.Url);

            urlToCrawl.ResponseTimeMs = httpResponse.ResponseTimeMs;

            var newUrls = GetNewUrls(crawledUrls, urlToCrawl, httpResponse.HtmlContent);

            crawledUrls.AddRange(newUrls);

            urlToCrawl = crawledUrls.FirstOrDefault(x => !x.ResponseTimeMs.HasValue);
        }

        return crawledUrls;
    }

    private IEnumerable<CrawledSiteUrl> GetNewUrls(IEnumerable<CrawledSiteUrl> crawledUrls, CrawledSiteUrl urlToCrawl, string htmlContent)
    {
        var validUrls = _htmlParser.GetLinks(urlToCrawl.Url, htmlContent)
           .Where(x => _urlValidator.IsAllowed(x, urlToCrawl.Url));

        return validUrls.Where(x => !crawledUrls.Any(y => y.Url == x))
            .Select(x => new CrawledSiteUrl
            {
                Url = x,
                UrlFoundLocation = UrlFoundLocation.Site
            });
    }
}
