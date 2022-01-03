using System;
using System.Collections.Generic;
using System.Linq;

namespace Cars.Models.View;

public class PaginatedList<T>
{
    private PaginatedList(IEnumerable<T> items, int count, int pageIndex, int pageSize)
    {
        PageIndex = pageIndex;
        TotalPages = (int)Math.Ceiling(count / (double)pageSize);
        TotalItems = count;
        Items.AddRange(items);
    }

    public List<T> Items { get; set; } = new();
    public int PageIndex { get; }
    public int TotalPages { get; }

    public int TotalItems { get; }

    public static PaginatedList<T> CreateAsync(List<T> source, int pageIndex, int pageSize)
    {
        var count = source.Count;
        var items = source.Skip(pageIndex * pageSize).Take(pageSize);
        return new PaginatedList<T>(items, count, pageIndex, pageSize);
    }
}