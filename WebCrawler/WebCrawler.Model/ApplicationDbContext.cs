using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using WebCrawler.Model.Entities;

namespace WebCrawler.Model;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions options) : base(options)
    {
    }

    public DbSet<SiteCrawlResult> SiteCrawlResults { get; set; }
    public DbSet<UrlCrawlResult> UrlCrawlResults { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<SiteCrawlResult>()
            .Property(p => p.CrawlDate).ValueGeneratedOnAdd()
            .HasDefaultValueSql("datetime('now')");
    }
}