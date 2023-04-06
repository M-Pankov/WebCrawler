using System;
using System.Collections.Generic;
using WebCrawler.Persistence.Entities;

namespace WebCrawler.Persistence.Entities;

public class CrawledSite : BaseEntity
{
    public CrawledSite()
    {
        CrawledPages = new List<CrawledSitePage>();
    }

    public DateTime CrawlDate { get; set; }
    public IEnumerable<CrawledSitePage> CrawledPages { get; set; }
}
