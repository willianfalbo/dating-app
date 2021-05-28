using System;
using System.Collections.Generic;

namespace DatingApp.Core.Models
{
    public class PagedResult<T> : List<T>
    {
        public int CurrentPage { get; set; } // current page number
        public int TotalPages { get; set; } // total pages to paginate
        public int PageSize { get; set; } // items to show per page
        public int TotalCount { get; set; } // row count from db query

        public PagedResult(IEnumerable<T> items, int count, int page, int pageSize)
        {
            CurrentPage = page;
            TotalPages = (int)Math.Ceiling(count / (double)pageSize);
            PageSize = pageSize;
            TotalCount = count;
            this.AddRange(items);
        }
    }
}