using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Http.Logging;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using WebCrawler.Console.Services;
using WebCrawler.Logic.Crawlers;
using WebCrawler.Logic.Parsers;
using WebCrawler.Logic.Services;
using WebCrawler.Logic.Validators;

namespace WebCrawler.Console;
public class Program
{
    public static async Task Main(string[] args)
    {
        var host = CreateHostBuilder(args).Build();

        var consoleCrawler = host.Services.GetRequiredService<ConsoleWebCrawler>();

        await consoleCrawler.StartCrawlAsync();
    }

    public static IHostBuilder CreateHostBuilder(string[] args) =>
        Host.CreateDefaultBuilder(args)
            .ConfigureServices( services =>
            {
                services.AddLogging(config => config.SetMinimumLevel(LogLevel.Warning));
                services.AddHttpClient<HtmlLoaderService>();
                services.AddScoped<HtmlParser>();
                services.AddScoped<HtmlLoaderService>();
                services.AddScoped<ConsoleService>();
                services.AddScoped<UrlValidator>();
                services.AddScoped<SitemapLoaderService>();
                services.AddScoped<SitemapCrawler>();
                services.AddScoped<SiteCrawler>();
                services.AddScoped<Crawler>();
                services.AddScoped<ConsoleWebCrawler>();
            });
}