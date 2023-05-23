using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebCrawler.Domain.CrawlResults;

namespace WebCrawler.Application.Interfaces;

public interface ICrawler
{
    Task<IEnumerable<CrawledSiteUrl>> CrawlUrlsAsync(Uri input);
}
