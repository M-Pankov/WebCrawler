using WebCrawler.Logic.Enums;

namespace WebCrawler.Web.Logic.ViewModels;

public class CrawledSiteResultViewModel : BaseUrlViewModel
{
    public long? ResponseTimeMs { get; set; }
    public UrlFoundLocation UrlFoundLocation { get; set; }
}
