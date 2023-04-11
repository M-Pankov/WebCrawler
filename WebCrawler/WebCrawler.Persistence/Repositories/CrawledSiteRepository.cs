using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
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

    public IQueryable<CrawledSite> GetAll()
    {
        return _context.CrawledSites;
    }

    public CrawledSite GetCrawledSiteById(int id)
    {
        return _context.CrawledSites.Include(x => x.CrawlResults).FirstOrDefault(x => x.Id == id);
    }

    public async Task<int> SaveChangesAsync()
    {
        if (_context != null)
        {
            return await _context.SaveChangesAsync();
        }
        return 0;
    }
}