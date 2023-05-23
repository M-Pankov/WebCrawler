using System;
using System.Collections.Generic;
using System.Linq;
using WebCrawler.Application.Validators;

namespace WebCrawler.Application.Helpers;

public class PagedList<T>
{
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 5;
    public int TotalCount { get; set; }
    public int TotalPages { get; set; }
    public List<T> Items { get; set; }

    public PagedList()
    {
        Items = new List<T>();
    }

    public PagedList(IQueryable<T> source, int pageNumber, int pageSize)
    {
        PageNumber = PageValidator.GetValidPageNumber(pageNumber);
        PageSize = PageValidator.GetValidPageSize(pageSize);
        TotalCount = source.Count();
        TotalPages = (int)Math.Ceiling(TotalCount / (double)PageSize);
        Items = new List<T>();
        Items.AddRange(source.Skip((PageNumber - 1) * PageSize).Take(PageSize));
    }

    public bool HasPreviousPage => (PageNumber > 1);

    public bool HasNextPage => (PageNumber < TotalPages);
}

