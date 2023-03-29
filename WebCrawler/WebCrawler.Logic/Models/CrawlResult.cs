using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebCrawler.Logic.Models;

public class CrawlResult
{
    public CrawlResult()
    {
        UniqueUrls = new List<Uri>();
        Results = new List<UrlWithResponseTime>();
    }

    public IList<Uri> UniqueUrls { get; set; }
    public IList<UrlWithResponseTime> Results { get; set; }
}
