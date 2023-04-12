using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebCrawler.Persistence.Entities;

namespace WebCrawler.Persistence.Repositories;

public class CrawledSiteResultRepository : ICrawledSiteResultRepository
{
    private readonly ApplicationDbContext _context;
    public CrawledSiteResultRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task AddAsync(CrawledSiteResult entity)
    {
        await _context.AddAsync(entity);
    }

    public async Task AddRangeAsync(IEnumerable<CrawledSiteResult> results)
    {
        await _context.AddRangeAsync(results);
    }

    public IQueryable<CrawledSiteResult> GetAll()
    {
        return _context.CrawledSiteResults;
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
