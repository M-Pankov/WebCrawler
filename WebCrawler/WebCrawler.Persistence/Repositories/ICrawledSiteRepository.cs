using System.Linq;
using System.Threading.Tasks;
using WebCrawler.Persistence.Entities;

namespace WebCrawler.Persistence.Repositories;

public interface ICrawledSiteRepository
{
    IQueryable<CrawledSite> GetAll();
    Task<CrawledSite> GetCrawledSiteByIdAsync(int id);
    Task AddAsync(CrawledSite entity);
    Task<int> SaveChangesAsync();
}
