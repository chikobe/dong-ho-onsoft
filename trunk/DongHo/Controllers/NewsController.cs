using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DongHo.Models;

namespace DongHo.Controllers
{
    public class NewsController : Controller
    {
        //
        // GET: /News/
        DataDataContext data = new DataDataContext();
        #region[NewsIndex]
        public ActionResult NewsIndex()
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
            var all = data.News.ToList();
            var pages = all.Skip(curpage * pagesize).Take(pagesize).ToList();
            //var pages = data.sp_News_Phantrang(page, pagesize, "", "").ToList();
            var url = Request.Path;
            numOfNews = all.Count;
            ViewBag.Pager = DongHo.Models.Phantrang.PhanTrang(25, curpage, numOfNews, url);
            return View(pages);
        }
        #endregion
        #region[NewsCreate]
        public ActionResult  NewsCreate()
        {
            var list = (from cat in data.GroupNews where cat.Level.Length == 5 select cat).ToList();
            for (int i = 0; i < list.Count; i++)
            {
                ViewBag.GroupNewsId = new SelectList(data.GroupNews, "Id", "Name");
            }
            return View();
        }
        #endregion
        #region[NewsCreate]
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult NewsCreate(FormCollection collection, New news)
        {
            if (Session["Username"] != null)
            {
                var Name = collection["Name"];
                var Image = collection["Image"];
                var Content = collection["Content"];
                var Detail = collection["Detail"];
                var Title = collection["Title"];
                var Description = collection["Description"];
                var Keyword = collection["Keyword"];
                var Priority = (collection["Priorities"] == "false") ? 0 : 1;
                var Index = (collection["Indexs"] == "false") ? 0 : 1;
                var Active = (collection["Actives"] == "false") ? 0 : 1;
                var GroupNewsId = collection["GroupNewsId"];
                news.GroupNewsId = Convert.ToInt32(GroupNewsId);
                news.Date = DateTime.Now;
                news.Tag = StringClass.NameToTag(Name);
                news.Lang = "vi";
                news.Name = Name;
                news.Image = Image;
                news.Content = Content;
                news.Detail = Detail;
                news.Title = Title;
                news.Description = Description;
                news.Keyword = Keyword;
                news.Priority = Priority;
                news.Index = Index;
                news.Active = Active;
                data.News.InsertOnSubmit(news);
                data.SubmitChanges();
                return RedirectToAction("NewsIndex");
            }
            else
            {
                return Redirect("/Admins/admins");
            }
        }
        #endregion
        #region[NewsEdit]
        public ActionResult NewsEdit(int id)
        {
            var list = (from cat in data.GroupNews where cat.Level.Length == 5 select cat).ToList();
            var Edit = data.News.First(m => m.Id == id);
            for (int i = 0; i < list.Count; i++)
            {
                ViewBag.GroupNewsId = new SelectList(list, "Id", "Name", Edit.GroupNewsId);
            }
            return View(Edit);
        }
        #endregion
        #region[NewsEdit]
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult NewsEdit(int id,FormCollection collection)
        {
            if (Session["Username"] != null)
            {
                var news = data.News.First(model => model.Id == id);
                var Name = collection["Name"];
                var Image = collection["Image"];
                var Content = collection["Content"];
                var Detail = collection["Detail"];
                var Title = collection["Title"];
                var Description = collection["Description"];
                var Keyword = collection["Keyword"];
                var Priority = (collection["Priorities"] == "false") ? 0 : 1;
                var Index = (collection["Indexs"] == "false") ? 0 : 1;
                var Active = (collection["Actives"] == "false") ? 0 : 1;
                var GroupNewsId = collection["GroupNewsId"];
                news.GroupNewsId = Convert.ToInt16(GroupNewsId);
                news.Tag = StringClass.NameToTag(Name);
                news.Lang = "vi";
                news.Name = Name;
                news.Image = Image;
                news.Content = Content;
                news.Detail = Detail;
                news.Title = Title;
                news.Description = Description;
                news.Keyword = Keyword;
                news.Priority = Priority;
                news.Index = Index;
                news.Active = Active;
                data.SubmitChanges();
                return RedirectToAction("NewsIndex");
            }
            else
            {
                return Redirect("/Admins/admins");
            }
        }
        #endregion
        #region[NewsDelete]
        public ActionResult NewsDelete(int id)
        {
            if (Session["Username"] != null)
            {
                var del = (from news in data.News where news.Id == id select news).Single();
                data.News.DeleteOnSubmit(del);
                data.SubmitChanges();
                return RedirectToAction("NewsIndex");
            }
            else
            {
                return Redirect("/Admins/admins");
            }
        }
        #endregion
        #region[NewsActive]
        public ActionResult NewsActive(int id)
        {
            if (Session["Username"] != null)
            {
                var act = (from news in data.News where news.Id == id select news).Single();
                if (act.Active == 1)
                {
                    act.Active = 0;
                }
                else { act.Active = 1; }
                data.SubmitChanges();
                return RedirectToAction("NewsIndex");
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
                            var Del = (from emp in data.News where emp.Id == id select emp).SingleOrDefault();
                            data.News.DeleteOnSubmit(Del);
                            data.SubmitChanges();
                        }
                    }
                }
                return RedirectToAction("NewsIndex");
            }
            else
            {
                return Redirect("/Admins/admins");
            }
        }
        #endregion
    }
}
