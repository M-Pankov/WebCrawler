using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebCrawler.Logic.Crawlers;
using WebCrawler.MVC.Services.RepositotyServices;
using WebCrawler.MVC.ViewModels;

namespace WebCrawler.MVC.Services.ControllerServices;

public class WebCrawlerControllerService
{
    private readonly CrawlerRepositoryService _crawlerRepositoryService;
    private readonly Crawler _crawler;
    public WebCrawlerControllerService(CrawlerRepositoryService crawlerRepositoryService, Crawler crawler)
    {
        _crawlerRepositoryService = crawlerRepositoryService;
        _crawler = crawler;
    }

    public IEnumerable<CrawledSiteVm> GetCrawledSites()
    {
        return _crawlerRepositoryService.GetAllCrawledSites().OrderByDescending(x => x.CrawlDate);
    }

    public CrawledSiteVm GetCrawledSiteResult(int id)
    {
        return _crawlerRepositoryService.GetCrawledSiteResults(id);
    }

    public async Task CrawlSite(string input)
    {
        var uriInput = new Uri(input);

        var crawlResult = await _crawler.CrawlUrlsAsync(uriInput);

        _crawlerRepositoryService.SaveSiteCrawlResult(uriInput, crawlResult);
    }
}
