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

        var compareResult = CompareToSitemap(crawledUrls, sitemapUrls);

        var result = await AddResponseTime(compareResult);

        return result;
    }

    private IEnumerable<UrlWithResponseTime> CompareToSitemap(IEnumerable<UrlWithResponseTime> crawledUrls, IEnumerable<UrlWithResponseTime> urlsFromSitemap)
    {
        foreach (var crawledUrl in crawledUrls)
        {
            var urlInSitemap = urlsFromSitemap.FirstOrDefault(x => x.Url == crawledUrl.Url);

            if (urlInSitemap == null)
            {
                continue;
            }

            crawledUrl.FoundFrom = UrlFoundFrom.Both;
        }

        var resultList = crawledUrls.ToList();

        var onlyInSitemapUrls = urlsFromSitemap.Where(x => !resultList.Exists(c => c.Url == x.Url));

        resultList.AddRange(onlyInSitemapUrls);

        return resultList;
    }

    private async Task<IEnumerable<UrlWithResponseTime>> AddResponseTime(IEnumerable<UrlWithResponseTime> urls)
    {
        var urlWithoutTimings = urls.Where(x => !x.ResponseTime.HasValue);

        foreach (var url in urlWithoutTimings)
        {
            var contentWithTiming = await _htmlLoader.GetHtmlContentWithResponseTimeAsync(url.Url);
            url.ResponseTime = contentWithTiming.ResponseTime;
        }

        return urls;
    }
}
