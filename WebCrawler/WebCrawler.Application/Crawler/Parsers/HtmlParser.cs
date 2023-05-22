using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;

namespace WebCrawler.Application.Crawler.Parsers;

public class HtmlParser
{
    public virtual IEnumerable<Uri> GetLinks(Uri baseUrl, string htmlContent)
    {
        HtmlDocument htmlDocument = new HtmlDocument();

        htmlDocument.LoadHtml(htmlContent);

        var linksFromPage = htmlDocument.DocumentNode
                .Descendants("a")
                .Select(a => a.GetAttributeValue("href", null))
                .Where(u => !string.IsNullOrEmpty(u))
                .Distinct();

        return linksFromPage.Select(x => GetAbsoluteUrl(baseUrl, x));
    }

    private Uri GetAbsoluteUrl(Uri baseUrl, string link)
    {
        var lowerLink = link.ToLower().TrimEnd('/');

        if (lowerLink.StartsWith("/"))
        {
            return new Uri(baseUrl, lowerLink);
        }

        Uri.TryCreate(lowerLink, UriKind.Absolute, out Uri uriResult);

        return uriResult;
    }
}
