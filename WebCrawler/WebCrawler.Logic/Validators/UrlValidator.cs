using System;
using System.Collections.Generic;

namespace WebCrawler.Logic.Validators;

public class UrlValidator
{
    public bool IsDisallowed(Uri input, IList<Uri> uniqueURLs, Uri parentUrl)
    {
        if (input == null || uniqueURLs.Contains(input) || input.Host != parentUrl.Host)
        {
            return true;
        }

        if (input.LocalPath.Contains('.'))
        {
            return true;
        }

        return false;
    }
}
