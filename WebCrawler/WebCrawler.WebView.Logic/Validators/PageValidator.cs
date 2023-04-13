namespace WebCrawler.WebView.Logic.Validators;

public static class PageValidator
{
    public static int GetValidPageNumber(int pageNumber)
    {
        if (pageNumber > 0)
        {
            return pageNumber;
        }

        return 0;
    }

    public static int GetValidPageSize(int pageSize)
    {
        if (pageSize >= 1)
        {
            return pageSize;
        }

        return 5;
    }
}
