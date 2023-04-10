using WebCrawler.Logic.Enums;

namespace WebCrawler.WebView.Logic.ViewModels;

public class CrawledSiteResultViewModel : BaseUrlViewModel
{
    public long? ResponseTimeMs { get; set; }
    public UrlFoundLocation UrlFoundLocation { get; set; }
}
