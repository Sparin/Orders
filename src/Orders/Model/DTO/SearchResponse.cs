using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Orders.Model.DTO
{
    public class SearchResponse<T>
    {
        public int TotalItems { get; set; }

        public int TotalPages { get; set; }

        public int CurrentPage { get; set; }

        public IEnumerable<T> Items { get; set; }


        public SearchResponse(int totalItems, int page, int limit, IEnumerable<T> items)
        {
            this.TotalItems = totalItems;
            this.TotalPages = totalItems / limit;
            this.TotalPages += totalItems % limit > 0 ? 1 : 0;
            this.CurrentPage = page;
            this.Items = items;
        }
    }
}
