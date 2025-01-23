using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestBlogCore.Models
{
    public class paginatedList<T> : List<T>
    {
        public int PageIndex { get; private set; }
        public int TotalPages { get; private set; }
        public string SearchString { get; private set; }

        public paginatedList(List<T> items, int count, int pageIndex, int pageSize, string searchString)
        {
            PageIndex = pageIndex;
            TotalPages = (int)Math.Ceiling(count / (double)pageSize);
            this.SearchString = searchString;
            this.AddRange(items);
        }

        public bool hasPreviousPage => (PageIndex > 1);
        public bool hasNextPage => (PageIndex < TotalPages);
    }
}
