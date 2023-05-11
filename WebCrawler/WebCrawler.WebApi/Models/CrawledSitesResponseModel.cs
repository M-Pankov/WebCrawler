using System.Collections.Generic;
using WebCrawler.Web.Logic.Helpers;
using WebCrawler.Web.Logic.ViewModels;

namespace WebCrawler.WebApi.Models;

public class CrawledSitesResponseModel
{
    public CrawledSitesResponseModel(PagedList<CrawledSiteViewModel> crawledSites)
    {
        CrawledSites = crawledSites;
        PageNumber = crawledSites.PageNumber;
        PageSize = crawledSites.PageSize;
        TotalPages = crawledSites.TotalPages;
        TotalCount = crawledSites.TotalCount;
    }
    public IEnumerable<CrawledSiteViewModel> CrawledSites { get; set; }
    public int PageNumber { get; set; }
    public int PageSize { get; set; }
    public int TotalCount { get; set; }
    public int TotalPages { get; set; }
}
