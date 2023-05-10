using System;
using System.Collections.Generic;
using System.Linq;
using WebCrawler.Web.Logic.Validators;

namespace WebCrawler.Web.Logic.Helpers;

public class PagedList<T> : List<T>
{
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 5;
    public int TotalCount { get; set; }
    public int TotalPages { get; set; }

    public PagedList()
    {
    }

    public PagedList(IQueryable<T> source, int pageNumber, int pageSize)
    {
        PageNumber = PageValidator.GetValidPageNumber(pageNumber);
        PageSize = PageValidator.GetValidPageSize(pageSize);
        TotalCount = source.Count();
        TotalPages = (int)Math.Ceiling(TotalCount / (double)PageSize);

        this.AddRange(source.Skip((PageNumber - 1) * PageSize).Take(PageSize));
    }

    public bool HasPreviousPage => (PageNumber > 1);

    public bool HasNextPage => (PageNumber + 1 < TotalPages);
}
