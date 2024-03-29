﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DongHo.Models;

namespace DongHo.Controllers
{
    public class BrandsController : Controller
    {
        //
        // GET: /Brands/

        DataDataContext data = new DataDataContext();
        #region[BrandsIndex]
        public ActionResult BrandsIndex()
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
            var all = data.Brands.ToList();
            var pages = all.Skip(curpage * pagesize).Take(pagesize).ToList();
            //var pages = data.sp_Brands_Phantrang(page, productize, "", "").ToList();
            var url = Request.Path;
            numOfNews = all.Count;
            if (numOfNews > 0)
            {
                ViewBag.Pager = Phantrang.PhanTrang(pagesize, curpage, numOfNews, url);
            }
            else
            {
                ViewBag.Pager = "";
            }
            return View(pages);
        }
        #endregion
        #region[BrandsCreate]
        public ActionResult BrandsCreate()
        {
            ViewBag.Supplier = new SelectList(data.Suppliers, "Id", "Name");
            return View();
        }
        #endregion
        #region[BrandsCreate]
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult BrandsCreate(FormCollection collection, Brand brand)
        {
            if (Request.Cookies["Username"] != null)
            {
                var Name = collection["Name"];
                var Ord = collection["Ord"];
                var Logo = collection["Logo"];
                brand.Name = Name;
                brand.Logo = Logo;
                brand.Ord = Convert.ToInt32(Ord);
                brand.Lang = "vi";
                brand.SupplierId = int.Parse(collection["Supplier"]);
                data.Brands.InsertOnSubmit(brand);
                data.SubmitChanges();
                return RedirectToAction("BrandsIndex");
            }
            else
            {
                return Redirect("/Admins/admins");
            }
        }
        #endregion
        #region[BrandsEdit]
        public ActionResult BrandsEdit(int id)
        {
            var Edit = data.Brands.First(m => m.Id == id);
            ViewBag.Supplier = new SelectList(data.Suppliers, "Id", "Name", Edit.SupplierId);
            return View(Edit);
        }
        #endregion
        #region[BrandsEdit]
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult BrandsEdit(int id, FormCollection collection)
        {
            if (Request.Cookies["Username"] != null)
            {
                var brand = data.Brands.First(model => model.Id == id);
                var Name = collection["Name"];
                var Ord = collection["Ord"];
                var Logo = collection["Logo"];
                brand.Name = Name;
                brand.Logo = Logo;
                brand.Ord = Convert.ToInt32(Ord);
                brand.SupplierId = int.Parse(collection["Supplier"]);
                data.SubmitChanges();
                return RedirectToAction("BrandsIndex");
            }
            else
            {
                return Redirect("/Admins/admins");
            }
        }
        #endregion
        #region[BrandsDelete]
        public ActionResult BrandsDelete(int id)
        {
            if (Request.Cookies["Username"] != null)
            {
                var del = (from brand in data.Brands where brand.Id == id select brand).Single();
                data.Brands.DeleteOnSubmit(del);
                data.SubmitChanges();
                return RedirectToAction("BrandsIndex");
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
            if (Request.Cookies["Username"] != null)
            {
                foreach (string key in Request.Form)
                {
                    var checkbox = "";
                    if (key.StartsWith("chk"))
                    {
                        checkbox = Request.Form["" + key];
                        if (checkbox != "false")
                        {
                            Int32 id = Convert.ToInt32(key.Remove(0, 3));
                            var Del = (from emp in data.Brands where emp.Id == id select emp).SingleOrDefault();
                            data.Brands.DeleteOnSubmit(Del);
                            data.SubmitChanges();
                        }
                    }
                }
                return RedirectToAction("BrandsIndex");
            }
            else
            {
                return Redirect("/Admins/admins");
            }
        }
        #endregion
    }
}
