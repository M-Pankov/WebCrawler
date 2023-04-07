using WebCrawler.Logic.Enums;

namespace WebCrawler.MVC.ViewModels
{
    public class CrawledSiteResultVm : BaseUrlVm
    {
        public long? ResponseTimeMs { get; set; }
        public UrlFoundLocation UrlFoundLocation { get; set; }
    }
}
