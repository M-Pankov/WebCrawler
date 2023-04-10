using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Diagnostics;
using System.Threading.Tasks;
using WebCrawler.Logic.Crawlers;
using WebCrawler.MVC.Services;
using WebCrawler.MVC.ViewModels;
using WebCrawler.Persistence.Entities;
using X.PagedList;

namespace WebCrawler.MVC.Controllers
{
    public class WebCrawlerController : Controller
    {
        private readonly WebCrawlerControllerService _webCrawlerControllerService;
        public WebCrawlerController(WebCrawlerControllerService webCrawlerControllerService)
        {
            _webCrawlerControllerService = webCrawlerControllerService;
        }

        public async Task<IActionResult> Index(string? input, int? page)
        {
            if (!string.IsNullOrWhiteSpace(input))
            {
                await _webCrawlerControllerService.CrawlSite(input);
            }

            var crawledSites = _webCrawlerControllerService.GetCrawledSites();
            int pageSize = 5;
            int pageNumber = (page ?? 1);
            return View(crawledSites.ToPagedList(pageNumber,pageSize));
        }

        public IActionResult TestResultPage(int id)
        {
            var crawledSiteVm = _webCrawlerControllerService.GetCrawledSiteResults(id);

            return View(crawledSiteVm);
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}