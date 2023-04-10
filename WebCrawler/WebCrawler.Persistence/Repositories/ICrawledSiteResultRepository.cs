using System.Collections.Generic;
using System.Linq;
using WebCrawler.Persistence.Entities;

namespace WebCrawler.Persistence.Repositories;

public interface ICrawledSiteResultRepository
{
    IQueryable<CrawledSiteResult> GetAll();
    void AddRange(IEnumerable<CrawledSiteResult> results);
    void Add(CrawledSiteResult entity);
    int SaveChanges();
}
