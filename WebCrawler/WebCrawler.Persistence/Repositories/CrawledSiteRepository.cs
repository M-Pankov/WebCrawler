using WebCrawler.Persistence.Entities;

namespace WebCrawler.Persistence.Repositories;

public class CrawledSiteRepository : ICrawledSiteRepository
{
    protected readonly ApplicationDbContext _context;
    public CrawledSiteRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public void Add(CrawledSite entity)
    {
        _context.CrawledSites.Add(entity);
    }

    public int SaveChanges()
    {
        if (_context != null)
        {
            return _context.SaveChanges();
        }
        return 0;
    }
}