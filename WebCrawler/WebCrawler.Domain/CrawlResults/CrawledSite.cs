using System;
using System.Collections.Generic;

namespace WebCrawler.Domain.CrawlResults;

public class CrawledSite : BaseUrlEntity
{
    public CrawledSite()
    {
        CrawlResults = new List<CrawledSiteUrl>();
    }

    public DateTime CrawlDate { get; set; }
    public IEnumerable<CrawledSiteUrl> CrawlResults { get; set; }
}
