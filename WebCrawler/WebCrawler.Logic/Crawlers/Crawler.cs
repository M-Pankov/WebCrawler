using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebCrawler.Logic.Enums;
using WebCrawler.Logic.Models;
using WebCrawler.Logic.Services;

namespace WebCrawler.Logic.Crawlers;

public class Crawler
{
    private readonly SiteCrawler _siteCrawler;
    private readonly SitemapCrawler _sitemapCrawler;
    private readonly HtmlLoaderService _htmlLoaderService;

    public Crawler(SiteCrawler siteCrawler, SitemapCrawler sitemapCrawler, HtmlLoaderService htmlLoaderService)
    {
        _siteCrawler = siteCrawler;
        _sitemapCrawler = sitemapCrawler;
        _htmlLoaderService = htmlLoaderService;
    }

    public virtual async Task<IEnumerable<CrawledUrl>> CrawlUrlAsync(Uri input)
    {
        var siteUrls = await _siteCrawler.CrawlSiteAsync(input);

        var sitemapUrls = await _sitemapCrawler.CrawlSitemapAsync(input);

        var allUrls = GetAllUrls(siteUrls, sitemapUrls);

        return await AddResponseTimeAsync(allUrls);
    }

    private IEnumerable<CrawledUrl> GetAllUrls(IEnumerable<CrawledUrl> siteUrls, IEnumerable<CrawledUrl> sitemapUrls)
    {
        var updatedUrls = UpdateUrlsFoundLocation(siteUrls, sitemapUrls).ToList();

        var onlySitemapUrls = sitemapUrls.Where(x => !siteUrls.Any(y => y.Url == x.Url));

        updatedUrls.AddRange(onlySitemapUrls);

        return updatedUrls;
    }

    private IEnumerable<CrawledUrl> UpdateUrlsFoundLocation(IEnumerable<CrawledUrl> siteUrls, IEnumerable<CrawledUrl> sitemapUrls)
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

    private async Task<IEnumerable<CrawledUrl>> AddResponseTimeAsync(IEnumerable<CrawledUrl> updatedUrls)
    {
        foreach (var updatedUrl in updatedUrls.Where(x => !x.ResponseTime.HasValue))
        {
            var httpResponse = await _htmlLoaderService.GetHttpResponseAsync(updatedUrl.Url);

            updatedUrl.ResponseTime = httpResponse.ResponseTime;
        }

        return updatedUrls;
    }
}
