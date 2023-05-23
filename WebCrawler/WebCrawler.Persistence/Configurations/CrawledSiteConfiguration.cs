using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WebCrawler.Domain.CrawlResults;

namespace WebCrawler.Persistence.Configurations;

public class CrawledSiteConfiguration : IEntityTypeConfiguration<CrawledSite>
{
    public void Configure(EntityTypeBuilder<CrawledSite> builder)
    {
        builder.HasKey(x => x.Id);

        builder.Property(x => x.CrawlDate).IsRequired();

        builder.Property(x => x.Url).IsRequired();

        builder.HasMany(x => x.CrawlResults)
            .WithOne(x => x.CrawledSite)
            .HasForeignKey(x => x.CrawledSiteId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
