using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;

namespace WebCrawler.Logic.Parsers;

public class HtmlParser
{
    public virtual IEnumerable<Uri> GetLinks(Uri baseUrl, string htmlString)
    {
        HtmlDocument htmlDocument = new HtmlDocument();

        htmlDocument.LoadHtml(htmlString);

        var linksFromPage = htmlDocument.DocumentNode
                .Descendants("a")
                .Select(a => a.GetAttributeValue("href", null))
                .Where(u => !string.IsNullOrEmpty(u))
                .Distinct();

        return linksFromPage.Select(x => GetAbsoluteUrlFromString(baseUrl, x));
    }

    private Uri GetAbsoluteUrlFromString(Uri baseUrl, string link)
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
