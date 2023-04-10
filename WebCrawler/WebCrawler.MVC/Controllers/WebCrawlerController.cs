using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Threading.Tasks;
using WebCrawler.WebView.Logic.Services;
using WebCrawler.WebView.Logic.ViewModels;

namespace WebCrawler.WebView.Controllers
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

            if (!page.HasValue)
            {

            }

            int pageSize = 5;

            var crawledSites = _webCrawlerControllerService.GetCrawledSitesPagedList((int)page, pageSize);


            return View(crawledSites);
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