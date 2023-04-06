using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WebCrawler.Persistence.Entities;

namespace WebCrawler.Persistence.EntityConfigurations;

public class CrawledSiteConfiguration : IEntityTypeConfiguration<CrawledSite>
{
    public void Configure(EntityTypeBuilder<CrawledSite> builder)
    {
        builder.HasKey(x => x.Id);

        builder.Property(x => x.CrawlDate).IsRequired();

        builder.Property(x => x.Url).IsRequired();

        builder.HasMany(x => x.CrawledPages)
            .WithOne(x => x.CrawledSite)
            .HasForeignKey(x => x.CrawledSiteId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
