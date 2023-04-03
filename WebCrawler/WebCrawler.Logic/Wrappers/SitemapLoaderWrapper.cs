using Louw.SitemapParser;
using System;
using System.Threading.Tasks;

namespace WebCrawler.Logic.Wrappers
{
    public class SitemapLoaderWrapper
    {
        private readonly SitemapLoader _sitemapLoader;

        public SitemapLoaderWrapper()
        {
            _sitemapLoader = new SitemapLoader();
        }

        public virtual Task<Sitemap> LoadAsync(Uri url)
        {
            return _sitemapLoader.LoadAsync(url);
        }
    }
}
