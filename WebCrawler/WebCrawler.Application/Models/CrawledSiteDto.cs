using System;
using System.Collections.Generic;

namespace WebCrawler.Application.Models;

public class CrawledSiteDto : BaseUrlDto
{
    public CrawledSiteDto()
    {
        SiteCrawlResults = new List<CrawledSiteUrlDto>();
        OnlySitemapResults = new List<CrawledSiteUrlDto>();
        OnlySiteResults = new List<CrawledSiteUrlDto>();
    }
    public IEnumerable<CrawledSiteUrlDto> SiteCrawlResults { get; set; }
    public IEnumerable<CrawledSiteUrlDto> OnlySitemapResults { get; set; }
    public IEnumerable<CrawledSiteUrlDto> OnlySiteResults { get; set; }
    public DateTime CrawlDate { get; set; }
}
