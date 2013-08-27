using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;

namespace DongHo.ViewModels
{
    public class SearchCatInfo
    {
        private string _Id;
        private string _Name;
        private string _NumberOfCat;
        public string Id { get { return _Id; } set { _Id = value; } }
        public string Name { get { return _Name; } set { _Name = value; } }
        public string NumberOfCat { get { return _NumberOfCat; } set { _NumberOfCat = value; } }
        public SearchCatInfo SearchCatIDataReader(IDataReader dr)
        {
            ViewModels.SearchCatInfo obj = new SearchCatInfo();
            obj.Id = (dr["Id"] is DBNull) ? string.Empty : dr["Id"].ToString();
            obj.Name = (dr["Name"] is DBNull) ? string.Empty : dr["Name"].ToString();
            obj.NumberOfCat = (dr["NumberOfCat"] is DBNull) ? string.Empty : dr["NumberOfCat"].ToString();
            return obj;
        }
    }
}