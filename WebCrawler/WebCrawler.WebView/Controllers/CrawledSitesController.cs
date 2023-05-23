using Microsoft.AspNetCore.Mvc;
using System;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using WebCrawler.Application;
using WebCrawler.Application.Helpers;
using WebCrawler.Presentation.WebView.ViewModels;

namespace WebCrawler.Presentation.WebView.Controllers;

public class CrawledSitesController : Controller
{
    private readonly CrawlerService _crawlerService;

    public CrawledSitesController(CrawlerService crawlerService)
    {
        _crawlerService = crawlerService;
    }

    public IActionResult Index(int pageNumber, int pageSize)
    {
        var crawledSites = _crawlerService.GetCrawledSitesPagedList(pageNumber, pageSize);

        return View(crawledSites);
    }

    public async Task<IActionResult> CrawlSite(string input)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(input))
            {
                throw new Exception("Empty input value.");
            }

            await _crawlerService.CrawlSiteAsync(input);

            return Redirect("Index");

        }
        catch (Exception ex)
        {
            return View("CrawlerError", ex.Message);
        }
    }

    public IActionResult Error()
    {
        return View(new ErrorViewModel
        {
            RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier
        });
    }
}
