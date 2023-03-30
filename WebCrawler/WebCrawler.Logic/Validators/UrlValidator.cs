using System;

namespace WebCrawler.Logic.Validators;

public class UrlValidator
{
    public virtual bool IsDisallowed(Uri input, Uri baseUrl)
    {
        if (input == null || input.Host != baseUrl.Host)
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
