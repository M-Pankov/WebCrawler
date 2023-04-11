using System.Linq;
using System.Threading.Tasks;
using WebCrawler.Persistence.Entities;

namespace WebCrawler.Persistence.Repositories;

public interface ICrawledSiteRepository
{
    IQueryable<CrawledSite> GetAll();
    CrawledSite GetCrawledSiteById(int id);
    void Add(CrawledSite entity);
    Task<int> SaveChangesAsync();
}
