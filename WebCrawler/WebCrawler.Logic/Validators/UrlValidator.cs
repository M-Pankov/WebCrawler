using System;
using System.Collections.Generic;

namespace WebCrawler.Logic.Validators;

public class UrlValidator
{
    public bool IsDisallowed(Uri input, Uri parentUrl)
    {
        if (input == null || input.Host != parentUrl.Host)
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
