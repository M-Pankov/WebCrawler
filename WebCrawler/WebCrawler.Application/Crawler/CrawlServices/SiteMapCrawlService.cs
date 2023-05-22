using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using WebCrawler.Application.Crawler.Loaders;
using WebCrawler.Domain.CrawlerModels;
using WebCrawler.Domain.CrawlResults;
using WebCrawler.Domain.Enums;

namespace WebCrawler.Application.Crawler.CrawlServices;

public class SiteMapCrawlService
{
    private readonly SiteMapLoader _sitemapLoaderService;

    public SiteMapCrawlService(SiteMapLoader sitemapLoader)
    {
        _sitemapLoaderService = sitemapLoader;
    }

    public virtual async Task<IEnumerable<CrawledSiteUrl>> CrawlSitemapAsync(Uri input)
    {
        var sitemapUrl = new Uri(input, "/sitemap.xml");

        return await LoadSitemapUrlsAsync(sitemapUrl);
    }

    private async Task<IEnumerable<CrawledSiteUrl>> LoadSitemapUrlsAsync(Uri sitemapUrl)
    {
        var sitemap = await _sitemapLoaderService.LoadAsync(sitemapUrl);

        var linksFromSitemap = sitemap.Items.Select(x => HttpUtility.UrlDecode(x.Location.ToString().ToLower().TrimEnd('/')));

        return linksFromSitemap.Distinct()
            .Select(x => new CrawledSiteUrl()
            {
                Url = new Uri(x),
                UrlFoundLocation = UrlFoundLocation.Sitemap
            });
    }
}
