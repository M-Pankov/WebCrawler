using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using WebCrawler.Web.Logic.Services;
using WebCrawler.Web.Logic.ViewModels;
using WebCrawler.WebApi.Models;

namespace WebCrawler.WebApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class WebCrawlerController : ControllerBase
{
    private readonly WebCrawlerService _webCrawlerService;
    private readonly CrawlerRepositoryService _crawlerRepositoryService;

    public WebCrawlerController(WebCrawlerService webCrawlerService, CrawlerRepositoryService crawlerRepositoryService)
    {
        _webCrawlerService = webCrawlerService;
        _crawlerRepositoryService = crawlerRepositoryService;
    }

    /// <summary>
    /// Get a paginated list of crawled sites.
    /// </summary>
    /// <param name="pageNumber">The page number to retrieve (default: 1).</param>
    /// <param name="pageSize">The number of sites per page (default: 5).</param>
    /// <returns>A paginated list of crawled sites.</returns>
    [HttpGet("crawled-sites")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(CrawledSitesResponseModel))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public IActionResult CrawledSites(int pageNumber, int pageSize)
    {
        var crawledSites = _crawlerRepositoryService.GetCrawledSitesPagedList(pageNumber, pageSize);
        var response = new CrawledSitesResponseModel(crawledSites);
        return Ok(response);
    }

    /// <summary>
    /// Get a crawled site results
    /// </summary>
    /// <param name="id">The crawled site id.</param>
    /// <returns> List of Urls with timings.</returns>
    [HttpGet("crawled-sites/{id}/results")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(CrawledSiteViewModel))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> CrawledSiteResults(int id)
    {
        var crawledSiteResult = await _webCrawlerService.GetCrawledSiteResultsAsync(id);

        return Ok(crawledSiteResult);
    }

    /// <summary>
    /// Crawl website.
    /// </summary>
    [HttpPost("crawl-site")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> CrawlSite(string? uriString)
    {
        await _webCrawlerService.CrawlSiteAsync(uriString);

        return Ok();
    }
}
