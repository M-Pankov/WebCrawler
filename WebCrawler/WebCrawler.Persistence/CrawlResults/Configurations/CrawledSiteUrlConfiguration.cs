using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WebCrawler.Domain.CrawlResults;

namespace WebCrawler.Persistence.CrawlResults.Configurations;

internal class CrawledSiteUrlConfiguration : IEntityTypeConfiguration<CrawledSiteUrl>
{
    public void Configure(EntityTypeBuilder<CrawledSiteUrl> builder)
    {
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Url).IsRequired();
        builder.Property(x => x.UrlFoundLocation).IsRequired();

        builder.HasOne(x => x.CrawledSite)
            .WithMany(e => e.CrawlResults)
            .HasForeignKey(x => x.CrawledSiteId)
            .OnDelete(DeleteBehavior.Cascade)
            .IsRequired();
    }
}
