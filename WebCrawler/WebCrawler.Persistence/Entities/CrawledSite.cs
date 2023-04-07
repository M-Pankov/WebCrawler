using System;
using System.Collections.Generic;
using WebCrawler.Persistence.Entities;

namespace WebCrawler.Persistence.Entities;

public class CrawledSite : BaseUrlEntity
{
    public CrawledSite()
    {
        CrawlResults = new List<CrawledSiteResult>();
    }

    public DateTime CrawlDate { get; set; }
    public IEnumerable<CrawledSiteResult> CrawlResults { get; set; }
}
