using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DongHo.Models;

namespace DongHo.Controllers
{
    public class GroupSupportController : Controller
    {
        //
        // GET: /GroupSupport/
        DataDataContext data = new DataDataContext();
        #region[GroupSupportIndex]
        public ActionResult GroupSupportIndex()
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
            var all = data.GroupSupports.ToList();
            var pages = all.Skip(curpage * pagesize).Take(pagesize).ToList();
            var url = Request.Path;
            numOfNews = all.Count;
            ViewBag.Pager = DongHo.Models.Phantrang.PhanTrang(25, curpage, numOfNews, url);
            return View(pages);
        }
        #endregion
        #region[GroupSupportCreate]
        public ActionResult GroupSupportCreate()
        {
            return View();
        }
        #endregion
        #region[GroupSupportCreate]
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult GroupSupportCreate(FormCollection collection, GroupSupport groupsp)
        {
            if (Request.Cookies["Username"] != null)
            {
                var Name = collection["Name"];
                var Ord = collection["Ord"];
                var Active = (collection["Actives"] == "false") ? 0 : 1;
                groupsp.Name = Name;
                groupsp.Ord = Convert.ToInt32(Ord);
                groupsp.Active = Active;
                data.GroupSupports.InsertOnSubmit(groupsp);
                data.SubmitChanges();
                return RedirectToAction("GroupSupportIndex");
            }
            else
            {
                return Redirect("/Admins/admins");
            }
        }
        #endregion
        #region[GroupSupportEdit]
        public ActionResult GroupSupportEdit(int id)
        {
            var Edit = data.GroupSupports.First(m => m.Id == id);
            return View(Edit);
        }
        #endregion
        #region[GroupSupportEdit]
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult GroupSupportEdit(int id, FormCollection collection)
        {
            if (Request.Cookies["Username"] != null)
            {
                var groupSP = data.GroupSupports.First(model => model.Id == id);
                var Name = collection["Name"];
                var Ord = collection["Ord"];
                var Active = (collection["Actives"] == "false") ? 0 : 1;
                groupSP.Name = Name;
                groupSP.Ord = Convert.ToInt32(Ord);
                groupSP.Active = Active;
                data.SubmitChanges();
                return RedirectToAction("GroupSupportIndex");
            }
            else
            {
                return Redirect("/Admins/admins");
            }
        }
        #endregion
        #region[GroupSupportDelete]
        public ActionResult GroupSupportDelete(int id)
        {
            if (Request.Cookies["Username"] != null)
            {
                var del = (from news in data.GroupSupports where news.Id == id select news).Single();
                data.GroupSupports.DeleteOnSubmit(del);
                data.SubmitChanges();
                return RedirectToAction("GroupSupportIndex");
            }
            else
            {
                return Redirect("/Admins/admins");
            }
        }
        #endregion
        #region[GroupSupportActive]
        public ActionResult GroupSupportActive(int id)
        {
            if (Request.Cookies["Username"] != null)
            {
                var act = (from news in data.GroupSupports where news.Id == id select news).Single();
                if (act.Active == 1)
                {
                    act.Active = 0;
                }
                else { act.Active = 1; }
                data.SubmitChanges();
                return RedirectToAction("GroupSupportIndex");
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
                            var Del = (from emp in data.GroupSupports where emp.Id == id select emp).SingleOrDefault();
                            data.GroupSupports.DeleteOnSubmit(Del);
                            data.SubmitChanges();
                        }
                    }
                }
                return RedirectToAction("GroupSupportIndex");
            }
            else
            {
                return Redirect("/Admins/admins");
            }
        }
        #endregion
    }
}
