using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace WebCrawler.Logic.Wrappers;

public class HttpClientWrapper
{
    private readonly HttpClient _httpClient;
    public HttpClientWrapper()
    {
        _httpClient = new HttpClient();
    }

    public virtual Task<string> GetStringAsync(Uri url)
    {
        return _httpClient.GetStringAsync(url);
    }
}
