using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebCrawler.WebView.Logic.ViewModels;

public class CrawledSiteViewModel : BaseUrlViewModel
{
    public CrawledSiteViewModel()
    {
        SiteCrawlResult = new List<CrawledSiteResultViewModel>();
        OnlySitemapResults = new List<CrawledSiteResultViewModel>();
        OnlySiteResults = new List<CrawledSiteResultViewModel>();
    }
    public IEnumerable<CrawledSiteResultViewModel> SiteCrawlResult { get; set; }
    public IEnumerable<CrawledSiteResultViewModel> OnlySitemapResults { get; set; }
    public IEnumerable<CrawledSiteResultViewModel> OnlySiteResults { get; set; }
    public DateTime CrawlDate { get; set; }
}
