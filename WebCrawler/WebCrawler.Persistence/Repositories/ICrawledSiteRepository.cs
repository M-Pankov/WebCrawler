using System.Linq;
using WebCrawler.Persistence.Entities;

namespace WebCrawler.Persistence.Repositories;

public interface ICrawledSiteRepository
{
    IQueryable<CrawledSite> GetAll();
    void Add(CrawledSite entity);
    int SaveChanges();
}
