using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using WebCrawler.Application;
using WebCrawler.Application.Interfaces;
using WebCrawler.Crawlers;
using WebCrawler.Crawlers.Loaders;
using WebCrawler.Crawlers.Parsers;
using WebCrawler.Crawlers.SubCrawlers;
using WebCrawler.Crawlers.Validators;
using WebCrawler.Persistence;
using WebCrawler.Persistence.Repositories;

namespace WebCrawler.InfrastructureIoC;

public static class DependencyContainer
{
    public static IServiceCollection RegisterCrawlerServices(this IServiceCollection services)
    {
        services.AddScoped<ICrawledSiteRepository, CrawledSiteRepository>();
        services.AddScoped<ICrawledSiteUrlRepository, CrawledSiteUrlRepository>();
        services.AddScoped<CrawlerService>();

        return services;
    }

    public static IServiceCollection RegisterCrawlers(this IServiceCollection services)
    {
        services.AddScoped<UrlValidator>();
        services.AddHttpClient<HtmlLoader>();
        services.AddScoped<HtmlLoader>();
        services.AddScoped<SiteMapLoader>();
        services.AddScoped<HtmlParser>();
        services.AddScoped<SiteMapCrawler>();
        services.AddScoped<SiteCrawler>();
        services.AddScoped<ICrawler, Crawler>();

        return services;
    }

    public static IServiceCollection RegisterDbContext(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<ApplicationDbContext>(options =>
        options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

        return services;
    }
}
