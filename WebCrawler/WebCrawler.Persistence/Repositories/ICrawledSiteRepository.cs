using WebCrawler.Persistence.Entities;

namespace WebCrawler.Persistence.Repositories;

public interface ICrawledSiteRepository
{
    void Add(CrawledSite entity);

    int SaveChanges();
}
