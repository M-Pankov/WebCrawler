using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebCrawler.Persistence.Entities;

namespace WebCrawler.Persistence.Repositories;

public interface ICrawledSiteResultRepository
{
    IQueryable<CrawledSiteResult> GetAll();
    Task AddRangeAsync(IEnumerable<CrawledSiteResult> results);
    Task AddAsync(CrawledSiteResult entity);
    Task<int> SaveChangesAsync();
}
