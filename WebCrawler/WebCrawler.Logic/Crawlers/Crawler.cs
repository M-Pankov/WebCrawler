using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebCrawler.Logic.Enums;
using WebCrawler.Logic.Loaders;
using WebCrawler.Logic.Models;

namespace WebCrawler.Logic.Crawlers;

public class Crawler
{
    private readonly SiteCrawler _siteCrawler;
    private readonly SitemapCrawler _sitemapCrawler;
    private readonly HtmlLoader _htmlLoader;

    public Crawler(SiteCrawler siteCrawler, SitemapCrawler sitemapCrawler, HtmlLoader htmlLoader)
    {
        _siteCrawler = siteCrawler;
        _sitemapCrawler = sitemapCrawler;
        _htmlLoader = htmlLoader;
    }

    public async Task<IEnumerable<UrlWithResponseTime>> FoundAllPagesWithResponseTime(Uri input)
    {
        var crawledUrls = await _siteCrawler.GetUrlsWithResponseTimeAsync(input);

        var sitemapUrls = await _sitemapCrawler.GetLinksFromSitemapAsync(input);

        var fullUrls = GetFullUrlsList(crawledUrls, sitemapUrls);

        return await AddUrlsResponseTimeIfNotExsit(fullUrls);
    }

    private IEnumerable<UrlWithResponseTime> GetFullUrlsList(IEnumerable<UrlWithResponseTime> crawledUrls, IEnumerable<UrlWithResponseTime> sitemapUrls)
    {
        var upadatedUrls = UpdateUrlsFoundLocation(crawledUrls, sitemapUrls).ToList();

        var onlySitemapUrls = sitemapUrls.Where(x => !crawledUrls.Any(y => y.Url == x.Url));

        upadatedUrls.AddRange(onlySitemapUrls);

        return upadatedUrls;
    }

    private IEnumerable<UrlWithResponseTime> UpdateUrlsFoundLocation(IEnumerable<UrlWithResponseTime> crawledUrls, IEnumerable<UrlWithResponseTime> urlsFromSitemap)
    {
        foreach (var crawledUrl in crawledUrls)
        {
            if (!urlsFromSitemap.Any(x => x.Url == crawledUrl.Url))
            {
                continue;
            }

            crawledUrl.UrlFoundLocation = UrlFoundLocation.SiteAndSitemap;
        }

        return crawledUrls;
    }

    private async Task<IEnumerable<UrlWithResponseTime>> AddUrlsResponseTimeIfNotExsit(IEnumerable<UrlWithResponseTime> upadatedUrls)
    {
        foreach (var url in upadatedUrls.Where(x => !x.ResponseTime.HasValue))
        {
            var htmlContentWithResponseTime = await _htmlLoader.GetHtmlContentWithResponseTimeAsync(url.Url);

            url.ResponseTime = htmlContentWithResponseTime.ResponseTime;
        }

        return upadatedUrls;
    }
}
