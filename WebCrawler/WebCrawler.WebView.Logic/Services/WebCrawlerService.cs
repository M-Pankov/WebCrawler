using System;
using System.Linq;
using System.Threading.Tasks;
using WebCrawler.Logic.Crawlers;
using WebCrawler.Logic.Enums;
using WebCrawler.Persistence.Entities;
using WebCrawler.WebView.Logic.Helpers;
using WebCrawler.WebView.Logic.Validators;
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

    public PagedList<CrawledSiteViewModel> GetCrawledSitesPagedList(int pageNumber, int pageSize)
    {
        return _crawlerRepositoryService.GetAllCrawledSites(pageNumber, pageSize);
    }

    public async Task<CrawledSiteViewModel> GetCrawledSiteResultsAsync(int id)
    {
        var crawledSite = await _crawlerRepositoryService.GetCrawledSiteByIdAsync(id);

        crawledSite.SiteCrawlResults = crawledSite.SiteCrawlResults;
        crawledSite.OnlySiteResults = crawledSite.SiteCrawlResults.Where(x => x.UrlFoundLocation == UrlFoundLocation.Site);
        crawledSite.OnlySitemapResults = crawledSite.SiteCrawlResults.Where(x => x.UrlFoundLocation == UrlFoundLocation.Sitemap);

        return crawledSite;
    }

    public async Task CrawlSiteAsync(string input)
    {
        var uriInput = new Uri(input);

        var crawlResult = await _crawler.CrawlUrlsAsync(uriInput);

        await _crawlerRepositoryService.SaveSiteCrawlResultAsync(uriInput, crawlResult);
    }
}
