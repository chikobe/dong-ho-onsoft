using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DongHo.Models;

namespace DongHo.Controllers
{
    public class CategoryController : Controller
    {
        //
        // GET: /Category/

        DataDataContext data = new DataDataContext();
        #region[CategoryIndex]
        public ActionResult CategoryIndex()
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
            var all = data.Categories.ToList();
            var pages = all.Skip(curpage * pagesize).Take(pagesize).ToList();
            //var pages = data.sp_Category_Phantrang(page, productize, "", "[Level] asc").ToList();
            var url = Request.Path;
            numOfNews = all.Count;
            if (numOfNews > 0)
            {
                ViewBag.Pager = DongHo.Models.Phantrang.PhanTrang(pagesize, curpage, numOfNews, url);
            }
            else
            {
                ViewBag.Pager = "";
            }
            return View(pages);
        }
        #endregion
        #region[CategoryCreate]
        public ActionResult CategoryCreate()
        {
            return View();
        }
        #endregion
        #region[CategoryCreate]
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult CategoryCreate(FormCollection collection, Category catego)
        {
            if (Session["Username"] != null)
            {
                var Name = collection["Name"];
                var Image = collection["Image"];
                var Content = collection["Content"];
                var Ord = collection["Ord"];
                var Active = (collection["Actives"] == "false") ? 0 : 1;
                var Index = (collection["Indexs"] == "false") ? 0 : 1;
                catego.Content = Content;
                catego.Tag = StringClass.NameToTag(Name);
                catego.Name = Name;
                catego.Image = Image;
                catego.Ord = Convert.ToInt32(Ord);
                catego.Active = Active;
                catego.Index = Convert.ToInt16(Index);
                catego.Description = collection["Description"];
                catego.Keyword = collection["Keyword"];
                catego.Title = collection["Title"];
                catego.Lang = "vi";
                data.Categories.InsertOnSubmit(catego);
                data.SubmitChanges();
                return RedirectToAction("CategoryIndex");
            }
            else
            {
                return Redirect("/Admins/admins");
            }
        }
        #endregion
        #region[CategoriesEdit]
        public ActionResult CategoriesEdit(int id)
        {
            var Edit = data.Categories.First(m => m.Id == id);
            return View(Edit);
        }
        #endregion
        #region[CategoriesEdit]
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult CategoriesEdit(int id, FormCollection collection)
        {
            if (Session["Username"] != null)
            {
                var catego = data.Categories.First(model => model.Id == id);
                var Name = collection["Name"];
                var Image = collection["Image"];
                var Content = collection["Content"];
                var Ord = collection["Ord"];
                var Active = (collection["Actives"] == "false") ? 0 : 1;
                var Index = (collection["Indexs"] == "false") ? 0 : 1;
                catego.Content = Content;
                catego.Tag = StringClass.NameToTag(Name);
                catego.Name = Name;
                catego.Image = Image;
                catego.Ord = Convert.ToInt32(Ord);
                catego.Active = Active;
                catego.Index = Convert.ToInt16(Index);
                catego.Description = collection["Description"];
                catego.Keyword = collection["Keyword"];
                catego.Title = collection["Title"];
                catego.Lang = "vi";
                data.SubmitChanges();
                return RedirectToAction("CategoryIndex");
            }
            else
            {
                return Redirect("/Admins/admins");
            }
        }
        #endregion
        #region[CategoryAddSub]
        public ActionResult CategoryAddSub()
        {
            return View();
        }
        #endregion
        #region[CategoryAddSub]
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult CategoryAddSub(FormCollection collection, Category catego, string level)
        {
            if (Session["Username"] != null)
            {
                var Name = collection["Name"];
                var Image = collection["Image"];
                var Content = collection["Content"];
                var Ord = collection["Ord"];
                var Active = (collection["Actives"] == "false") ? 0 : 1;
                var Index = (collection["Indexs"] == "false") ? 0 : 1;
                catego.Content = Content;
                catego.Tag = StringClass.NameToTag(Name);
                catego.Name = Name;
                catego.Image = Image;
                catego.Ord = Convert.ToInt32(Ord);
                catego.Active = Active;
                catego.Index = Convert.ToInt16(Index);
                catego.Description = collection["Description"];
                catego.Keyword = collection["Keyword"];
                catego.Title = collection["Title"];
                catego.Lang = "vi";
                catego.Level = level + "00000";
                data.Categories.InsertOnSubmit(catego);
                data.SubmitChanges();
                return RedirectToAction("CategoryIndex");
            }
            else
            {
                return Redirect("/Admins/admins");
            }
        }
        #endregion
        #region[CategoryDelete]
        public ActionResult CategoryDelete(int id)
        {
            if (Session["Username"] != null)
            {
                var del = (from categaa in data.Categories where categaa.Id == id select categaa).Single();
                data.Categories.DeleteOnSubmit(del);
                data.SubmitChanges();
                return RedirectToAction("CategoryIndex");
            }
            else
            {
                return Redirect("/Admins/admins");
            }
        }
        #endregion
        #region[CategoryActive]
        public ActionResult CategoryActive(int id)
        {
            if (Session["Username"] != null)
            {
                var act = (from catego in data.Categories where catego.Id == id select catego).Single();
                if (act.Active == 1)
                {
                    act.Active = 0;
                }
                else { act.Active = 1; }
                data.SubmitChanges();
                return RedirectToAction("CategoryIndex");
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
                foreach (string key in Request.Form)
                {
                    var checkbox = "";
                    if (key.StartsWith("chk"))
                    {
                        checkbox = Request.Form["" + key];
                        if (checkbox != "false")
                        {
                            Int32 id = Convert.ToInt32(key.Remove(0, 3));
                            var Del = (from emp in data.Categories where emp.Id == id select emp).SingleOrDefault();
                            data.Categories.DeleteOnSubmit(Del);
                            data.SubmitChanges();
                        }
                    }
                }
                return RedirectToAction("CategoryIndex");
            }
            else
            {
                return Redirect("/Admins/admins");
            }
        }
        #endregion
    }
}
