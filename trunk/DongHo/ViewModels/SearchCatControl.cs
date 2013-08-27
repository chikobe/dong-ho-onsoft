using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;

namespace DongHo.ViewModels
{
    public class SearchCatControl : ViewModels.SqlDataProvider
    {
        #region[Product_SearchByCat]
        public List<SearchCatInfo> Product_SearchByCat(string where)
        {
            List<ViewModels.SearchCatInfo> list = new List<ViewModels.SearchCatInfo>();
            using (SqlCommand dbCmd = new SqlCommand("sp_Product_SearchByCat", GetConnection()))
            {
                ViewModels.SearchCatInfo obj = new ViewModels.SearchCatInfo();
                dbCmd.CommandType = CommandType.StoredProcedure;
                dbCmd.Parameters.Add(new SqlParameter("@where", where));
                SqlDataReader dr = dbCmd.ExecuteReader();
                if (dr.HasRows)
                {
                    while (dr.Read())
                    {
                        list.Add(obj.SearchCatIDataReader(dr));
                    }
                }
                dr.Close();
                obj = null;
            }
            return list;
        }
        #endregion
    }
}