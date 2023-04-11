using System;
using System.Collections.Generic;
using System.Linq;

namespace WebCrawler.WebView.Logic.Helpers;

public class PagedList<T> : List<T>
{
    private int _pageNumber = 0;
    public int PageNumber
    {
        get
        {
            return _pageNumber;
        }

        set
        {
            if (value > 0)
            {
                _pageNumber = value;
            }
        }
    }

    private int _pageSize = 5;
    public int PageSize
    {
        get
        {
            return _pageSize;
        }

        set
        {
            if(value > 0)
            {
                _pageSize = value;
            }
        }
    }

    public int TotalCount { get; set; }
    public int TotalPages { get; set; }

    public PagedList()
    {
    }

    public PagedList(IQueryable<T> source, int pageNumber, int pageSize)
    {
        PageNumber = pageNumber;
        PageSize = pageSize;
        TotalCount = source.Count();
        TotalPages = (int)Math.Ceiling(TotalCount / (double)PageSize);

        this.AddRange(source.Skip(PageNumber * PageSize).Take(PageSize));
    }

    public bool HasPreviousPage => (PageNumber > 0);

    public bool HasNextPage => (PageNumber + 1 < TotalPages);
}
