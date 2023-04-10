using System;
using System.Collections.Generic;

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
