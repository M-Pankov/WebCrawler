using System;
using WebCrawler.Logic.Enums;

namespace WebCrawler.Logic.Models;

public class CrawledUrl
{
    public Uri Url { get; set; }
    public long? ResponseTimeMs { get; set; }
    public UrlFoundLocation UrlFoundLocation { get; set; }
}
