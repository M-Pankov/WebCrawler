using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using WebCrawler.Application;
using WebCrawler.Application.Helpers;
using WebCrawler.Application.Models;

namespace WebCrawler.Presentation.WebApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class WebCrawlerController : ControllerBase
{
    private readonly CrawlerService _crawlerService;

    public WebCrawlerController(CrawlerService crawlerService)
    {
        _crawlerService = crawlerService;
    }

    /// <summary>
    /// Get a paginated list of crawled sites.
    /// </summary>
    /// <param name="pageNumber">The page number to retrieve (default: 1).</param>
    /// <param name="pageSize">The number of sites per page (default: 5).</param>
    /// <returns>A paginated list of crawled sites.</returns>
    [HttpGet("crawled-sites")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(PagedList<CrawledSiteDto>))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public IActionResult CrawledSites(int pageNumber, int pageSize)
    {
        var crawledSites = _crawlerService.GetCrawledSitesPagedList(pageNumber, pageSize);
        return Ok(crawledSites);
    }

    /// <summary>
    /// Get a crawled site results
    /// </summary>
    /// <param name="id">The crawled site id.</param>
    /// <returns> List of Urls with timings.</returns>
    [HttpGet("crawled-sites/{id}/results")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(CrawledSiteDto))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CrawledSiteResults(int id)
    {
        var crawledSiteResult = await _crawlerService.GetCrawledSiteResultsAsync(id);

        return Ok(crawledSiteResult);
    }

    /// <summary>
    /// Crawl website.
    /// </summary>
    [HttpPost("crawl-site")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CrawlSite(string? uriString)
    {
        await _crawlerService.CrawlSiteAsync(uriString);

        return Ok();
    }
}
