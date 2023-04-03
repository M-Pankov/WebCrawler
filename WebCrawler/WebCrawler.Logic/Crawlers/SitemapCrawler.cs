using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using WebCrawler.Logic.Enums;
using WebCrawler.Logic.Models;
using WebCrawler.Logic.Services;

namespace WebCrawler.Logic.Crawlers;

public class SitemapCrawler
{
    private readonly SitemapLoaderService _sitemapLoaderService;

    public SitemapCrawler(SitemapLoaderService sitemapLoader)
    {
        _sitemapLoaderService = sitemapLoader;
    }

    public virtual async Task<IEnumerable<CrawledUrl>> CrawlSitemapAsync(Uri input)
    {
        var sitemapUrl = new Uri(input, "/sitemap.xml");

        return await LoadSitemapUrlsAsync(sitemapUrl);
    }

    private async Task<IEnumerable<CrawledUrl>> LoadSitemapUrlsAsync(Uri sitemapUrl)
    {
        var sitemap = await _sitemapLoaderService.LoadAsync(sitemapUrl);

        var linksFromSitemap = sitemap.Items.Select(x => HttpUtility.UrlDecode(x.Location.ToString().ToLower().TrimEnd('/')));

        return linksFromSitemap.Distinct()
            .Select(x => new CrawledUrl()
            {
                Url = new Uri(x),
                UrlFoundLocation = UrlFoundLocation.Sitemap
            });
    }
}
