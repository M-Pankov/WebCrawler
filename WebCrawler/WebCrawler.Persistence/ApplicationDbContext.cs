using Microsoft.EntityFrameworkCore;
using System.Reflection;
using WebCrawler.Persistence.Entities;

namespace WebCrawler.Persistence;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions options) : base(options)
    {
    }

    public DbSet<CrawledSite> CrawledSites { get; set; }
    public DbSet<CrawledSiteResult> CrawledSiteResults { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }
}