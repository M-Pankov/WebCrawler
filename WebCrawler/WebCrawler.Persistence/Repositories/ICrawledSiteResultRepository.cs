using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebCrawler.Persistence.Entities;

namespace WebCrawler.Persistence.Repositories;

public interface ICrawledSiteResultRepository
{
    IEnumerable<CrawledSiteResult> GetAll();
    void AddRange(IEnumerable<CrawledSiteResult> results);
    void Add(CrawledSiteResult entity);
    int SaveChanges();
}
