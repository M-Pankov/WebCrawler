using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using WebCrawler.Application.Interfaces;
using WebCrawler.Domain.CrawlResults;

namespace WebCrawler.Persistence.Repositories;

public class CrawledSiteRepository : ICrawledSiteRepository
{
    protected readonly ApplicationDbContext _context;
    public CrawledSiteRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task AddAsync(CrawledSite entity)
    {
        await _context.CrawledSites.AddAsync(entity);
    }

    public IQueryable<CrawledSite> GetAll()
    {
        return _context.CrawledSites;
    }

    public async Task<CrawledSite> GetCrawledSiteByIdAsync(int id)
    {
        return await _context.CrawledSites.Include(x => x.CrawlResults).FirstOrDefaultAsync(x => x.Id == id);
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