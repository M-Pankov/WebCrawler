using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebCrawler.Logic.Models;
using WebCrawler.Persistence.Entities;
using WebCrawler.Persistence.Repositories;
using WebCrawler.WebView.Logic.Helpers;
using WebCrawler.WebView.Logic.ViewModels;

namespace WebCrawler.WebView.Logic.Services;

public class CrawlerRepositoryService
{
    private readonly ICrawledSiteRepository _crawledSiteRepository;
    private readonly ICrawledSiteResultRepository _crawlSiteResultRepository;
    public CrawlerRepositoryService(ICrawledSiteRepository crawledSiteRepository, ICrawledSiteResultRepository crawlSiteResultRepository)
    {
        _crawledSiteRepository = crawledSiteRepository;
        _crawlSiteResultRepository = crawlSiteResultRepository;
    }

    public IQueryable<CrawledSite> GetAllCrawledSites()
    {
        var crawledSites = _crawledSiteRepository.GetAll().OrderByDescending(x => x.CrawlDate);

        return crawledSites;
    }

    public CrawledSiteViewModel GetCrawledSiteById(int id)
    {
        var crawledSite = _crawledSiteRepository.GetCrawledSiteById(id);

        var crawledSiteViewModel = Mapper.CrawledSiteToViewModel(crawledSite);

        crawledSiteViewModel.SiteCrawlResult = crawledSite.CrawlResults.OrderBy(x => x.ResponseTimeMs)
            .Select(x => Mapper.CrawledSiteResultToViewModel(x));

        return crawledSiteViewModel;
    }

    public async Task SaveSiteCrawlResult(Uri baseUrl, IEnumerable<CrawledUrl> results)
    {
        var crawledSite = new CrawledSite()
        {
            Url = baseUrl,
            CrawlDate = DateTime.Now
        };

        _crawledSiteRepository.Add(crawledSite);

        var siteUrlCrawlResults = results.Select(x => new CrawledSiteResult()
        {
            Url = x.Url,
            ResponseTimeMs = x.ResponseTimeMs,
            UrlFoundLocation = x.UrlFoundLocation,
            CrawledSite = crawledSite,
        });

        _crawlSiteResultRepository.AddRange(siteUrlCrawlResults);

        await _crawledSiteRepository.SaveChangesAsync();
    }
}
