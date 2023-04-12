using System;
using System.Diagnostics;
using System.Net.Http;
using System.Threading.Tasks;
using WebCrawler.Logic.Models;

namespace WebCrawler.Logic.Services;

public class HtmlLoaderService
{
    private readonly HttpClient _httpClient;

    public HtmlLoaderService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public virtual async Task<HttpResponse> GetHttpResponseAsync(Uri input)
    {
        var stopwatch = new Stopwatch();

        try
        {
            stopwatch.Restart();

            var htmlString = await _httpClient.GetStringAsync(input);

            stopwatch.Stop();

            return new HttpResponse()
            {
                HtmlContent = htmlString,
                ResponseTimeMs = stopwatch.ElapsedMilliseconds
            };

        }catch(Exception ex)
        {
            stopwatch.Stop();

            return new HttpResponse()
            {
                HtmlContent = "",
                ResponseTimeMs = stopwatch.ElapsedMilliseconds
            };
        }
    }
}
