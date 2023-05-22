using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebCrawler.Application.Crawler.Loaders;
using WebCrawler.Domain.CrawlResults;
using WebCrawler.Domain.Enums;

namespace WebCrawler.Application.Crawler.CrawlServices;

public class CrawlService
{
    private readonly SiteCrawlService _siteCrawlService;
    private readonly SiteMapCrawlService _siteMapCrawlService;
    private readonly HtmlLoader _htmlLoader;
    public CrawlService(SiteCrawlService siteCrawlService, SiteMapCrawlService siteMapCrawlService, HtmlLoader htmlLoader)
    {
        _htmlLoader = htmlLoader;
        _siteMapCrawlService = siteMapCrawlService;
        _siteCrawlService = siteCrawlService;
    }

    public virtual async Task<IEnumerable<CrawledSiteUrl>> CrawlUrlsAsync(Uri input)
    {
        var siteUrls = await _siteCrawlService.CrawlSiteAsync(input);

        var sitemapUrls = await _siteMapCrawlService.CrawlSitemapAsync(input);

        var allUrls = GetAllUrls(siteUrls, sitemapUrls);

        return await AddResponseTimeAsync(allUrls);
    }

    private IEnumerable<CrawledSiteUrl> GetAllUrls(IEnumerable<CrawledSiteUrl> siteUrls, IEnumerable<CrawledSiteUrl> sitemapUrls)
    {
        var updatedUrls = UpdateUrlsFoundLocation(siteUrls, sitemapUrls).ToList();

        var onlySitemapUrls = sitemapUrls.Where(x => !siteUrls.Any(y => y.Url == x.Url));

        updatedUrls.AddRange(onlySitemapUrls);

        return updatedUrls;
    }

    private IEnumerable<CrawledSiteUrl> UpdateUrlsFoundLocation(IEnumerable<CrawledSiteUrl> siteUrls, IEnumerable<CrawledSiteUrl> sitemapUrls)
    {
        foreach (var siteUrl in siteUrls)
        {
            if (!sitemapUrls.Any(x => x.Url == siteUrl.Url))
            {
                continue;
            }

            siteUrl.UrlFoundLocation = UrlFoundLocation.Both;
        }

        return siteUrls;
    }

    private async Task<IEnumerable<CrawledSiteUrl>> AddResponseTimeAsync(IEnumerable<CrawledSiteUrl> updatedUrls)
    {
        foreach (var updatedUrl in updatedUrls.Where(x => !x.ResponseTimeMs.HasValue))
        {
            var httpResponse = await _htmlLoader.GetHttpResponseAsync(updatedUrl.Url);

            updatedUrl.ResponseTimeMs = httpResponse.ResponseTimeMs;
        }

        return updatedUrls;
    }
}
