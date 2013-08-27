using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DongHo.Models
{
    public class ShoppingCart
    {
        DataDataContext Data = new DataDataContext();

        string ShoppingCartId { get; set; }

        public const string CartSessionKey = "CartId";

        public string GetCartId(HttpContextBase context)
        {
            if (context.Session[CartSessionKey] == null)
            {
                if (!string.IsNullOrWhiteSpace(context.User.Identity.Name))
                {
                    context.Session[CartSessionKey] = context.User.Identity.Name;
                }
                else
                {
                    // Generate a new random GUID using System.Guid class
                    Guid tempCartId = Guid.NewGuid();

                    // Send tempCartId back to client as a cookie
                    context.Session[CartSessionKey] = tempCartId.ToString();
                }
            }

            return context.Session[CartSessionKey].ToString();
        }
    }
}