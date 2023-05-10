namespace WebCrawler.Web.Logic.Validators;

public static class PageValidator
{
    public static int GetValidPageNumber(int pageNumber)
    {
        if (pageNumber > 1)
        {
            return pageNumber;
        }

        return 1;
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
