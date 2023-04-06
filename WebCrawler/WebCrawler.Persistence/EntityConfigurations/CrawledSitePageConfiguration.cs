using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WebCrawler.Persistence.Entities;

namespace WebCrawler.Persistence.EntityConfigurations
{
    internal class CrawledSitePageConfiguration : IEntityTypeConfiguration<CrawledSitePage>
    {
        public void Configure(EntityTypeBuilder<CrawledSitePage> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Url).IsRequired();
            builder.Property(x => x.UrlFoundLocation).IsRequired();

            builder.HasOne(x => x.CrawledSite)
                .WithMany(e => e.CrawledPages)
                .HasForeignKey(x => x.CrawledSiteId)
                .OnDelete(DeleteBehavior.Cascade)
                .IsRequired();
        }
    }
}
