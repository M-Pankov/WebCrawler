using WebCrawler.Domain.Enums;

namespace WebCrawler.Domain.CrawlResults;

public class CrawledSiteUrl : BaseUrlEntity
{
    public long? ResponseTimeMs { get; set; }
    public UrlFoundLocation UrlFoundLocation { get; set; }
    public int CrawledSiteId { get; set; }
    public CrawledSite CrawledSite { get; set; }
}
