using System;
using System.Threading.Tasks;
using WebCrawler.ConsoleUi.Wrappers;
using WebCrawler.Logic.Crawlers;

namespace WebCrawler.ConsoleUi;

public class ConsoleWebCrawler
{
    private readonly Crawler _crawler;
    private readonly ConsoleWrapper _consoleWrapper;

    public ConsoleWebCrawler(Crawler crawler, ConsoleWrapper consoleWrapper)
    {
        _crawler = crawler;
        _consoleWrapper = consoleWrapper;
    }

    public async Task StartCrawl()
    {
        _consoleWrapper.WriteLine("Enter the site address in the format: \"https://example.com\"");

        var input = _consoleWrapper.ReadLine();

        Uri? uriResult;

        while (!Uri.TryCreate(input, UriKind.Absolute, out uriResult))
        {
            _consoleWrapper.WriteLine("Wrong format!");
            input = _consoleWrapper.ReadLine();
        }

        var result = await _crawler.FindAllPagesWithResponseTime(uriResult);

        foreach (var resultItem in result)
        {
            _consoleWrapper.WriteLine($"\r\n{resultItem.Url} : {resultItem.ResponseTime} ms : {resultItem.UrlFoundLocation}");
        }

        _consoleWrapper.ReadLine();

    }
}
