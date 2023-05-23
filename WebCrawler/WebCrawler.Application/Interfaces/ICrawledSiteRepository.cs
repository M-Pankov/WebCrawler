using System.Linq;
using System.Threading.Tasks;
using WebCrawler.Domain.CrawlResults;

namespace WebCrawler.Application.Interfaces;

public interface ICrawledSiteRepository
{
    IQueryable<CrawledSite> GetAll();
    Task<CrawledSite> GetCrawledSiteByIdAsync(int id);
    Task AddAsync(CrawledSite entity);
    Task<int> SaveChangesAsync();
}
