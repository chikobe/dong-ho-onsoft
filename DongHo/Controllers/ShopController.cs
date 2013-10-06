using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DongHo.Models;

namespace DongHo.Controllers
{
    public class ShopController : Controller
    {
        //
        // GET: /Shop/
        DataDataContext data = new DataDataContext();
        #region[ShopIndex]
        public ActionResult ShopIndex()
        {
            string page = "1";//so phan trang hien tai
            var pagesize = 25;//so ban ghi tren 1 trang
            var numOfNews = 0;//tong so ban ghi co duoc truoc khi phan trang
            int curpage = 0; // trang hien tai dung cho phan trang
            if (Request["page"] != null)
            {
                page = Request["page"];
                curpage = Convert.ToInt32(page) - 1;
            }
            var all = data.Shops.ToList();
            var pages = all.Skip(curpage * pagesize).Take(pagesize).ToList();
            //var pages = data.sp_Shop_Phantrang(page, pagesize, "", "").ToList();
            var url = Request.Path;
            numOfNews = all.Count;
            ViewBag.Pager = DongHo.Models.Phantrang.PhanTrang(pagesize, curpage, numOfNews, url);
            return View(pages);
        }
        #endregion
        #region[ShopCreate]
        public ActionResult ShopCreate()
        {
            return View();
        }
        #endregion
        #region[ShopCreate]
        [HttpPost]
        public ActionResult ShopCreate(FormCollection collec, Shop shop)
        {
            if (Session["Username"] != null)
            {
                shop.Name = collec["Name"];
                shop.Address = collec["Address"];
                shop.PhoneNumber = collec["PhoneNumber"];
                shop.CompanyId = 1;
                data.Shops.InsertOnSubmit(shop);
                data.SubmitChanges();
                return RedirectToAction("ShopIndex");
            }
            else
            {
                return Redirect("/Admins/admins");
            }
        }
        #endregion
        #region[ShopEdit]
        public ActionResult ShopEdit(int id)
        {
            var Edit = data.Shops.First(m => m.id == id);
            return View(Edit);
        }
        #endregion
        #region[ShopEdit]
        [HttpPost]
        public ActionResult ShopEdit(FormCollection collec, int id)
        {
            if (Session["Username"] != null)
            {
                var shop = data.Shops.First(m => m.id == id);
                shop.Name = collec["Name"];
                shop.Address = collec["Address"];
                shop.PhoneNumber = collec["PhoneNumber"];
                data.SubmitChanges();
                return RedirectToAction("ShopIndex");
            }
            else
            {
                return Redirect("/Admins/admins");
            }
        }
        #endregion
        #region[ShopDelete]
        public ActionResult ShopDelete(int id)
        {
            if (Session["Username"] != null)
            {
                var del = data.Shops.First(m => m.id == id);
                data.Shops.DeleteOnSubmit(del);
                data.SubmitChanges();
                return RedirectToAction("ShopIndex");
            }
            else
            {
                return Redirect("/Admins/admins");
            }
        }
        #endregion
        #region[MultiDelete]
        public ActionResult MultiDelete()
        {
            if (Session["Username"] != null)
            {
                string str = "";
                foreach (string key in Request.Form)
                {
                    var checkbox = "";
                    if (key.StartsWith("chk"))
                    {
                        checkbox = Request.Form["" + key];
                        if (checkbox != "false")
                        {
                            Int32 id = Convert.ToInt32(key.Remove(0, 3));
                            var Del = (from emp in data.Shops where emp.id == id select emp).SingleOrDefault();
                            data.Shops.DeleteOnSubmit(Del);
                            str += id.ToString() + ",";
                            data.SubmitChanges();
                        }
                    }
                }
                return RedirectToAction("ShopIndex");
            }
            else
            {
                return Redirect("/Admins/admins");
            }
        }
        #endregion
    }
}
