using System;

namespace WebCrawler.Logic.Models;

public class UrlWithResponseTime
{
    public Uri? Url { get; set; }
    public long? ResponseTime { get; set; }
}
