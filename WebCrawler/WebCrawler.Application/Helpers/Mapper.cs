using System.Linq;
using WebCrawler.Application.Models;
using WebCrawler.Domain.CrawlResults;

namespace WebCrawler.Application.Helpers;

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
