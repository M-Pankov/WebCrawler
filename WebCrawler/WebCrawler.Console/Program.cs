using System;
using WebCrawler.ConsoleUi.Wrappers;
using WebCrawler.Logic.Crawlers;
using WebCrawler.Logic.Loaders;
using WebCrawler.Logic.Parsers;
using WebCrawler.Logic.Validators;
using WebCrawler.Logic.Wrappers;

namespace WebCrawler.ConsoleUi;
public class Program
{
    public static void Main(string[] args)
    {
        var httpClient = new HttpClientWrapper();
        var htmlParser = new HtmlParser();
        var urlValidator = new UrlValidator();
        var htmlLoader = new HtmlLoader(httpClient);
        var sitemapLoader = new SitemapLoaderWrapper();
        var sitemapCrawler = new SitemapCrawler(sitemapLoader); 
        var siteCrawler = new SiteCrawler(htmlParser, urlValidator, htmlLoader);
        var consoleWrapper = new ConsoleWrapper();
        var crawler = new Crawler(siteCrawler,sitemapCrawler,htmlLoader);
        var consoleCrawler = new ConsoleWebCrawler(crawler, consoleWrapper);

         consoleCrawler.StartCrawl().Wait();
    }
}