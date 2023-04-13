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
    private readonly ICrawledSiteResultRepository _crawledSiteResultRepository;

    public CrawlerRepositoryService(ICrawledSiteRepository crawledSiteRepository, ICrawledSiteResultRepository crawledSiteResultRepository)
    {
        _crawledSiteRepository = crawledSiteRepository;
        _crawledSiteResultRepository = crawledSiteResultRepository;
    }

    public PagedList<CrawledSiteViewModel> GetCrawledSitesPagedList(int pageNumber, int pageSize)
    {
        var crawledSites = _crawledSiteRepository.GetAll().OrderByDescending(x => x.CrawlDate);

        var crawledSitesList = Mapper.CrawledSitesPagedListToViewModel(new PagedList<CrawledSite>(crawledSites, pageNumber, pageSize));

        return crawledSitesList;
    }

    public async Task<CrawledSiteViewModel> GetCrawledSiteByIdAsync(int id)
    {
        var crawledSite = await _crawledSiteRepository.GetCrawledSiteByIdAsync(id);

        var crawledSiteViewModel = Mapper.CrawledSiteToViewModel(crawledSite);

        crawledSiteViewModel.SiteCrawlResults = crawledSite.CrawlResults.OrderBy(x => x.ResponseTimeMs)
            .Select(x => Mapper.CrawledSiteResultToViewModel(x));

        return crawledSiteViewModel;
    }

    public async Task SaveSiteCrawlResultAsync(Uri baseUrl, IEnumerable<CrawledUrl> results)
    {
        var crawledSite = new CrawledSite()
        {
            Url = baseUrl,
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
