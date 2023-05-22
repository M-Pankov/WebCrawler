using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebCrawler.Domain.CrawlResults;

public class BaseUrlEntity
{
    public int Id { get; set; }
    public Uri Url { get; set; }
}
