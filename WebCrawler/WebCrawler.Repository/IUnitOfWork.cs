using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebCrawler.Model;
using WebCrawler.Model.Entities;
using WebCrawler.Repository.BaseRepository;

namespace WebCrawler.Repository;

public interface IUnitOfWork : IDisposable
{
    IRepository<SiteCrawlResult> SiteCrawlResults { get; }

    int Complete();
}
