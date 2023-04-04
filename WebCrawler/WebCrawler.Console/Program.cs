using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
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
            .ConfigureServices((hostContext, services) =>
            {
                services.AddHttpClient();
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