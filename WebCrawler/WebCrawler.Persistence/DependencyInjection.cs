using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using WebCrawler.Persistence.Repositories;

namespace WebCrawler.Persistence.Extensions;

public static class DependencyInjection
{
    public static IServiceCollection AddDbServices(this IServiceCollection service, IConfiguration configuration)
    {
        service.AddDbContext<ApplicationDbContext>(options =>
        options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));
        service.AddScoped<ICrawledSiteRepository, CrawledSiteRepository>();
        service.AddScoped<ISiteUrlCrawlResultRepository, SiteUrlCrawlResultRepository>();

        return service;
    }
}
