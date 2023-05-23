using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebCrawler.Application.Interfaces;
using WebCrawler.Crawlers.Loaders;
using WebCrawler.Crawlers.SubCrawlers;
using WebCrawler.Domain.CrawlResults;
using WebCrawler.Domain.Enums;

namespace WebCrawler.Crawlers;

public class Crawler : ICrawler
{
    private readonly SiteCrawler _siteCrawler;
    private readonly SiteMapCrawler _siteMapCrawler;
    private readonly HtmlLoader _htmlLoader;
    public Crawler(SiteCrawler siteCrawler, SiteMapCrawler siteMapCrawler, HtmlLoader htmlLoader)
    {
        _htmlLoader = htmlLoader;
        _siteMapCrawler = siteMapCrawler;
        _siteCrawler = siteCrawler;
    }

    public virtual async Task<IEnumerable<CrawledSiteUrl>> CrawlUrlsAsync(Uri input)
    {
        var siteUrls = await _siteCrawler.CrawlSiteAsync(input);

        var sitemapUrls = await _siteMapCrawler.CrawlSitemapAsync(input);

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
