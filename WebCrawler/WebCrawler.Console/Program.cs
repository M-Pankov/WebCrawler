using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using WebCrawler.Console.Services;
using WebCrawler.Logic.Crawlers;
using WebCrawler.Logic.Parsers;
using WebCrawler.Logic.Services;
using WebCrawler.Logic.Validators;
using WebCrawler.Model;
using WebCrawler.Repository;

namespace WebCrawler.Console;
public class Program
{
    public static async Task Main(string[] args)
    {
        var host = CreateHostBuilder(args).Build();

        OnSturtUp(host);

        var consoleCrawler = host.Services.GetRequiredService<ConsoleWebCrawler>();

        await consoleCrawler.StartCrawlAsync();
    }

    public static IHostBuilder CreateHostBuilder(string[] args) =>
        Host.CreateDefaultBuilder(args)
            .ConfigureServices(services =>
            {
                services.AddDbContext<ApplicationDbContext>(options => options.UseSqlite("Data Source=WebCrawlerConsole.db"));
                services.AddLogging(config => config.SetMinimumLevel(LogLevel.Warning));
                services.AddScoped<IUnitOfWork, UnitOfWork>();
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

    public static void OnSturtUp(IHost host)
    {
        var context = host.Services.GetRequiredService<ApplicationDbContext>();
        context.Database.Migrate();
    }
}