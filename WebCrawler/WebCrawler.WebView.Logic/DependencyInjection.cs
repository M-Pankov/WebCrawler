using Microsoft.Extensions.DependencyInjection;
using WebCrawler.WebView.Logic.Services;

namespace WebCrawler.WebView.Logic;

public static class DependencyInjection
{
    public static IServiceCollection AddWebViewServices(this IServiceCollection services)
    {
        services.AddScoped<CrawlerRepositoryService>();
        services.AddScoped<WebCrawlerService>();
        return services;
    }
}
