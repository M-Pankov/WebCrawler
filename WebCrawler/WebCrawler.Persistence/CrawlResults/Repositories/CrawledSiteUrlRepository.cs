using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebCrawler.Application.Interfaces;
using WebCrawler.Domain.CrawlResults;

namespace WebCrawler.Persistence.CrawlResults.Repositories;

public class CrawledSiteUrlRepository : ICrawledSiteUrlRepository
{
    private readonly ApplicationDbContext _context;
    public CrawledSiteUrlRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task AddAsync(CrawledSiteUrl entity)
    {
        await _context.AddAsync(entity);
    }

    public async Task AddRangeAsync(IEnumerable<CrawledSiteUrl> results)
    {
        await _context.AddRangeAsync(results);
    }

    public IQueryable<CrawledSiteUrl> GetAll()
    {
        return _context.CrawledSiteUrls;
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
