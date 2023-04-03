using Louw.SitemapParser;
using System;
using System.Threading.Tasks;

namespace WebCrawler.Logic.Services;

public class SitemapLoaderService
{
    private readonly SitemapLoader _sitemapLoader;

    public SitemapLoaderService()
    {
        _sitemapLoader = new SitemapLoader();
    }

    public virtual Task<Sitemap> LoadAsync(Uri url)
    {
        return _sitemapLoader.LoadAsync(url);
    }
}
