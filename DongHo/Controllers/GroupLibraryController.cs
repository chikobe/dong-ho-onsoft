using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DongHo.Models;

namespace DongHo.Controllers
{
    public class GroupLibraryController : Controller
    {
        //
        // GET: /GroupLibrary/
        DataDataContext data = new DataDataContext();
        #region[GroupLibraryIndex]
        public ActionResult GroupLibraryIndex()
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
            var all = data.GroupLibraries.ToList();
            var pages = all.Skip(curpage * pagesize).Take(pagesize).ToList();
            //var pages = data.sp_GroupLibrary_Phantrang(page, pagesize, "", "[Level] asc").ToList();
            var url = Request.Path;
            numOfNews = all.Count;
            ViewBag.Pager = DongHo.Models.Phantrang.PhanTrang(25, curpage, numOfNews, url);
            return View(pages);
        }
        #endregion
        #region[GroupLibraryCreate]
        public ActionResult GroupLibraryCreate()
        {
            return View();
        }
        #endregion
        #region[GroupLibraryCreate]
        [HttpPost]
        public ActionResult GroupLibraryCreate(FormCollection collection, GroupLibrary gr)
        {
            if (Request.Cookies["Username"] != null)
            {
                gr.Name = collection["Name"];
                gr.Tag = StringClass.NameToTag(collection["Name"]);
                gr.Image = collection["Image"];
                gr.Ord = Convert.ToInt32(collection["Ord"]);
                var Active = (collection["Actives"] == "false") ? 0 : 1;
                gr.Active = Active;
                gr.Lang = "vi";
                data.GroupLibraries.InsertOnSubmit(gr);
                data.SubmitChanges();
                return RedirectToAction("GroupLibraryIndex");
            }
            else
            {
                return Redirect("/Admins/admins");
            }
        }
        #endregion
        #region[GroupLibraryEdit]
        public ActionResult GroupLibraryEdit(int id)
        {
            var Edit = data.GroupLibraries.First(m => m.Id == id);
            return View(Edit);
        }
        #endregion
        #region[GroupLibraryEdit]
        [HttpPost]
        public ActionResult GroupLibraryEdit(int id,FormCollection collection)
        {
            if (Request.Cookies["Username"] != null)
            {
                var gr = data.GroupLibraries.First(m => m.Id == id);
                gr.Name = collection["Name"];
                gr.Tag = StringClass.NameToTag(collection["Name"]);
                gr.Image = collection["Image"];
                gr.Ord = Convert.ToInt32(collection["Ord"]);
                var Active = (collection["Actives"] == "false") ? 0 : 1;
                gr.Active = Active;
                data.SubmitChanges();
                return RedirectToAction("GroupLibraryIndex");
            }
            else
            {
                return Redirect("/Admins/admins");
            }
        }
        #endregion
        #region[GroupLibraryAddSub]
        public ActionResult GroupLibraryAddSub()
        {
            return View();
        }
        #endregion
        #region[GroupLibraryAddSub]
        [HttpPost]
        public ActionResult GroupLibraryAddSub(string Level, FormCollection collection, GroupLibrary gr)
        {
            if (Request.Cookies["Username"] != null)
            {
                gr.Name = collection["Name"];
                gr.Tag = StringClass.NameToTag(collection["Name"]);
                gr.Image = collection["Image"];
                gr.Ord = Convert.ToInt32(collection["Ord"]);
                var Active = (collection["Actives"] == "false") ? 0 : 1;
                gr.Active = Active;
                gr.Level = Level + "00000";
                gr.Lang = "vi";
                data.GroupLibraries.InsertOnSubmit(gr);
                data.SubmitChanges();
                return RedirectToAction("GroupLibraryIndex");
            }
            else
            {
                return Redirect("/Admins/admins");
            }
        }
        #endregion
        #region[GroupLibraryActive]
        public ActionResult GroupLibraryActive(int id)
        {
            if (Request.Cookies["Username"] != null)
            {
                var act = data.GroupLibraries.First(m => m.Id == id);
                if (act.Active == 1)
                {
                    act.Active = 0;
                }
                else
                {
                    act.Active = 1;
                }
                data.SubmitChanges();
                return RedirectToAction("GroupLibraryIndex");
            }
            else
            {
                return Redirect("/Admins/admins");
            }
        }
        #endregion
        #region[GroupLibraryDelete]
        public ActionResult GroupLibraryDelete(int id)
        {
            if (Request.Cookies["Username"] != null)
            {
                var del = data.GroupLibraries.First(m => m.Id == id);
                data.GroupLibraries.DeleteOnSubmit(del);
                data.SubmitChanges();
                return RedirectToAction("GroupLibraryIndex");
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
                            var Del = (from emp in data.GroupLibraries where emp.Id == id select emp).SingleOrDefault();
                            data.GroupLibraries.DeleteOnSubmit(Del);
                            str += id.ToString() + ",";
                            data.SubmitChanges();
                        }
                    }
                }
                return RedirectToAction("GroupLibraryIndex");
            }
            else
            {
                return Redirect("/Admins/admins");
            }
        }
        #endregion
    }
}
