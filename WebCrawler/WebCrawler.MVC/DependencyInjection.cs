using Microsoft.Extensions.DependencyInjection;
using WebCrawler.MVC.Services;

namespace WebCrawler.MVC;

public static class DependencyInjection
{
    public static IServiceCollection AddWebCrawlerMvcServices(this IServiceCollection services)
    {
        services.AddScoped<CrawlerRepositoryService>();
        services.AddScoped<WebCrawlerControllerService>();
        return services;
    }
}
