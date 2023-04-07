using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using WebCrawler.Logic.Models;
using WebCrawler.MVC.ViewModels;
using WebCrawler.Persistence.Entities;
using WebCrawler.Persistence.Repositories;

namespace WebCrawler.MVC.Services.RepositotyServices;

public class CrawlerRepositoryService
{
    private readonly ICrawledSiteRepository _crawledSiteRepository;
    private readonly ICrawledSiteResultRepository _crawlSiteResultRepository;
    public CrawlerRepositoryService(ICrawledSiteRepository crawledSiteRepository, ICrawledSiteResultRepository crawlSiteResultRepository)
    {
        _crawledSiteRepository = crawledSiteRepository;
        _crawlSiteResultRepository = crawlSiteResultRepository;
    }

    public IEnumerable<CrawledSiteVm> GetAllCrawledSites()
    {
        var crawledSites = _crawledSiteRepository.GetAll();

        return crawledSites.Select(x => CrawledSiteToVm(x));
    }

    public CrawledSiteVm GetCrawledSiteResults(int id)
    {
        var crawledSite = _crawledSiteRepository.GetAll().First(x => x.Id == id);
        var crawledSiteVm = CrawledSiteToVm(crawledSite);
        var crawledSiteResults = _crawlSiteResultRepository.GetAll().Where(x => x.CrawledSiteId == crawledSiteVm.Id);
        crawledSiteVm.SiteCrawlResult = crawledSiteResults.OrderBy(x => x.ResponseTimeMs).Select(x => CrawledSiteResultToVm(x));
        return crawledSiteVm;
    }

    public int SaveSiteCrawlResult(Uri baseUrl, IEnumerable<CrawledUrl> results)
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

        return crawledSite.Id;
    }

    private CrawledSiteVm CrawledSiteToVm (CrawledSite crawledSite)
    {
        return new CrawledSiteVm()
        {
            Id = crawledSite.Id,
            Url = crawledSite.Url,
            CrawlDate = crawledSite.CrawlDate,
        };
    }

    private CrawledSiteResultVm CrawledSiteResultToVm(CrawledSiteResult crawledSiteResult)
    {
        return new CrawledSiteResultVm()
        {
            Id = crawledSiteResult.Id,
            Url = crawledSiteResult.Url,
            UrlFoundLocation = crawledSiteResult.UrlFoundLocation,
            ResponseTimeMs = crawledSiteResult.ResponseTimeMs,
        };
    }
}
