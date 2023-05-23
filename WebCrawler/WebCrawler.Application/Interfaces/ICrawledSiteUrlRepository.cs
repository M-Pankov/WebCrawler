using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebCrawler.Domain.CrawlResults;

namespace WebCrawler.Application.Interfaces;

public interface ICrawledSiteUrlRepository
{
    IQueryable<CrawledSiteUrl> GetAll();
    Task AddRangeAsync(IEnumerable<CrawledSiteUrl> results);
    Task AddAsync(CrawledSiteUrl entity);
    Task<int> SaveChangesAsync();
}
