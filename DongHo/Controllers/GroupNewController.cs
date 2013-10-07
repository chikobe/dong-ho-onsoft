using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DongHo.Models;

namespace DongHo.Controllers
{
    public class GroupNewController : Controller
    {
        //
        // GET: /GroupNew/

        DataDataContext data = new DataDataContext();
        #region[GroupNewIndex]
        public ActionResult GroupNewIndex()
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
            var all = data.GroupNews.ToList();
            var pages = all.Skip(curpage * pagesize).Take(pagesize).ToList();
            //var pages = data.sp_GroupNews_Phantrang(page, productize, "", "").ToList();
            var url = Request.Path;
            numOfNews = all.Count;
            ViewBag.Pager = DongHo.Models.Phantrang.PhanTrang(pagesize, curpage, numOfNews, url);
            return View(pages);
        }
        #endregion
        #region[GroupNewCreate]
        public ActionResult GroupNewCreate()
        {
            return View();
        }
        #endregion
        #region[GroupNewCreate]
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult GroupNewCreate(FormCollection collection, GroupNew group)
        {
            if (Request.Cookies["Username"] != null)
            {
                var Name = collection["Name"];
                var Title = collection["Title"];
                var Description = collection["Description"];
                var Keyword = collection["Keyword"];
                var Ord = collection["Ord"];
                var Index = (collection["Indexs"] == "false") ? 0 : 1;
                var Active = (collection["Actives"] == "false") ? 0 : 1;
                var Priority = (collection["Priority"] == "false") ? 0 : 1;
                group.Tag = StringClass.NameToTag(Name);
                group.Lang = "vi";
                group.Name = Name;
                group.Title = Title;
                group.Description = Description;
                group.Keyword = Keyword;
                group.Priority = Priority;
                group.Ord = Convert.ToInt32(Ord);
                group.Index = Index;
                group.Active = Active;
                data.GroupNews.InsertOnSubmit(group);
                data.SubmitChanges();
                return RedirectToAction("GroupNewIndex");
            }
            else
            {
                return Redirect("/Admins/admins");
            }
        }
        #endregion
        #region[GroupNewEdit]
        public ActionResult GroupNewEdit(int id)
        {
            var Edit = data.GroupNews.First(m => m.Id == id);
            return View(Edit);
        }
        #endregion
        #region[GroupNewEdit]
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult GroupNewEdit(int id, FormCollection collection)
        {
            if (Request.Cookies["Username"] != null)
            {
                var group = data.GroupNews.First(model => model.Id == id);
                var Name = collection["Name"];
                var Title = collection["Title"];
                var Description = collection["Description"];
                var Keyword = collection["Keyword"];
                var Ord = collection["Ord"];
                var Index = (collection["Indexs"] == "false") ? 0 : 1;
                var Active = (collection["Actives"] == "false") ? 0 : 1;
                var Priority = (collection["Priority"] == "false") ? 0 : 1;
                group.Index = Convert.ToInt32(Index);
                group.Active = Convert.ToInt32(Active);
                group.Name = Name;
                group.Title = Title;
                group.Description = Description;
                group.Keyword = Keyword;
                group.Ord = Convert.ToInt32(Ord);
                group.Priority = Priority;
                data.SubmitChanges();
                return RedirectToAction("GroupNewIndex");
            }
            else
            {
                return Redirect("/Admins/admins");
            }
        }
        #endregion
        #region[GroupNewDelete]
        public ActionResult GroupNewDelete(int id)
        {
            if (Request.Cookies["Username"] != null)
            {
                var del = (from groupN in data.GroupNews where groupN.Id == id select groupN).Single();
                data.GroupNews.DeleteOnSubmit(del);
                data.SubmitChanges();
                return RedirectToAction("GroupNewIndex");
            }
            else
            {
                return Redirect("/Admins/admins");
            }
        }
        #endregion
        #region[GroupNewActive]
        public ActionResult GroupNewActive(int id)
        {
            if (Request.Cookies["Username"] != null)
            {
                var act = (from groupN in data.GroupNews where groupN.Id == id select groupN).Single();
                if (act.Active == 1)
                {
                    act.Active = 0;
                }
                else { act.Active = 1; }
                data.SubmitChanges();
                return RedirectToAction("GroupNewIndex");
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
                            var Del = (from emp in data.GroupNews where emp.Id == id select emp).SingleOrDefault();
                            data.GroupNews.DeleteOnSubmit(Del);
                            data.SubmitChanges();
                        }
                    }
                }
                return RedirectToAction("GroupNewIndex");
            }
            else
            {
                return Redirect("/Admins/admins");
            }
        }
        #endregion
    }
}
