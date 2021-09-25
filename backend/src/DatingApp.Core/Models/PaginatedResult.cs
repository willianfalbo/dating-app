using System;
using System.Collections.Generic;

namespace DatingApp.Core.Models
{
    /// <summary>
    /// Class to support paginated entities result.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class Paginated<T>
    {
        public int Page { get; set; } // current page number
        public int Limit { get; set; } // items to show per page
        public int TotalItems { get; set; } // total rows from the database
        public int TotalPages { get; set; } // total pages to paginate
        public IEnumerable<T> Items { get; set; }

        public Paginated(IEnumerable<T> items, int totalItems, int page, int limit)
        {
            Page = page;
            Limit = limit;
            TotalItems = totalItems;
            TotalPages = (int)Math.Ceiling(totalItems / (double)limit);
            Items = items;
        }
    }
}