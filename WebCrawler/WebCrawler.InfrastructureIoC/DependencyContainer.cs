using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using WebCrawler.Application.Crawler;
using WebCrawler.Application.Crawler.CrawlServices;
using WebCrawler.Application.Crawler.Interfaces;
using WebCrawler.Application.Crawler.Loaders;
using WebCrawler.Application.Crawler.Parsers;
using WebCrawler.Application.Crawler.Validators;
using WebCrawler.Persistence;
using WebCrawler.Persistence.CrawlResults.Repositories;

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
        services.AddScoped<SiteMapCrawlService>();
        services.AddScoped<SiteCrawlService>();
        services.AddScoped<CrawlService>();

        return services;
    }

    public static IServiceCollection RegisterDbContext(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<ApplicationDbContext>(options =>
        options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

        return services;
    }
}
