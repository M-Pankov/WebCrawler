using Louw.SitemapParser;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace WebCrawlService;

public class SitemapCrawler
{
    public async Task<IList<string>> GetLinksFromSitemap(Uri input)
    {
        var sitemapUrl = new Uri(input, "/sitemap.xml");

        var linksFromSitemap = await LoadSitemapLinks(sitemapUrl);

        return linksFromSitemap.Distinct().ToList();
    }

    private async Task<IEnumerable<string>> LoadSitemapLinks(Uri sitemapUrl)
    {
        SitemapLoader sitemapLoader = new SitemapLoader();

        var sitemap = new Sitemap(sitemapUrl);

        var loadedSitemap = await sitemapLoader.LoadAsync(sitemap);

        var urlsFromSitemap = loadedSitemap.Items.Select(x => HttpUtility.UrlDecode(x.Location.ToString().ToLower()));

        return urlsFromSitemap;
    }
}
