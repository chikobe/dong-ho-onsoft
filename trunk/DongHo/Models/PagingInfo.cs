using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DongHo.Models
{
    public class PagingInfo<T>
    {
        public int PageId { get; set; }
        public int PageSize { get; set; }
        public int ItemCount { get; set; }
        public IList<T> Items { get; set; }
    }
}