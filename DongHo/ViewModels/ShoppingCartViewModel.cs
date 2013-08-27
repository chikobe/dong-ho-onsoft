using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DongHo.ViewModels
{
    public class ShoppingCartViewModel
    {
        public ShoppingCartViewModel()
        {
            this.CartItems = new List<Cart>();
        }
        public List<Cart> CartItems { get; set; }
        public decimal CartTotal { get; set; }
    }
}