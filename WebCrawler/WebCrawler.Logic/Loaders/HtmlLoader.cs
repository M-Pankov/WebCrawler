using System;
using System.Diagnostics;
using System.Threading.Tasks;
using WebCrawler.Logic.Models;
using WebCrawler.Logic.Wrappers;

namespace WebCrawler.Logic.Loaders;

public class HtmlLoader
{
    private readonly HttpClientWrapper _httpClientWrapper;

    public HtmlLoader(HttpClientWrapper httpClientWrapper)
    {
        _httpClientWrapper = httpClientWrapper;
    }

    public virtual async Task<HtmlContentWithResponseTime> GetHtmlContentWithResponseTimeAsync(Uri input)
    {
        var stopwatch = new Stopwatch();

        stopwatch.Restart();

        var htmlString = await _httpClientWrapper.GetStringAsync(input);

        stopwatch.Stop();

        return new HtmlContentWithResponseTime()
        {
            HtmlContent = htmlString,
            ResponseTime = stopwatch.ElapsedMilliseconds
        };

    }
}
