using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebCrawler.Persistence.Entities;

namespace WebCrawler.Persistence.Repositories;

public class SiteUrlCrawlResultRepository : ISiteUrlCrawlResultRepository
{
    private readonly ApplicationDbContext _context;
    public SiteUrlCrawlResultRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public void Add(SiteUrlCrawlResult entity)
    {
        _context.Add(entity);
    }

    public void AddRange(IEnumerable<SiteUrlCrawlResult> results)
    {
       _context.AddRange(results);
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
