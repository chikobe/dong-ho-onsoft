using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DongHo.Models;

namespace DongHo.Controllers
{
    public class MemberController : Controller
    {
        //
        // GET: /Member/
        DataDataContext data = new DataDataContext();
        #region[MemberIndex]
        public ActionResult MemberIndex()
        {
            if (Session["Username"] != null)
            {
                string page = "1";//so phan trang hien tai
                var pagesize = "25";//so ban ghi tren 1 trang
                var numOfNews = 0;//tong so ban ghi co duoc truoc khi phan trang
                int curpage = 0; // trang hien tai dung cho phan trang
                if (Request["page"] != null)
                {
                    page = Request["page"];
                    curpage = Convert.ToInt32(page) - 1;
                }
                var all = data.Members.ToList();
                var pages = data.sp_Member_Phantrang(page, pagesize, "", "").ToList();
                var url = Request.Path;
                numOfNews = all.Count;
                ViewBag.Pager = DongHo.Models.Phantrang.PhanTrang(25, curpage, numOfNews, url);
                return View(pages);
            }
            else
            {
                return Redirect("/Admins/admins");
            }
        }
        #endregion
        #region[MemberCreate]
        public ActionResult MemberCreate()
        {
            if (Session["Username"] != null)
            {
                ViewBag.GrMember = new SelectList(data.GroupMembers, "Id", "Name");
                return View();
            }
            else
            {
                return Redirect("/Admins/admins");
            }
        }
        #endregion
        #region[MemberCreate]
        [HttpPost]
        public ActionResult MemberCreate(FormCollection collect, Member mem)
        {
            if (Session["Username"] != null)
            {
                mem.Name = collect["Name"];
                mem.Email = collect["Email"];
                mem.Username = collect["Username"];
                mem.GroupMemberId = Convert.ToInt32(collect["GrMember"]);
                mem.Password = StringClass.Encrypt(collect["Password"]);
                var Active = (collect["Actives"] == "false") ? 0 : 1;
                data.Members.InsertOnSubmit(mem);
                data.SubmitChanges();
                return RedirectToAction("MemberIndex");
            }
            else
            {
                return Redirect("/Admins/admins");
            }
        }
        #endregion
        #region[MemberEdit]
        public ActionResult MemberEdit(int id)
        {
            if (Session["Username"] != null)
            {
                var Edit = data.Members.First(m => m.Id == id);
                ViewBag.GrMember = new SelectList(data.GroupMembers, "Id", "Name", Edit.GroupMemberId);
                return View(Edit);
            }
            else
            {
                return Redirect("/Admins/admins");
            }
        }
        #endregion
        #region[MemberEdit]
        [HttpPost]
        public ActionResult MemberEdit(FormCollection collect, int id)
        {
            if (Session["Username"] != null)
            {
                var mem = data.Members.First(m => m.Id == id);
                mem.Name = collect["Name"];
                mem.Email = collect["Email"];
                mem.Username = collect["Username"];
                mem.GroupMemberId = Convert.ToInt32(collect["GrMember"]);
                mem.Password = StringClass.Encrypt(collect["Password"]);
                var Active = (collect["Actives"] == "false") ? 0 : 1;
                data.SubmitChanges();
                return RedirectToAction("MemberIndex");
            }
            else
            {
                return Redirect("/Admins/admins");
            }
        }
        #endregion
        #region[MemberActive]
        public ActionResult MemberActive(int id)
        {
            if (Session["Username"] != null)
            {
                var act = data.Members.First(m => m.Id == id);
                act.Active = (act.Active == 1) ? 0 : 1;
                data.SubmitChanges();
                return RedirectToAction("MemberIndex");
            }
            else
            {
                return Redirect("/Admins/admins");
            }
        }
        #endregion
        #region[MemberDelete]
        public ActionResult MemberDelete(int id)
        {
            if (Session["Username"] != null)
            {
                var del = data.Members.First(m => m.Id == id);
                data.Members.DeleteOnSubmit(del);
                data.SubmitChanges();
                return RedirectToAction("MemberIndex");
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
                            var Del = (from emp in data.Members where emp.Id == id select emp).SingleOrDefault();
                            data.Members.DeleteOnSubmit(Del);
                            data.SubmitChanges();
                        }
                    }
                }
                return RedirectToAction("MemberIndex");
            }
            else
            {
                return Redirect("/Admins/admins");
            }
        }
        #endregion
    }
}
