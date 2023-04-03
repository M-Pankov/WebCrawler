using System.Net.Http;
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
        var httpClient = new HttpClient();
        var htmlParser = new HtmlParser();
        var urlValidator = new UrlValidator();
        var htmlLoader = new HtmlLoaderService(httpClient);
        var sitemapLoader = new SitemapLoaderService();
        var sitemapCrawler = new SitemapCrawler(sitemapLoader);
        var siteCrawler = new SiteCrawler(htmlParser, urlValidator, htmlLoader);
        var consoleService = new ConsoleService();
        var crawler = new Crawler(siteCrawler, sitemapCrawler, htmlLoader);
        var consoleCrawler = new ConsoleWebCrawler(crawler, consoleService);

        await consoleCrawler.StartCrawlAsync();
    }
}