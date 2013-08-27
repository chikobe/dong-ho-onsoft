using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DongHo.Models;
namespace DongHo.Controllers
{
    public class ColorController : Controller
    {
        //
        // GET: /Color/

        DataDataContext data = new DataDataContext();
        #region[ColorIndex]
        public ActionResult ColorIndex()
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
                var all = data.Colors.ToList();
                var product = data.sp_Colors_Phantrang(page, productize, "", "").ToList();
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
        #region[ColorCreate]
        public ActionResult ColorCreate()
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
        #region[ColorCreate]
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult ColorCreate(FormCollection collection, Color color)
        {
            if (Session["Username"] != null)
            {
                var Name = collection["Name"];
                var Img = collection["Img"];
                color.Lang = "vi";
                color.Name = Name;
                color.Img = Img;
                data.Colors.InsertOnSubmit(color);
                data.SubmitChanges();
                return RedirectToAction("ColorIndex");
            }
            else
            {
                return Redirect("/Admins/admins");
            }
        }
        #endregion
        #region[ColorEdit]
        public ActionResult ColorEdit(int id)
        {
            if (Session["Username"] != null)
            {
                var Edit = data.Colors.First(m => m.Id == id);
                return View(Edit);
            }
            else
            {
                return Redirect("/Admins/admins");
            }
        }
        #endregion
        #region[ColorEdit]
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult ColorEdit(int id, FormCollection collection)
        {
            if (Session["Username"] != null)
            {
                var color = data.Colors.First(model => model.Id == id);
                var Name = collection["Name"];
                var Img = collection["Img"];
                color.Lang = "vi";
                color.Name = Name;
                color.Img = Img;
                data.SubmitChanges();
                return RedirectToAction("ColorIndex");
            }
            else
            {
                return Redirect("/Admins/admins");
            }
        }
        #endregion
        #region[ColorDelete]
        public ActionResult ColorDelete(int id)
        {
            if (Session["Username"] != null)
            {
                var del = (from color in data.Colors where color.Id == id select color).Single();
                data.Colors.DeleteOnSubmit(del);
                data.SubmitChanges();
                return RedirectToAction("ColorIndex");
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
                            var Del = (from emp in data.Colors where emp.Id == id select emp).SingleOrDefault();
                            data.Colors.DeleteOnSubmit(Del);
                            data.SubmitChanges();
                        }
                    }
                }
                return RedirectToAction("ColorIndex");
            }
            else
            {
                return Redirect("/Admins/admins");
            }
        }
        #endregion
    }
}
