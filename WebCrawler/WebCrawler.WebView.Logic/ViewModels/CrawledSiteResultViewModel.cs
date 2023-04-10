using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebCrawler.Logic.Enums;

namespace WebCrawler.WebView.Logic.ViewModels;

public class CrawledSiteResultViewModel : BaseUrlViewModel
{
    public long? ResponseTimeMs { get; set; }
    public UrlFoundLocation UrlFoundLocation { get; set; }
}
