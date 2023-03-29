using System;

namespace WebCrawler.Logic.Models;

public class PageWithTiming
{
    public Uri PageUrl { get; set; }
    public long Timing { get; set; }
}
