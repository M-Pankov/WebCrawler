using Microsoft.Extensions.DependencyInjection;
using WebCrawler.Web.Logic.Services;

namespace WebCrawler.Web.Logic;

public static class DependencyInjection
{
    public static IServiceCollection AddWebServices(this IServiceCollection services)
    {
        services.AddScoped<CrawlerRepositoryService>();
        services.AddScoped<WebCrawlerService>();

        return services;
    }
}
