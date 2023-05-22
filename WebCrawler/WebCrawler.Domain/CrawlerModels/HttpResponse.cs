namespace WebCrawler.Domain.CrawlerModels;

public class HttpResponse
{
    public string HtmlContent { get; set; }
    public long ResponseTimeMs { get; set; }
}
