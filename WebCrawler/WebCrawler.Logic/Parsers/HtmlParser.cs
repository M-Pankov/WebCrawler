using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;

namespace WebCrawler.Logic.Parsers;

public class HtmlParser
{
    public IEnumerable<Uri> GetLinksFromHtmlString(Uri parentUrl, string htmlString)
    {
        HtmlDocument htmlDocument = new HtmlDocument();

        htmlDocument.LoadHtml(htmlString);

        var linksFromPage = htmlDocument.DocumentNode
                .Descendants("a")
                .Select(a => a.GetAttributeValue("href", null))
                .Where(u => !string.IsNullOrEmpty(u))
                .Distinct();

        return linksFromPage.Select(x => GetAbsoluteUrlFromString(parentUrl, x));
    }

    private Uri GetAbsoluteUrlFromString(Uri parentUrl, string link)
    {
        var lowerLink = link.ToLower();

        if (lowerLink.StartsWith("/"))
        {
            return new Uri(parentUrl, lowerLink);
        }

        Uri.TryCreate(lowerLink, UriKind.Absolute, out Uri uriResult);
        return uriResult;
    }
}
