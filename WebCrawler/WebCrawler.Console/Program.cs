using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Threading.Tasks;
using WebCrawler.InfrastructureIoC;
using WebCrawler.Presentation.Console;
using WebCrawler.Presentation.Console.Services;

namespace WebCrawler.Presentation.Console;
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
        .ConfigureAppConfiguration(app =>
        {
            app.AddJsonFile("appsettings.json");
        })
        .ConfigureServices((builderContext, services) =>
        {
            services.RegisterCrawlers();
            services.RegisterDbContext(builderContext.Configuration);
            services.RegisterCrawlerServices();
            services.AddScoped<ConsoleService>();
            services.AddScoped<ConsoleWebCrawler>();
        });
}