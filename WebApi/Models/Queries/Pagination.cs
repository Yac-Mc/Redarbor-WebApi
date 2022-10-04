using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApi.Models.Queries
{
    public class Pagination<T>
    {
        public int PageSize { get; set; } = 25;
        public int Page { get; set; } = 1;
        public string Sort { get; set; } = "1";
        public string SortDirection { get; set; } = "ASC";
        public List<Filter> Filters { get; set; }
        public int PagesQuantity { get; set; }
        public IEnumerable<T> Data { get; set; }
        public long TotalRows { get; set; }
    }
}
