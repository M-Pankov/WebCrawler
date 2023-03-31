using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using WebCrawler.Logic.Enums;
using WebCrawler.Logic.Models;
using WebCrawler.Logic.Wrappers;

namespace WebCrawler.Logic.Crawlers;

public class SitemapCrawler
{
    private readonly SitemapLoaderWrapper _sitemapLoaderWrapper;

    public SitemapCrawler()
    {
        _sitemapLoaderWrapper = new SitemapLoaderWrapper();
    }

    public SitemapCrawler(SitemapLoaderWrapper sitemapLoaderWrapper)
    {
        _sitemapLoaderWrapper = sitemapLoaderWrapper;
    }

    public virtual async Task<IEnumerable<UrlWithResponseTime>> GetLinksFromSitemapAsync(Uri input)
    {
        var sitemapUrl = new Uri(input, "/sitemap.xml");

        return await LoadSitemapUrlsAsync(sitemapUrl);
    }

    private async Task<IEnumerable<UrlWithResponseTime>> LoadSitemapUrlsAsync(Uri sitemapUrl)
    {
        var sitemap = await _sitemapLoaderWrapper.LoadAsync(sitemapUrl);

        var linksFromSitemap = sitemap.Items.Select(x => HttpUtility.UrlDecode(x.Location.ToString().ToLower().TrimEnd('/')));

        return linksFromSitemap.Distinct()
            .Select(x => new UrlWithResponseTime()
            {
                Url = new Uri(x),
                UrlFoundLocation = UrlFoundLocation.Sitemap
            });
    }
}
