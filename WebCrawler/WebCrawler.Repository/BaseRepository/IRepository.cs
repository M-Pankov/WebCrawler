using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebCrawler.Repository.BaseRepository;

public interface IRepository<TEntity> where TEntity : class
{
    void Add(TEntity entity);
}
