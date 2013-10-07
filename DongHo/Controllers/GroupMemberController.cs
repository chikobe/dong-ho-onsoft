using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DongHo.Models;

namespace DongHo.Models
{
    public class GroupMemberController : Controller
    {
        //
        // GET: /GroupMember/
        DataDataContext data = new DataDataContext();
        #region[GroupMemberIndex]
        public ActionResult GroupMemberIndex()
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
            var all = data.GroupMembers.ToList();
            var pages = all.Skip(curpage * pagesize).Take(pagesize).ToList();
            //var pages = data.sp_GroupMember_Phantrang(page, productize, "", "").ToList();
            var url = Request.Path;
            numOfNews = all.Count;
            ViewBag.Pager = DongHo.Models.Phantrang.PhanTrang(25, curpage, numOfNews, url);
            return View(pages);
        }
        #endregion
        #region[GroupMemberCreate]
        public ActionResult GroupMemberCreate()
        {
            return View();
        }
        #endregion
        #region[GroupMemberCreate]
        [HttpPost]
        public ActionResult GroupMemberCreate(FormCollection collect, GroupMember grMem)
        {
            if (Request.Cookies["Username"] != null)
            {
                grMem.Name = collect["Name"];
                var Active = (collect["Actives"] == "false") ? 0 : 1;
                grMem.Active = Active;
                data.GroupMembers.InsertOnSubmit(grMem);
                data.SubmitChanges();
                return RedirectToAction("GroupMemberIndex");
            }
            else
            {
                return Redirect("/Admins/admins");
            }
        }
        #endregion
        #region[GroupMemberEdit]
        public ActionResult GroupMemberEdit(int id)
        {
            var Edit = data.GroupMembers.First(m => m.Id == id);
            return View(Edit);
        }
        #endregion
        #region[GroupMemberEdit]
        [HttpPost]
        public ActionResult GroupMemberEdit(int id, FormCollection collect)
        {
            if (Request.Cookies["Username"] != null)
            {
                var grMem = data.GroupMembers.First(m => m.Id == id);
                grMem.Name = collect["Name"];
                var Active = (collect["Actives"] == "false") ? 0 : 1;
                grMem.Active = Active;
                data.SubmitChanges();
                return RedirectToAction("GroupMemberIndex");
            }
            else
            {
                return Redirect("/Admins/admins");
            }
        }
        #endregion
        #region[GroupMemberActive]
        public ActionResult GroupMemberActive(int id)
        {
            if (Request.Cookies["Username"] != null)
            {
                var act = data.GroupMembers.First(m => m.Id == id);
                act.Active = (act.Active == 1) ? 0 : 1;
                data.SubmitChanges();
                return RedirectToAction("GroupMemberIndex");
            }
            else
            {
                return Redirect("/Admins/admins");
            }
        }
        #endregion
        #region[GroupMemberDelete]
        public ActionResult GroupMemberDelete(int id)
        {
            if (Request.Cookies["Username"] != null)
            {
                var del = data.GroupMembers.First(m => m.Id == id);
                data.GroupMembers.DeleteOnSubmit(del);
                data.SubmitChanges();
                return RedirectToAction("GroupMemberIndex");
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
                            var Del = (from emp in data.GroupMembers where emp.Id == id select emp).SingleOrDefault();
                            data.GroupMembers.DeleteOnSubmit(Del);
                            data.SubmitChanges();
                        }
                    }
                }
                return RedirectToAction("GroupMemberIndex");
            }
            else
            {
                return Redirect("/Admins/admins");
            }
        }
        #endregion
    }
}
