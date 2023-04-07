using System.Collections.Generic;
using System;
using WebCrawler.Persistence.Entities;

namespace WebCrawler.MVC.ViewModels;

public class CrawledSiteVm : BaseUrlVm
{
    public CrawledSiteVm()
    {
        SiteCrawlResult = new List<CrawledSiteResultVm>();
    }

    public DateTime CrawlDate { get; set; }
    public IEnumerable<CrawledSiteResultVm> SiteCrawlResult { get; set; }
}
