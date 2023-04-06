using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebCrawler.Logic.Models;

namespace WebCrawler.Model.Entities;

public class SiteCrawlResult
{
    [Key]
    public int SiteCrawlResultId { get; set; }
    public Uri Url { get; set; } 
    public DateTime CrawlDate { get; set; }
    public ICollection<UrlCrawlResult> CrawledUrls { get; set; } = new List<UrlCrawlResult>();
}
