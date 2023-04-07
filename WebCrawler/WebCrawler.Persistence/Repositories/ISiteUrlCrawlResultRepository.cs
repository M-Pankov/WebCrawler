using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebCrawler.Persistence.Entities;

namespace WebCrawler.Persistence.Repositories;

public interface ISiteUrlCrawlResultRepository
{
    void AddRange(IEnumerable<SiteUrlCrawlResult> results);
    void Add(SiteUrlCrawlResult entity);
    int SaveChanges();
}
