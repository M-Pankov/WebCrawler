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
    public CrawlerRepositoryService(ICrawledSiteRepository crawledSitesRepository)
    {
        _crawledSitesRepository = crawledSitesRepository;
    }

    public void SaveCrawlResult(Uri uriInput, IEnumerable<CrawledUrl> results)
    {
        var crawlResult = new CrawledSite()
        {
            Url = uriInput,
            CrawlDate = DateTime.Now,
            CrawledPages = results.Select(x => new CrawledSitePage()
            {
                Url = x.Url,
                ResponseTimeMs = x.ResponseTimeMs,
                UrlFoundLocation = x.UrlFoundLocation
            })
        };

        _crawledSitesRepository.Add(crawlResult);
        _crawledSitesRepository.SaveChanges();
    }
}
