using System;
using System.Collections.Generic;
using System.Linq;
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
        var crawledSite = _crawledSiteRepository.GetAll().First(x => x.Id == id);

        return Mapper.CrawledSiteToViewModel(crawledSite);
    }

    public IEnumerable<CrawledSiteResultViewModel> GetCrawledSiteResultsById(int id)
    {
        var crawledSiteResults = _crawlSiteResultRepository.GetAll().Where(x => x.CrawledSiteId == id);

        return crawledSiteResults.Select(x => Mapper.CrawledSiteResultToViewModel(x));
    }

    public void SaveSiteCrawlResult(Uri baseUrl, IEnumerable<CrawledUrl> results)
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

        _crawledSiteRepository.SaveChanges();
    }
}
