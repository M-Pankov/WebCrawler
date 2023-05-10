using System.Linq;
using WebCrawler.Persistence.Entities;
using WebCrawler.Web.Logic.ViewModels;

namespace WebCrawler.Web.Logic.Helpers;

public static class Mapper
{
    public static CrawledSiteViewModel CrawledSiteToViewModel(CrawledSite crawledSite)
    {
        return new CrawledSiteViewModel()
        {
            Id = crawledSite.Id,
            Url = crawledSite.Url,
            CrawlDate = crawledSite.CrawlDate,
        };
    }

    public static CrawledSiteResultViewModel CrawledSiteResultToViewModel(CrawledSiteResult crawledSiteResult)
    {
        return new CrawledSiteResultViewModel()
        {
            Id = crawledSiteResult.Id,
            Url = crawledSiteResult.Url,
            UrlFoundLocation = crawledSiteResult.UrlFoundLocation,
            ResponseTimeMs = crawledSiteResult.ResponseTimeMs,
        };
    }

    public static PagedList<CrawledSiteViewModel> CrawledSitesPagedListToViewModel(PagedList<CrawledSite> crawledSites)
    {
        var pagedList = new PagedList<CrawledSiteViewModel>()
        {
            TotalCount = crawledSites.TotalCount,
            TotalPages = crawledSites.TotalPages,
            PageNumber = crawledSites.PageNumber,
            PageSize = crawledSites.PageSize,
        };

        pagedList.AddRange(crawledSites.Select(x => CrawledSiteToViewModel(x)));

        return pagedList;
    }
}
