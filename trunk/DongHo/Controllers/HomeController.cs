using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DongHo.Models;

namespace DongHo.Controllers
{
    public class HomeController : Controller
    {
        DataDataContext data = new DataDataContext();
        #region[Trang chu]
        public ActionResult Index(string req = "Hot nhất")
        {
            #region[Lay du lieu tu db theo dieu kien truyen vao]
            string chuoi = "";
            List<Product> list = new List<Product>();
            if (req == "Hot nhất")
            {
                list = data.Products.Where(m => m.Index == 1 && m.Active == 1).ToList();
            }
            else if (req == "Sản phẩm mới nhất")
            {
                list = data.Products.Where(m => m.Active == 1).OrderByDescending(m => m.Date).ToList();
            }
            else if (req == "Xem nhiều")
            {
                list = data.Products.Where(m => m.Active == 1).OrderByDescending(m => m.View).ToList();
            }
            else if (req == "Mua nhiều")
            {
                list = data.Products.Where(m => m.Active == 1).OrderByDescending(m => m.Count).ToList();
                //list = (from n in data.Products where (from m in data.tbBilldetails select m.proid).Contains(n.Id) select n).ToList();
            }
            else if (req == "Giảm giá nhiều")
            {
                list = data.Products.Where(m => m.Active == 1).OrderByDescending(m => m.Codepro).ToList();
            }
            Session["Type"] = req;
            #endregion
            #region[Hien thi san pham]
            for (int i = 0; i < list.Count; i++)
            {
                string anh = "";
                var proimg = data.ProImages.Where(m => m.ProId == list[i].Id).ToList();
                for (int k = 0; k < proimg.Count; k++)
                {
                    var a = proimg[k].Image.IndexOf("_noz");
                    if (a > 0)
                    {
                        anh = proimg[k].Image;
                        break;
                    }
                }
                chuoi += "<div class=\"div-pro\">";
                chuoi += "<a href=\"/sanpham/chitiet/" + list[i].Tag + "\">";
                chuoi += "<img src=\"" + anh + "\" />";
                chuoi += "<div class=\"titlePro\">";
                chuoi += "<p>"+FormatContentNews(list[i].Name,50)+"</p>";
                chuoi += "<span class=\"require\">" + StringClass.Format_Price(list[i].Price.ToString()) + " VNĐ</span>";
                chuoi += "<span class=\"oldPrice\">" + StringClass.Format_Price(list[i].PiceOld) + " VNĐ</span>";
                chuoi += "<div class=\"hideTitlePro\">";
                chuoi += "<span class=\"require\">Giảm giá " + list[i].Codepro + " %</span>";
                chuoi += "<div style=\"padding:8px;\">" + list[i].Content + "</div>";
                chuoi += "</div>";
                chuoi += "</div>";
                chuoi += "</a>";
                chuoi += "</div>";
            }
            ViewBag.View = chuoi;
            #endregion
            #region[Ho tro truc tuyen - hien thi o trang chu]
            string support = "";
            var supp = data.Supports.Where(m => m.Active == 1).OrderBy(m => m.Type).ToList();
            if (supp.Count > 0)
            {
                support += "<p>";
                for (int i = 0; i < supp.Count; i++)
                {
                    if (supp[i].Type == 0)
                    {
                        support += "<a href=\"ymsgr:sendIM?" + supp[i].Nick + "\"><img src=\"http://opi.yahoo.com/online?u=" + supp[i].Nick + "&amp;m=g&amp;t=1 border=0\" /></a>";
                    }
                    else if (supp[i].Type == 1)
                    {
                        support += "<a href=\"skype:" + supp[i].Nick + "?chat\"><img src=\"HTTP://MYSTATUS.SKYPE.COM/smallclassic/" + supp[i].Nick + "\" /></a>";
                    }
                    else
                    {
                        support += "</p>";
                        support += "<p class=\"hotline\">Hotline: <span>" + supp[i].Tel + "</span></p>";
                    }
                }
            }
            ViewBag.Support = support;
            #endregion
            return View();
        }
        #endregion
        #region[Xem gio hang - nam o tren header]
        public ActionResult showCartTop()
        {
            string chuoi = "";
            chuoi+="<span class=\"numCart\">0</span>";
            chuoi += "<div class=\"showCartTop\">";
            chuoi += "<p>Hiện không có sản phẩm nào trong giỏ hàng</p>";
            chuoi += "</div>";
            return PartialView();
        }
        #endregion
        #region[Cat chuoi text de hien thi]
        protected string FormatContentNews(string value, int count)
        {
            string _value = value;
            if (_value.Length >= count)
            {
                string ValueCut = _value.Substring(0, count - 3);
                string[] valuearray = ValueCut.Split(' ');
                string valuereturn = "";
                for (int i = 0; i < valuearray.Length - 1; i++)
                {
                    valuereturn = valuereturn + " " + valuearray[i];
                }
                return valuereturn;
            }
            else
            {
                return _value;
            }
        }
        #endregion
    }
}
