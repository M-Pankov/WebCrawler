using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebCrawler.Logic.Enums;

namespace WebCrawler.Model.Entities;

public class UrlCrawlResult
{
    [Key]
    public int CrawledUrlId { get; set; }
    public Uri Url { get; set; }
    public long? ResponseTimeMs { get; set; }
    public UrlFoundLocation UrlFoundLocation { get; set; }
    public int SiteCrawlResultId { get; set; }
    public SiteCrawlResult SiteCrawlResult { get; set; } = null!;
}
