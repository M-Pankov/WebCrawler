using Microsoft.Extensions.DependencyInjection;
using WebCrawler.MVC.Services.ControllerServices;
using WebCrawler.MVC.Services.RepositotyServices;

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
