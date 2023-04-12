using System;

namespace WebCrawler.Logic.Validators;

public class UrlValidator
{
    public virtual bool IsAllowed(Uri input, Uri baseUrl)
    {
        if (input == null || input.Host != baseUrl.Host)
        {
            return false;
        }

        if (input.Scheme != baseUrl.Scheme)
        {
            return false;
        }

        if (input.LocalPath.Contains('.'))
        {
            return false;
        }

        return true;
    }
}
