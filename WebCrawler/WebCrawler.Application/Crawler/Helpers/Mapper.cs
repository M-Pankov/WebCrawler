using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebCrawler.Application.Crawler.Models;
using WebCrawler.Domain.CrawlResults;

namespace WebCrawler.Application.Crawler.Helpers;

public static class Mapper
{
    public static CrawledSiteDto CrawledSiteToDto(CrawledSite crawledSite)
    {
        return new CrawledSiteDto()
        {
            Id = crawledSite.Id,
            Url = crawledSite.Url,
            CrawlDate = crawledSite.CrawlDate,
        };
    }

    public static CrawledSiteUrlDto CrawledSiteUrlToDto(CrawledSiteUrl crawledSiteResult)
    {
        return new CrawledSiteUrlDto()
        {
            Id = crawledSiteResult.Id,
            Url = crawledSiteResult.Url,
            UrlFoundLocation = crawledSiteResult.UrlFoundLocation,
            ResponseTimeMs = crawledSiteResult.ResponseTimeMs,
        };
    }

    public static PagedList<CrawledSiteDto> CrawledSitesPagedListToDto(PagedList<CrawledSite> crawledSites)
    {
        return new PagedList<CrawledSiteDto>()
        {
            TotalCount = crawledSites.TotalCount,
            TotalPages = crawledSites.TotalPages,
            PageNumber = crawledSites.PageNumber,
            PageSize = crawledSites.PageSize,
            Items = crawledSites.Items.Select(x => CrawledSiteToDto(x)).ToList()
        };

    }
}
