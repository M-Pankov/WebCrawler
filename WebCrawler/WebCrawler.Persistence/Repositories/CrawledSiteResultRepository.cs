using System.Collections.Generic;
using System.Linq;
using WebCrawler.Persistence.Entities;

namespace WebCrawler.Persistence.Repositories;

public class CrawledSiteResultRepository : ICrawledSiteResultRepository
{
    private readonly ApplicationDbContext _context;
    public CrawledSiteResultRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public void Add(CrawledSiteResult entity)
    {
        _context.Add(entity);
    }

    public void AddRange(IEnumerable<CrawledSiteResult> results)
    {
        _context.AddRange(results);
    }

    public IQueryable<CrawledSiteResult> GetAll()
    {
        return _context.CrawledSiteResults;
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
