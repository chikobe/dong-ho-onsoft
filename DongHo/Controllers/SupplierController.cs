using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DongHo.Models;

namespace DongHo.Controllers
{
    public class SupplierController : Controller
    {
        //
        // GET: /Supplier/
        DataDataContext data = new DataDataContext();
        #region[SupplierIndex]
        public ActionResult SupplierIndex()
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
            var all = data.Suppliers.ToList();
            var pages = all.Skip(curpage * pagesize).Take(pagesize).ToList();
            //var pages = data.sp_Supplier_Phantrang(page, productize, "", "").ToList();
            var url = Request.Path;
            numOfNews = all.Count;
            if (pages.Count > 0)
            {
                ViewBag.Pager = DongHo.Models.Phantrang.PhanTrang(pagesize, curpage, numOfNews, url);
            }
            return View(pages);
        }
        #endregion
        #region[SupplierCreate]
        public ActionResult SupplierCreate()
        {
            return View();
        }
        #endregion
        #region[SupplierCreate]
        [HttpPost]
        public ActionResult SupplierCreate(FormCollection collect, Supplier sup)
        {
            if (Request.Cookies["Username"] != null)
            {
                sup.Name = collect["Name"];
                sup.Phone = collect["Phone"];
                sup.Email = collect["Email"];
                sup.Website = collect["Website"];
                sup.Contact = collect["Contact"];
                sup.Address = collect["Address"];
                sup.National = collect["National"];
                sup.Ord = int.Parse(collect["Ord"]);
                data.Suppliers.InsertOnSubmit(sup);
                data.SubmitChanges();
                return RedirectToAction("SupplierIndex");
            }
            else
            {
                return Redirect("/Admins/admins");
            }
        }
        #endregion
        #region[SupplierEdit]
        public ActionResult SupplierEdit(int id)
        {
            var Edit = data.Suppliers.First(m => m.Id == id);
            return View(Edit);
        }
        #endregion
        #region[SupplierEdit]
        [HttpPost]
        public ActionResult SupplierEdit(FormCollection collect, int id)
        {
            if (Request.Cookies["Username"] != null)
            {
                var sup = data.Suppliers.First(m => m.Id == id);
                sup.Name = collect["Name"];
                sup.Phone = collect["Phone"];
                sup.Email = collect["Email"];
                sup.Website = collect["Website"];
                sup.Contact = collect["Contact"];
                sup.Address = collect["Address"];
                sup.National = collect["National"];
                sup.Ord = int.Parse(collect["Ord"]);
                data.SubmitChanges();
                return RedirectToAction("SupplierIndex");
            }
            else
            {
                return Redirect("/Admins/admins");
            }
        }
        #endregion
        #region[SupplierDelete]
        public ActionResult SupplierDelete(int id)
        {
            if (Request.Cookies["Username"] != null)
            {
                var del = data.Suppliers.First(m => m.Id == id);
                data.Suppliers.DeleteOnSubmit(del);
                data.SubmitChanges();
                return RedirectToAction("SupplierIndex");
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
                            var Del = (from emp in data.Suppliers where emp.Id == id select emp).SingleOrDefault();
                            data.Suppliers.DeleteOnSubmit(Del);
                            data.SubmitChanges();
                        }
                    }
                }
                return RedirectToAction("SupplierIndex");
            }
            else
            {
                return Redirect("/Admins/admins");
            }
        }
        #endregion
    }
}
