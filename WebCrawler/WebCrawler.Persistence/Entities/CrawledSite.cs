using System;
using System.Collections.Generic;
using WebCrawler.Persistence.Entities;

namespace WebCrawler.Persistence.Entities;

public class CrawledSite : BaseUrlEntity
{
    public CrawledSite()
    {
        CrawledUrls = new List<SiteUrlCrawlResult>();
    }

    public DateTime CrawlDate { get; set; }
    public IEnumerable<SiteUrlCrawlResult> CrawledUrls { get; set; }
}
