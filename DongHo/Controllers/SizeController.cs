using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DongHo.Models;

namespace DongHo.Controllers
{
    public class SizeController : Controller
    {
        //
        // GET: /Size/

        DataDataContext data = new DataDataContext();
        #region[SizeIndex]
        public ActionResult SizeIndex()
        {
            if (Session["Username"] != null)
            {
                string page = "1";//so phan trang hien tai
                var productize = "25";//so ban ghi tren 1 trang
                var numOfNews = 0;//tong so ban ghi co duoc truoc khi phan trang
                int curpage = 0; // trang hien tai dung cho phan trang
                if (Request["page"] != null)
                {
                    page = Request["page"];
                    curpage = Convert.ToInt32(page) - 1;
                }
                var all = data.Sizes.ToList();
                var product = data.sp_Sizes_Phantrang(page, productize, "", "").ToList();
                var url = Request.Path;
                numOfNews = all.Count;
                ViewBag.Pager = DongHo.Models.Phantrang.PhanTrang(25, curpage, numOfNews, url);
                return View(product);
            }
            else
            {
                return Redirect("/Admins/admins");
            }
        }
        #endregion
        #region[SizeCreate]
        public ActionResult SizeCreate()
        {
            if (Session["Username"] != null)
            {
                return View();
            }
            else
            {
                return Redirect("/Admins/admins");
            }
        }
        #endregion
        #region[SizeCreate]
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult SizeCreate(FormCollection collection, Size sizes)
        {
            if (Session["Username"] != null)
            {
                var Name = collection["Name"];
                var Des = collection["Des"];
                sizes.Lang = "vi";
                sizes.Name = Name;
                sizes.Des = Des;
                data.Sizes.InsertOnSubmit(sizes);
                data.SubmitChanges();
                return RedirectToAction("SizeIndex");
            }
            else
            {
                return Redirect("/Admins/admins");
            }
        }
        #endregion
        #region[SizeEdit]
        public ActionResult SizeEdit(int id)
        {
            if (Session["Username"] != null)
            {
                var Edit = data.Sizes.First(m => m.Id == id);
                return View(Edit);
            }
            else
            {
                return Redirect("/Admins/admins");
            }
        }
        #endregion
        #region[SizeEdit]
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult SizeEdit(int id, FormCollection collection)
        {
            if (Session["Username"] != null)
            {
                var sizes = data.Sizes.First(model => model.Id == id);
                var Name = collection["Name"];
                var Des = collection["Des"];
                sizes.Lang = "vi";
                sizes.Name = Name;
                sizes.Des = Des;
                data.SubmitChanges();
                return RedirectToAction("SizeIndex");
            }
            else
            {
                return Redirect("/Admins/admins");
            }
        }
        #endregion
        #region[SizeDelete]
        public ActionResult SizeDelete(int id)
        {
            if (Session["Username"] != null)
            {
                if (ModelState.IsValid)
                {
                    var del = (from size in data.Sizes where size.Id == id select size).Single();
                    data.Sizes.DeleteOnSubmit(del);
                    data.SubmitChanges();
                }
                return RedirectToAction("SizeIndex");
            }
            else
            {
                return Redirect("/Admins/admins");
            }
        }
        #endregion
        #region[MultiDelete]
        [HttpPost]
        public ActionResult MultiDelete()
        {
            if (Session["Username"] != null)
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
                            var Del = (from emp in data.Sizes where emp.Id == id select emp).SingleOrDefault();
                            data.Sizes.DeleteOnSubmit(Del);
                            data.SubmitChanges();
                        }
                    }
                }
                return RedirectToAction("SizeIndex");
            }
            else
            {
                return Redirect("/Admins/admins");
            }
        }
        #endregion
    }

   
}
