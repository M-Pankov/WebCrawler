using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using WebCrawler.WebView.Logic.Services;

namespace WebCrawler.WebView.Controllers;

public class CrawlResultsController : Controller
{
    private readonly WebCrawlerService _webCrawlerService;

    public CrawlResultsController(WebCrawlerService webCrawlerService)
    {
        _webCrawlerService = webCrawlerService;
    }

    public async Task<IActionResult> SiteCrawlResult(int id)
    {
        var crawledSiteResult = await _webCrawlerService.GetCrawledSiteResultsAsync(id);
        return View(crawledSiteResult);
    }
}
