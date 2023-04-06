using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebCrawler.Model;
using WebCrawler.Model.Entities;
using WebCrawler.Repository.BaseRepository;

namespace WebCrawler.Repository;

public class UnitOfWork : IUnitOfWork
{
    private readonly ApplicationDbContext _context;

    public UnitOfWork(ApplicationDbContext context)
    {
        _context = context;
        SiteCrawlResults = new Repository<SiteCrawlResult>(_context);
    }

    public IRepository<SiteCrawlResult> SiteCrawlResults { get; private set; }

    public int Complete()
    {
        if(_context != null)
        {
            return _context.SaveChanges();
        }
        return 0;
    }

    public void Dispose()
    {
        if (_context != null)
        {
            _context.Dispose();
        }
    }
}
