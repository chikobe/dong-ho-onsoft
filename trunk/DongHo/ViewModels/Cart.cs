using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DongHo.ViewModels
{
    public class Cart
    {
        public int productId { get; set; }
        public string productImage { get; set; }
        public string productName { get; set; }
        public string productTag { get; set; }
        public string price { get; set; }
        public int count { get; set; }
        public int total { get; set; }
    }
}