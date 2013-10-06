using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DongHo.Models;

namespace DongHo.Controllers
{
    public class SupportController : Controller
    {
        //
        // GET: /Support/

        DataDataContext data = new DataDataContext();
        #region[SupportIndex]
        public ActionResult SupportIndex()
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
            var all = data.Supports.ToList();
            var pages = all.Skip(curpage * pagesize).Take(pagesize).ToList();
            //var product = data.sp_Support_Phantrang(page, productize, "", "").ToList();
            var url = Request.Path;
            numOfNews = all.Count;
            ViewBag.Pager = DongHo.Models.Phantrang.PhanTrang(25, curpage, numOfNews, url);
            return View(pages);
        }
        #endregion
        #region[SupportCreate]
        public ActionResult SupportCreate()
        {
            ViewBag.GroupSupport = new SelectList(data.GroupSupports, "Id", "Name");
            ViewBag.Type = new SelectList(TypeSupport(), "value", "text");
            return View();
        }
        #endregion
        #region[SupportCreate]
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult SupportCreate(FormCollection collection, Support supports)
        {
            if (Session["Username"] != null)
            {
                var Name = collection["Name"];
                var Tel = collection["Tel"];
                var Type = collection["Type"];
                var Nick = collection["Nick"];
                var Ord = collection["Ord"];
                var Location = collection["Location"];
                var Active = (collection["Actives"] == "false") ? 0 : 1;
                var GroupSupport = collection["GroupSupport"];
                supports.GroupSupportId = Convert.ToInt32(GroupSupport);
                supports.Lang = "vi";
                supports.Name = Name;
                supports.Tel = Tel;
                supports.Type = Convert.ToInt32(Type);
                supports.Nick = Nick;
                supports.Ord = Convert.ToInt32(Ord);
                supports.Active = Active;
                supports.Location = Convert.ToInt32(Location);
                data.Supports.InsertOnSubmit(supports);
                data.SubmitChanges();
                return RedirectToAction("SupportIndex");
            }
            else
            {
                return Redirect("/Admins/admins");
            }
        }
        #endregion
        #region[SupportEdit]
        public ActionResult SupportEdit(int id)
        {
            var Edit = data.Supports.First(m => m.Id == id);
            ViewBag.GroupSupport = new SelectList(data.GroupSupports, "Id", "Name", Edit.GroupSupportId);
            ViewBag.Type = new SelectList(TypeSupport(), "value", "text",Edit.Type);
            return View(Edit);
        }
        #endregion
        #region[SupportEdit]
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult SupportEdit(int id, FormCollection collection)
        {
            if (Session["Username"] != null)
            {
                var support = data.Supports.First(model => model.Id == id);
                var Name = collection["Name"];
                var Tel = collection["Tel"];
                var Type = collection["Type"];
                var Nick = collection["Nick"];
                var Ord = collection["Ord"];
                var Location = collection["Location"];
                var Active = (collection["Actives"] == "false") ? 0 : 1;
                support.Lang = "vi";
                support.Name = Name;
                support.Tel = Tel;
                support.Type = Convert.ToInt32(Type);
                support.Nick = Nick;
                support.Ord = Convert.ToInt32(Ord);
                support.Active = Convert.ToInt32(Active);
                support.Location = Convert.ToInt32(Location);
                data.SubmitChanges();
                return RedirectToAction("SupportIndex");
            }
            else
            {
                return Redirect("/Admins/admins");
            }
        }
        #endregion
        #region[SupportDelete]
        public ActionResult SupportDelete(int id)
        {
            if (Session["Username"] != null)
            {
                var del = (from support in data.Supports where support.Id == id select support).Single();
                data.Supports.DeleteOnSubmit(del);
                data.SubmitChanges();
                return RedirectToAction("SupportIndex");
            }
            else
            {
                return Redirect("/Admins/admins");
            }
        }
        #endregion
        #region[SupportActive]
        public ActionResult SupportActive(int id)
        {
            if (Session["Username"] != null)
            {
                var act = (from news in data.Supports where news.Id == id select news).Single();
                if (act.Active == 1)
                {
                    act.Active = 0;
                }
                else { act.Active = 1; }
                data.SubmitChanges();
                return RedirectToAction("SupportIndex");
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
                            var Del = (from emp in data.Supports where emp.Id == id select emp).SingleOrDefault();
                            data.Supports.DeleteOnSubmit(Del);
                            data.SubmitChanges();
                        }
                    }
                }
                return RedirectToAction("SupportIndex");
            }
            else
            {
                return Redirect("/Admins/admins");
            }
        }
        #endregion
        public class ddl
        {
            public string value { get; set; }
            public string text { get; set; }
        }
        public List<ddl> TypeSupport()
        {
            List<ddl> type = new List<ddl>();
            type.Add(new ddl { value = "0", text = "yahoo" });
            type.Add(new ddl { value = "1", text = "skype" });
            type.Add(new ddl { value = "2", text = "hotline" });
            return type;
        }
    }
}
