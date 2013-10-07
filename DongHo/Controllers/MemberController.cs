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
            string page = "1";//so phan trang hien tai
            var pagesize = 25;//so ban ghi tren 1 trang
            var numOfNews = 0;//tong so ban ghi co duoc truoc khi phan trang
            int curpage = 0; // trang hien tai dung cho phan trang
            if (Request["page"] != null)
            {
                page = Request["page"];
                curpage = Convert.ToInt32(page) - 1;
            }
            var all = data.Members.ToList();
            var pages = all.Skip(curpage * pagesize).Take(pagesize).ToList();
            //var pages = data.sp_Member_Phantrang(page, pagesize, "", "").ToList();
            var url = Request.Path;
            numOfNews = all.Count;
            ViewBag.Pager = DongHo.Models.Phantrang.PhanTrang(pagesize, curpage, numOfNews, url);
            return View(pages);
        }
        #endregion
        #region[MemberCreate]
        public ActionResult MemberCreate()
        {
            ViewBag.GrMember = new SelectList(data.GroupMembers, "Id", "Name");
            return View();
        }
        #endregion
        #region[MemberCreate]
        [HttpPost]
        public ActionResult MemberCreate(FormCollection collect, Member mem)
        {
            if (Request.Cookies["Username"] != null)
            {
                if (Request.Cookies["Username"]["Role"] == "1")
                {
                    mem.Name = collect["Name"];
                    mem.Email = collect["Email"];
                    mem.Username = collect["Username"];
                    mem.GroupMemberId = Convert.ToInt32(collect["GrMember"]);
                    mem.Password = StringClass.Encrypt(collect["Password"]);
                    var Active = (collect["Actives"] == "false") ? 0 : 1;
                    mem.Role = 0;
                    mem.Active = Active;
                    mem.DateCreated = DateTime.Now;
                    data.Members.InsertOnSubmit(mem);
                    data.SubmitChanges();
                }
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
            var Edit = data.Members.First(m => m.Id == id);
            ViewBag.GrMember = new SelectList(data.GroupMembers, "Id", "Name", Edit.GroupMemberId);
            return View(Edit);
        }
        #endregion
        #region[MemberEdit]
        [HttpPost]
        public ActionResult MemberEdit(FormCollection collect, int id)
        {
            if (Request.Cookies["Username"] != null)
            {
                if (Request.Cookies["Username"]["Role"] == "1")
                {
                    var mem = data.Members.First(m => m.Id == id);
                    mem.Name = collect["Name"];
                    mem.Email = collect["Email"];
                    mem.Username = collect["Username"];
                    mem.GroupMemberId = Convert.ToInt32(collect["GrMember"]);
                    mem.Password = StringClass.Encrypt(collect["Password"]);
                    mem.DateModified = DateTime.Now;
                    var role = (collect["Roles"] == "false") ? 0 : 1;
                    var Active = (collect["Actives"] == "false") ? 0 : 1;
                    mem.Active = Active;
                    mem.Role = role;
                    data.SubmitChanges();
                }
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
            if (Request.Cookies["Username"] != null && Request.Cookies["Username"]["Role"] == "1")
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
            if (Request.Cookies["Username"] != null && Request.Cookies["Username"]["Role"] == "1")
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
