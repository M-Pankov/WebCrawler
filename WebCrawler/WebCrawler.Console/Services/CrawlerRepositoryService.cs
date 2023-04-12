using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebCrawler.Logic.Models;
using WebCrawler.Persistence.Entities;
using WebCrawler.Persistence.Repositories;

namespace WebCrawler.Console.Services;

public class CrawlerRepositoryService
{
    private readonly ICrawledSiteRepository _crawledSiteRepository;
    private readonly ICrawledSiteResultRepository _crawledSiteResultRepository;
    public CrawlerRepositoryService(ICrawledSiteRepository crawledSiteRepository, ICrawledSiteResultRepository crawledSiteResultRepositoryRepository)
    {
        _crawledSiteRepository = crawledSiteRepository;
        _crawledSiteResultRepository = crawledSiteResultRepositoryRepository;
    }

    public async Task SaveCrawlResultAsync(Uri uriInput, IEnumerable<CrawledUrl> results)
    {
        var crawledSite = new CrawledSite()
        {
            Url = uriInput,
            CrawlDate = DateTime.Now
        };

        await _crawledSiteRepository.AddAsync(crawledSite);

        var siteUrlCrawlResults = results.Select(x => new CrawledSiteResult()
        {
            Url = x.Url,
            ResponseTimeMs = x.ResponseTimeMs,
            UrlFoundLocation = x.UrlFoundLocation,
            CrawledSite = crawledSite,
        });

        await _crawledSiteResultRepository.AddRangeAsync(siteUrlCrawlResults);

        await _crawledSiteRepository.SaveChangesAsync();
    }
}
