using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebCrawler.Logic.Enums;
using WebCrawler.Logic.Loaders;
using WebCrawler.Logic.Models;
using WebCrawler.Logic.Parsers;
using WebCrawler.Logic.Validators;

namespace WebCrawler.Logic.Crawlers;

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

    public SiteCrawler()
    {
        _htmlLoader = new HtmlLoader();
        _htmlParser = new HtmlParser();
        _urlValidator = new UrlValidator();
    }

    public virtual async Task<IEnumerable<UrlWithResponseTime>> GetUrlsWithResponseTimeAsync(Uri input)
    {
        var startUrl = new UrlWithResponseTime()
        {
            Url = input,
            UrlFoundLocation = UrlFoundLocation.Site
        };

        return await CrawlUrlAsync(startUrl);
    }

    private async Task<IEnumerable<UrlWithResponseTime>> CrawlUrlAsync(UrlWithResponseTime urlToCrawl)
    {
        var crawledUrls = new List<UrlWithResponseTime>
        {
            urlToCrawl
        };

        while (urlToCrawl != null)
        {
            var htmlContentWithResponseTime = await _htmlLoader.GetHtmlContentWithResponseTimeAsync(urlToCrawl.Url);

            urlToCrawl.ResponseTime = htmlContentWithResponseTime.ResponseTime;

            var newUrls = FilterNewUrlsFromHtmlContent(crawledUrls, urlToCrawl, htmlContentWithResponseTime.HtmlContent);

            crawledUrls.AddRange(newUrls);

            urlToCrawl = crawledUrls.FirstOrDefault(x => !x.ResponseTime.HasValue);
        }

        return crawledUrls;
    }

    private IEnumerable<UrlWithResponseTime> FilterNewUrlsFromHtmlContent(IEnumerable<UrlWithResponseTime> currentLinks, UrlWithResponseTime input, string htmlContent)
    {
        var currentUrls = currentLinks.Select(x => x.Url);

        var vaidUrlsFromPage = _htmlParser.GetLinks(input.Url, htmlContent)
           .Where(x => !_urlValidator.IsDisallowed(x, input.Url));

        return vaidUrlsFromPage.Where(x => !currentUrls.Contains(x))
            .Select(x => new UrlWithResponseTime()
            {
                Url = x,
                UrlFoundLocation = UrlFoundLocation.Site
            });
    }
}
