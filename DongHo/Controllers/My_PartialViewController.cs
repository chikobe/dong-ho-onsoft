using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DongHo.Models;

namespace DongHo.Controllers
{
    public class My_PartialViewController : Controller
    {
        //
        // GET: /My_PartialView/
        DataDataContext data = new DataDataContext();
        #region[Logo]
        public ActionResult Logo()
        {
            var logo = data.Advertises.Where(m => m.Position == 1 & m.Active == true).FirstOrDefault();
            ViewBag.View = "<a href=\"" + logo.Link + "\"><img src=\"" + logo.Image + "\"  width=\"" + logo.Width + "\" height=\"" + logo.Height + "\"  /></a>";
            return PartialView();
        }
        #endregion
        #region[hien thi header top]
        public ActionResult _HeaderTop()
        {
            string chuoi = "";
            if (Session["Email"] == null)
            {
                chuoi += "<ul>";
                chuoi += "<li><a href=\"/Pages/dangnhap\">Đăng nhập</a></li>";
                chuoi += "<li><a href=\"/Pages/dangki\">Đăng ký</a></li>";
                chuoi += "</ul>";
            }
            else
            {
                var list = data.tbCUSTOMERs.Where(m => m.vemail == Session["Email"].ToString()).FirstOrDefault();
                if (list != null)
                {
                    chuoi += "<ul>";
                    chuoi += "<li><a href=\"/Pages/member\">"+ list.vcusname +"</a></li>";
                    chuoi += "<li><a href=\"/Pages/dangnhap\">Đăng xuất</a></li>";
                    chuoi += "</ul>";
                }
            }
            ViewBag.View = chuoi;
            return PartialView();
        }
        #endregion
        #region[Tim kiem]
        [ChildActionOnly]
        public ActionResult timkiem()
        {
            return PartialView();
        }
        #endregion
        #region[Tim kiem]
        [HttpPost]
        public ActionResult timkiemTop(FormCollection collection)
        {
            var search = collection["Search"];
            var seachWord = search.Replace(" ", "+");
            return Redirect("/Pages/timkiem?searchword=" + seachWord + "");
        }
        #endregion
        #region[Tim kiem]
        [HttpPost]
        public ActionResult search(string query)
        {
            string chuoi = "";
            if (query == "")
            {
                return PartialView();
            }
            else
            {
                List<Product> listData = null;
                //checking the query parameter sent from view. If it is null we will return null else we will return list based on query.
                var list = (from n in data.Products select n).ToList();
                if (!string.IsNullOrEmpty(query))
                {
                    listData = list.Where(q => q.Name.ToLower().Contains(query.ToLower())).Take(6).ToList();
                    if (listData.Count > 0)
                    {
                        chuoi += "<ul id='searchUL'>";
                        for (int i = 0; i < listData.Count; i++)
                        {
                            chuoi += "<li>";
                            chuoi += "<a href='/sanpham/chitiet/" + listData[i].Tag + "'>";
                            chuoi += "<img src='" + listData[i].Image + "'/>";
                            chuoi += "<span>" + FormatContentNews(listData[i].Name,50) + " VNĐ</span>";
                            if (listData[i].PiceOld == "0")
                            {
                                chuoi += "<p>" + StringClass.Format_Price(listData[i].Price.ToString()) + "</p>";
                            }
                            else
                            {
                                chuoi += "<p>" + StringClass.Format_Price(listData[i].PiceOld) + "</p>";
                            }
                            chuoi += "<div class='clear'></div>";
                            chuoi += "</a>";
                            chuoi += "</li>";
                        }
                        chuoi += "</ul>";
                    }
                }
                else
                {
                    chuoi = null;
                }
                //Returning the matched list as json data.
                return Json(new { success = chuoi });
            }
        }
        #endregion
        #region[menu]
        public ActionResult _menu()
        {
            string chuoi = "";
            string chuoisub = "";
            int j = 0;
            var menu = data.Pages.Where(m => m.Level.Length == 5 && m.Index == 1 && m.Active == 1).ToList();
            List<string> color = new List<string>();
            color.Add("#ed1c24");
            color.Add("#f99d1c");
            color.Add("#bed62f");
            color.Add("#38bb8d");
            color.Add("#0097d8");
            chuoi += "<ul>";
            for (int i = 0; i < menu.Count; i++)
            {
                var menusub = data.Pages.Where(m => m.Level.Length == menu[i].Level.Length + 5 && m.Level.Substring(0, menu[i].Level.Length) == menu[i].Level && m.Index == 1 && m.Active == 1).ToList();
                if (menusub.Count > 0)
                {
                    chuoi += "<li><a href=\"" + menu[i].Link + "\" rel=\"ddsubmenu" + (i + 1) + "\" style=\"color : " + color[j] + "\">" + menu[i].Name + "</a></li>";
                    chuoisub += "<ul id=\"ddsubmenu" + (i + 1) + "\" class=\"ddsubmenustyle\">";
                    for (int k = 0; k < menusub.Count; k++)
                    {
                        chuoisub += "<li><a href=\"" + menusub[k].Link + "\">" + menusub[k].Name + "</a></li>";
                    }
                    chuoisub += "</ul>";
                }
                else
                {
                    chuoi += "<li><a href=\"" + menu[i].Link + "\" style=\"color : " + color[j] + "\">" + menu[i].Name + "</a></li>";
                }
                j++;
                if (j == color.Count) j = 0;
            }
            chuoi += "</ul>";
            ViewBag.menu = chuoi;
            ViewBag.menusub = chuoisub;
            return PartialView();
        }
        #endregion
        #region[DropCategory]
        public ActionResult _DropCategory()
        {
            string chuoi = "";
            string chuoisub = "";
            var cat = data.Pages.Where(m => m.Level.Length == 5 && m.Active == 1 && m.Index==1).ToList();
            chuoi += "<ul class=\"sub-level1 clearfix\">";
            for (int i = 0; i < cat.Count; i++)
            {
                var catsub = data.Pages.Where(m => m.Level.Length == 10 && m.Level.Substring(0, 5) == cat[i].Level && m.Active == 1 && m.Index == 1).ToList();
                if (catsub.Count > 0)
                {
                    chuoi += "<li id=\"nav_cat_" + i + "\" class=\"par_cat\"><a href=\"/sanpham/sp/" + cat[i].Tag + "\">" + cat[i].Name + "</a></li>";
                    chuoisub += "<div id=\"sub_cat_" + i + "\" class=\"wrap-sub\">";
                    chuoisub += "<ul class=\"sub-level2\">";
                    for (int k = 0; k < catsub.Count; k++)
                    {
                        chuoisub += "<li><a href=\"/sanpham/sp/" + catsub[k].Tag + "\">" + catsub[k].Name + "</a></li>";
                    }
                    chuoisub += "</ul>";
                    chuoisub += "</div>";
                }
                else
                {
                    chuoi += "<li id=\"nav_cat_" + i + "\" class='no_sub'><a href=\"/sanpham/sp/" + cat[i].Tag + "\">" + cat[i].Name + "</a></li>";
                }
            }
            chuoi += "</ul>";
            ViewBag.Cat = chuoi;
            ViewBag.Catsub = chuoisub;
            return PartialView();
        }
        #endregion
        #region[Ho tro truc tuyen]
        public ActionResult _support()
        {
            string chuoi = "";
            var list = data.Supports.Where(m => m.Active == 1).OrderBy(m=>m.Type).ToList();
            if (list.Count > 0)
            {
                chuoi += "<ul>";
                for (int i = 0; i < list.Count; i++)
                {
                    if (list[i].Type == 1)
                    {
                        chuoi += "<li><a href=\"skype:" + list[i].Nick + "?chat\"><img src=\"HTTP://MYSTATUS.SKYPE.COM/smallclassic/" + list[i].Nick + "\" title=\"Tư vấn online\"/></a></li>";
                    }
                    else if (list[i].Type == 0)
                    {
                        chuoi += "<li><a href=\"ymsgr:sendIM?" + list[i].Nick + "\"><img src=\"http://opi.yahoo.com/online?u=" + list[i].Nick + "&amp;m=g&amp;t=1 border=0\" title=\"Tư vấn online\"/></a></li>";
                    }
                    else
                    {
                        chuoi += "<li><p style=\"line-height:40px;\">Hotline: <span>" + list[i].Tel + "</span></p></li>";
                    }
                }
                chuoi += "</ul>";
                ViewBag.View = chuoi;
            }
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
        #region[QuangCao2Ben_benTrai]
        public ActionResult _quangCao2Ben_benTrai()
        {
            string chuoi = "";
            var list = data.Advertises.Where(m => m.Position == 3 && m.Active == true).ToList();
            if (list.Count > 0)
            {
                for (int i = 0; i < list.Count; i++)
                {
                    chuoi += "<div class=\"slide\">";
                    chuoi += "<a href=\"" + list[i].Link + "\" title=\"" + list[i].Name + "\" target=\"" + list[i].Target + "\"><img src=\"" + list[i].Image + "\" width=\"" + list[i].Width + "\" height=\"" + list[i].Height + "\" alt=\"Images " + (i + 1) + "\"></a>";
                    chuoi += "</div>";
                }
            }
            ViewBag.View = chuoi;
            return PartialView();
        }
        #endregion
        #region[QuangCao2Ben_benPhai]
        public ActionResult _quangCao2Ben_benPhai()
        {
            string chuoi = "";
            var list = data.Advertises.Where(m => m.Position == 2 && m.Active == true).ToList();
            if (list.Count > 0)
            {
                for (int i = 0; i < list.Count; i++)
                {
                    chuoi += "<div class=\"slide\">";
                    chuoi += "<a href=\"" + list[i].Link + "\" target=\"" + list[i].Target + "\"><img src=\"" + list[i].Image + "\" width=\"" + list[i].Width + "\" height=\"" + list[i].Height + "\" alt=\"Images " + (i + 1) + "\"></a>";
                    chuoi += "</div>";
                }
            }
            ViewBag.View = chuoi;
            return PartialView();
        }
        #endregion
    }
}
