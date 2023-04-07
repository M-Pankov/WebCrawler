using System;
using System.Collections.Generic;
using System.Linq;
using WebCrawler.Logic.Models;
using WebCrawler.Persistence.Entities;
using WebCrawler.Persistence.Repositories;

namespace WebCrawler.Console.Services;

public class CrawlerRepositoryService
{
    private readonly ICrawledSiteRepository _crawledSitesRepository;
    private readonly ICrawledSiteResultRepository _siteUrlCrawlResultRepository;
    public CrawlerRepositoryService(ICrawledSiteRepository crawledSitesRepository, ICrawledSiteResultRepository siteUrlCrawlResultRepository)
    {
        _crawledSitesRepository = crawledSitesRepository;
        _siteUrlCrawlResultRepository = siteUrlCrawlResultRepository;
    }

    public void SaveCrawlResult(Uri uriInput, IEnumerable<CrawledUrl> results)
    {
        var crawledSite = new CrawledSite()
        {
            Url = uriInput,
            CrawlDate = DateTime.Now
        };

        _crawledSitesRepository.Add(crawledSite);

        var siteUrlCrawlResults = results.Select(x => new CrawledSiteResult()
        {
            Url = x.Url,
            ResponseTimeMs = x.ResponseTimeMs,
            UrlFoundLocation = x.UrlFoundLocation,
            CrawledSite = crawledSite,
        });

        _siteUrlCrawlResultRepository.AddRange(siteUrlCrawlResults);

        _crawledSitesRepository.SaveChanges();
    }
}
