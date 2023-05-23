using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using WebCrawler.Application;

namespace WebCrawler.Presentation.WebView.Controllers;

public class CrawlResultsController : Controller
{
    private readonly CrawlerService _crawlerService;

    public CrawlResultsController(CrawlerService crawlerService)
    {
        _crawlerService = crawlerService;
    }

    public async Task<IActionResult> Index(int id)
    {
        var crawledSiteResult = await _crawlerService.GetCrawledSiteResultsAsync(id);
        return View(crawledSiteResult);
    }
}
