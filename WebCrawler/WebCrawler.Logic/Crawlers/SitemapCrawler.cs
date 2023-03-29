using Louw.SitemapParser;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace WebCrawler.Logic.Crawlers;

public class SitemapCrawler
{
    private readonly SitemapLoader _sitemapLoader;
    public SitemapCrawler()
    {
        _sitemapLoader = new SitemapLoader();
    }

    public async Task<IList<string>> GetLinksFromSitemapAsync(Uri input)
    {
        var sitemapUrl = new Uri(input, "/sitemap.xml");

        var linksFromSitemap = await LoadSitemapLinksAsync(sitemapUrl);

        return linksFromSitemap.Distinct().ToList();
    }

    private async Task<IEnumerable<string>> LoadSitemapLinksAsync(Uri sitemapUrl)
    {
        var sitemap = new Sitemap(sitemapUrl);

        var loadedSitemap = await _sitemapLoader.LoadAsync(sitemap);

        var urlsFromSitemap = loadedSitemap.Items.Select(x => HttpUtility.UrlDecode(x.Location.ToString().ToLower()));

        return urlsFromSitemap;
    }
}
