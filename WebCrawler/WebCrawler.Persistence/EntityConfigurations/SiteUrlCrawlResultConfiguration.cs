using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WebCrawler.Persistence.Entities;

namespace WebCrawler.Persistence.EntityConfigurations
{
    internal class SiteUrlCrawlResultConfiguration : IEntityTypeConfiguration<CrawledSiteResult>
    {
        public void Configure(EntityTypeBuilder<CrawledSiteResult> builder)
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
}
