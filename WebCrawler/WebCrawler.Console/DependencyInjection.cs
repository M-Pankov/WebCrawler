using Microsoft.Extensions.DependencyInjection;
using WebCrawler.Console.Services;

namespace WebCrawler.Console;

public static class DependencyInjection
{
    public static IServiceCollection AddWebCrawlerConsoleServices(this IServiceCollection services)
    {
        services.AddScoped<ConsoleService>();
        services.AddScoped<CrawlerRepositoryService>();
        services.AddScoped<ConsoleWebCrawler>();

        return services;
    }
}
