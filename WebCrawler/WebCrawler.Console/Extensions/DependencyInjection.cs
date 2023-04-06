using Microsoft.Extensions.DependencyInjection;
using WebCrawler.Console.Services;

namespace WebCrawler.Console.Extensions;

public static class DependencyInjection
{
    public static void AddWebCrawlerConsoleServices(this IServiceCollection services)
    {
        services.AddScoped<ConsoleService>();
        services.AddScoped<CrawlerRepositoryService>();
        services.AddScoped<ConsoleWebCrawler>();
    }
}
