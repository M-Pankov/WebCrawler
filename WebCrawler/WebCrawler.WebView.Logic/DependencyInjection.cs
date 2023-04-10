using Microsoft.Extensions.DependencyInjection;
using WebCrawler.WebView.Logic.Services;

namespace WebCrawler.WebView.Logic;

public static class DependencyInjection
{
    public static IServiceCollection AddWebCrawlerWebViewServices(this IServiceCollection services)
    {
        services.AddScoped<CrawlerRepositoryService>();
        services.AddScoped<WebCrawlerControllerService>();
        return services;
    }
}
