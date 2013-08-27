using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DongHo.ViewModels
{
    public class SearchCatBusiness
    {
        private static SearchCatControl db = new SearchCatControl();
        public static List<SearchCatInfo> Product_SearchByCat(string where)
        {
            return db.Product_SearchByCat(where);
        }
    }
}