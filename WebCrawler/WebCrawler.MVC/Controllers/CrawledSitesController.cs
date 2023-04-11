using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Threading.Tasks;
using WebCrawler.WebView.Logic.Services;
using WebCrawler.WebView.Logic.ViewModels;

namespace WebCrawler.WebView.Controllers;

public class CrawledSitesController : Controller
{
    private readonly WebCrawlerService _webCrawlerService;
    public CrawledSitesController(WebCrawlerService webCrawlerService)
    {
        _webCrawlerService = webCrawlerService;
    }

    public IActionResult Index(int? pageNumber, int? pageSize)
    {
        var crawledSites = _webCrawlerService.GetCrawledSitesPagedList(pageNumber, pageSize);

        return View(crawledSites);
    }

    public async Task<IActionResult> CrawlSite(string? input)
    {
        if (!string.IsNullOrWhiteSpace(input))
        {
            await _webCrawlerService.CrawlSite(input);
        }

        return Redirect("Index");
    }

    public IActionResult Error()
    {
        return View(new ErrorViewModel
        {
            RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier
        });
    }
}
