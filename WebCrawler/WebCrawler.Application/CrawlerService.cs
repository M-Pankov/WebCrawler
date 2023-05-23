using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebCrawler.Application.Helpers;
using WebCrawler.Application.Interfaces;
using WebCrawler.Application.Models;
using WebCrawler.Domain.CrawlResults;
using WebCrawler.Domain.Enums;

namespace WebCrawler.Application;

public class CrawlerService
{
    private readonly ICrawledSiteRepository _crawledSiteRepository;
    private readonly ICrawledSiteUrlRepository _crawledSiteUrlRepository;
    private readonly ICrawler _crawler;
    public CrawlerService(ICrawledSiteRepository crawledSiteRepository, ICrawledSiteUrlRepository crawledSiteUrlRepository, ICrawler crawler)
    {
        _crawledSiteRepository = crawledSiteRepository;
        _crawledSiteUrlRepository = crawledSiteUrlRepository;
        _crawler = crawler;
    }

    public virtual async Task<int> CrawlSiteAsync(string input)
    {
        var uriInput = new Uri(input);

        var crawlResult = await _crawler.CrawlUrlsAsync(uriInput);

        return await SaveSiteCrawlResultAsync(uriInput, crawlResult);
    }

    public virtual PagedList<CrawledSiteDto> GetCrawledSitesPagedList(int pageNumber, int pageSize)
    {
        var crawledSites = _crawledSiteRepository.GetAll().OrderByDescending(x => x.CrawlDate);

        var crawledSitesList = Mapper.CrawledSitesPagedListToDto(new PagedList<CrawledSite>(crawledSites, pageNumber, pageSize));

        return crawledSitesList;
    }

    public virtual async Task<CrawledSiteDto> GetCrawledSiteResultsAsync(int id)
    {
        var crawledSite = await _crawledSiteRepository.GetCrawledSiteByIdAsync(id);

        var crawledSiteDto = Mapper.CrawledSiteToDto(crawledSite);

        crawledSiteDto.SiteCrawlResults = crawledSite.CrawlResults.OrderBy(x => x.ResponseTimeMs)
            .Select(x => Mapper.CrawledSiteUrlToDto(x));

        crawledSiteDto.SiteCrawlResults = crawledSiteDto.SiteCrawlResults;
        crawledSiteDto.OnlySiteResults = crawledSiteDto.SiteCrawlResults.Where(x => x.UrlFoundLocation == UrlFoundLocation.Site);
        crawledSiteDto.OnlySitemapResults = crawledSiteDto.SiteCrawlResults.Where(x => x.UrlFoundLocation == UrlFoundLocation.Sitemap);

        return crawledSiteDto;
    }

    private async Task<int> SaveSiteCrawlResultAsync(Uri baseUrl, IEnumerable<CrawledSiteUrl> results)
    {
        var crawledSite = new CrawledSite()
        {
            Url = baseUrl,
            CrawlDate = DateTime.Now
        };

        await _crawledSiteRepository.AddAsync(crawledSite);

        var siteUrlCrawlResults = results.Select(x => new CrawledSiteUrl()
        {
            Url = x.Url,
            ResponseTimeMs = x.ResponseTimeMs,
            UrlFoundLocation = x.UrlFoundLocation,
            CrawledSite = crawledSite,
        });

        await _crawledSiteUrlRepository.AddRangeAsync(siteUrlCrawlResults);

        await _crawledSiteRepository.SaveChangesAsync();

        return crawledSite.Id;
    }
}
