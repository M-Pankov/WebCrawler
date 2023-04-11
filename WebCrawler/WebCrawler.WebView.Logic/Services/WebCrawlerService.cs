using System;
using System.Linq;
using System.Threading.Tasks;
using WebCrawler.Logic.Crawlers;
using WebCrawler.Persistence.Entities;
using WebCrawler.WebView.Logic.Helpers;
using WebCrawler.WebView.Logic.ViewModels;

namespace WebCrawler.WebView.Logic.Services;

public class WebCrawlerService
{
    private readonly CrawlerRepositoryService _crawlerRepositoryService;
    private readonly Crawler _crawler;
    public WebCrawlerService(CrawlerRepositoryService crawlerRepositoryService, Crawler crawler)
    {
        _crawlerRepositoryService = crawlerRepositoryService;
        _crawler = crawler;
    }

    public PagedList<CrawledSiteViewModel> GetCrawledSitesPagedList(int? pageNumber, int? pageSize)
    {
        var crawledSites = _crawlerRepositoryService.GetAllCrawledSites();

        var crawledSitesList = Mapper.CrawledSitesPagedListToViewModel(new PagedList<CrawledSite>(crawledSites, pageNumber, pageSize));

        return crawledSitesList;
    }

    public CrawledSiteViewModel GetCrawledSiteResults(int id)
    {
        var crawledSite = _crawlerRepositoryService.GetCrawledSiteById(id);

        crawledSite.SiteCrawlResult = crawledSite.SiteCrawlResult;
        crawledSite.OnlySiteResults = crawledSite.SiteCrawlResult.Where(x => x.UrlFoundLocation == WebCrawler.Logic.Enums.UrlFoundLocation.Site);
        crawledSite.OnlySitemapResults = crawledSite.SiteCrawlResult.Where(x => x.UrlFoundLocation == WebCrawler.Logic.Enums.UrlFoundLocation.Sitemap);

        return crawledSite;
    }

    public async Task CrawlSite(string input)
    {
        var uriInput = new Uri(input);

        var crawlResult = await _crawler.CrawlUrlsAsync(uriInput);

        await _crawlerRepositoryService.SaveSiteCrawlResult(uriInput, crawlResult);
    }
}
