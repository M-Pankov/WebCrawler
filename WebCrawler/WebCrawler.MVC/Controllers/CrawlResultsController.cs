using Microsoft.AspNetCore.Mvc;
using WebCrawler.WebView.Logic.Services;

namespace WebCrawler.WebView.Controllers;

public class CrawlResultsController : Controller
{
    private readonly WebCrawlerService _webCrawlerService;

    public CrawlResultsController(WebCrawlerService webCrawlerService)
    {
        _webCrawlerService = webCrawlerService;
    }

    public IActionResult SiteCrawlResult(int id)
    {
        var crawledSiteResult = _webCrawlerService.GetCrawledSiteResults(id);
        return View(crawledSiteResult);
    }
}
