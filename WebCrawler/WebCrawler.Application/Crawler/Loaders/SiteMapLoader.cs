using Louw.SitemapParser;
using System;
using System.Threading.Tasks;

namespace WebCrawler.Application.Crawler.Loaders;

public class SiteMapLoader
{
    private readonly SitemapLoader _sitemapLoader;

    public SiteMapLoader()
    {
        _sitemapLoader = new SitemapLoader();
    }

    public virtual Task<Sitemap> LoadAsync(Uri url)
    {
        return _sitemapLoader.LoadAsync(url);
    }
}