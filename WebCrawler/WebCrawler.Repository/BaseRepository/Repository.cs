using Microsoft.EntityFrameworkCore;
using WebCrawler.Model;

namespace WebCrawler.Repository.BaseRepository;

public class Repository<TEntity> : IRepository<TEntity> where TEntity : class
{
    protected readonly DbContext Context;
    public Repository(DbContext context)
    {
        Context = context;
    }

    public void Add(TEntity entity)
    {
        Context.Set<TEntity>().Add(entity);
    }
}