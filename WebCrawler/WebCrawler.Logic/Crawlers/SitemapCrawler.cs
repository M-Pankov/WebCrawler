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
        var sitemap = await _sitemapLoader.LoadAsync(sitemapUrl);

        var linksFromSitemap = sitemap.Items.Select(x => HttpUtility.UrlDecode(x.Location.ToString().ToLower()));

        return linksFromSitemap;
    }
}
