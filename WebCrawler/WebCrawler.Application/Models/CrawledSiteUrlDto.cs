using WebCrawler.Domain.Enums;

namespace WebCrawler.Application.Models;

public class CrawledSiteUrlDto : BaseUrlDto
{
    public long? ResponseTimeMs { get; set; }
    public UrlFoundLocation UrlFoundLocation { get; set; }
}
