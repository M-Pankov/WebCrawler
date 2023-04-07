using System.Collections;
using System.Collections.Generic;
using WebCrawler.Persistence.Entities;

namespace WebCrawler.Persistence.Repositories;

public interface ICrawledSiteRepository
{
    IEnumerable<CrawledSite> GetAll();
    void Add(CrawledSite entity);
    int SaveChanges();
}
