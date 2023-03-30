using System;
using WebCrawler.Logic.Enums;

namespace WebCrawler.Logic.Models;

public class UrlWithResponseTime
{
    public Uri Url { get; set; }
    public long? ResponseTime { get; set; }
    public UrlFoundFrom FoundFrom { get; set; }
}
