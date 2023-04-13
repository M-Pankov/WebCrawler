using Microsoft.AspNetCore.Mvc;
using System;
using System.Diagnostics;
using System.Threading.Tasks;
using WebCrawler.WebView.Logic.Services;
using WebCrawler.WebView.Logic.ViewModels;

namespace WebCrawler.WebView.Controllers;

public class CrawledSitesController : Controller
{
    private readonly WebCrawlerService _webCrawlerService;
    private readonly CrawlerRepositoryService _crawlerRepositoryService;

    public CrawledSitesController(WebCrawlerService webCrawlerService, CrawlerRepositoryService crawlerRepositoryService)
    {
        _webCrawlerService = webCrawlerService;
        _crawlerRepositoryService = crawlerRepositoryService;
    }

    public IActionResult Index(int pageNumber, int pageSize)
    {
        var crawledSites = _crawlerRepositoryService.GetCrawledSitesPagedList(pageNumber, pageSize);

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

            await _webCrawlerService.CrawlSiteAsync(input);

            return Redirect("Index");

        }
        catch(Exception ex) 
        {
            return View("CrawlError",ex.Message);
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
