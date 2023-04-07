using WebCrawler.Logic.Enums;

namespace WebCrawler.Persistence.Entities;

public class SiteUrlCrawlResult : BaseUrlEntity
{
    public long? ResponseTimeMs { get; set; }
    public UrlFoundLocation UrlFoundLocation { get; set; }
    public int CrawledSiteId { get; set; }
    public CrawledSite CrawledSite { get; set; }
}
