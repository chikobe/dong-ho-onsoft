using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DongHo.Models;

namespace DongHo.Controllers
{
    public class LibraryController : Controller
    {
        //
        // GET: /Library/
        DataDataContext data = new DataDataContext();
        #region[LibraryIndex]
        public ActionResult LibraryIndex()
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
            var all = data.Libraries.ToList();
            var pages = all.Skip(curpage * pagesize).Take(pagesize).ToList();
            //var pages = data.sp_Library_Phantrang(page, pagesize, "", "").ToList();
            var url = Request.Path;
            numOfNews = all.Count;
            ViewBag.Pager = DongHo.Models.Phantrang.PhanTrang(pagesize, curpage, numOfNews, url);
            return View(pages);
        }
        #endregion
        #region[LibraryCreate]
        public ActionResult LibraryCreate()
        {
            ViewBag.GrLibrary = new SelectList(showlistGroupLibrary(), "value", "text");
            return View();
        }
        #endregion
        #region[LibraryCreate]
        [HttpPost]
        public ActionResult LibraryCreate(FormCollection collec, Library lib)
        {
            if (Request.Cookies["Username"] != null)
            {
                lib.Name = collec["Name"];
                lib.Tag = StringClass.NameToTag(collec["Name"]);
                lib.Image = collec["Image"];
                lib.Info = collec["Info"];
                lib.File = collec["File"];
                lib.GroupLibraryId = Convert.ToInt32(collec["GrLibrary"]);
                lib.Priority = (collec["Priorities"] == "false") ? 0 : 1;
                lib.Active = (collec["Actives"] == "false") ? 0 : 1;
                lib.Lang = "vi";
                data.Libraries.InsertOnSubmit(lib);
                data.SubmitChanges();
                return RedirectToAction("LibraryIndex");
            }
            else
            {
                return Redirect("/Admins/admins");
            }
        }
        #endregion
        #region[LibraryEdit]
        public ActionResult LibraryEdit(int id)
        {
            var Edit = data.Libraries.First(m => m.Id == id);
            ViewBag.GrLibrary = new SelectList(showlistGroupLibrary(), "value", "text", Edit.GroupLibraryId);
            return View(Edit);
        }
        #endregion
        #region[LibraryEdit]
        [HttpPost]
        public ActionResult LibraryEdit(FormCollection collec, int id)
        {
            if (Request.Cookies["Username"] != null)
            {
                var lib = data.Libraries.First(m => m.Id == id);
                lib.Name = collec["Name"];
                lib.Tag = StringClass.NameToTag(collec["Name"]);
                lib.Image = collec["Image"];
                lib.Info = collec["Info"];
                lib.File = collec["File"];
                lib.GroupLibraryId = Convert.ToInt32(collec["GrLibrary"]);
                lib.Priority = (collec["Priorities"] == "false") ? 0 : 1;
                lib.Active = (collec["Actives"] == "false") ? 0 : 1;
                data.SubmitChanges();
                return RedirectToAction("LibraryIndex");
            }
            else
            {
                return Redirect("/Admins/admins");
            }
        }
        #endregion
        #region[LibraryActive]
        public ActionResult LibraryActive(int id)
        {
            if (Request.Cookies["Username"] != null)
            {
                var act = data.Libraries.First(m => m.Id == id);
                if (act.Active == 1)
                {
                    act.Active = 0;
                }
                else
                {
                    act.Active = 1;
                }
                data.SubmitChanges();
                return RedirectToAction("LibraryIndex");
            }
            else
            {
                return Redirect("/Admins/admins");
            }
        }
        #endregion
        #region[LibraryDelete]
        public ActionResult LibraryDelete(int id)
        {
            if (Request.Cookies["Username"] != null)
            {
                var del = data.Libraries.First(m => m.Id == id);
                data.Libraries.DeleteOnSubmit(del);
                data.SubmitChanges();
                return RedirectToAction("LibraryIndex");
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
                            var Del = (from emp in data.Libraries where emp.Id == id select emp).SingleOrDefault();
                            data.Libraries.DeleteOnSubmit(Del);
                            str += id.ToString() + ",";
                            data.SubmitChanges();
                        }
                    }
                }
                return RedirectToAction("LibraryIndex");
            }
            else
            {
                return Redirect("/Admins/admins");
            }
        }
        #endregion
        #region[showlistGroupLibrary]
        public static List<DDL> showlistGroupLibrary()
        {
            DataDataContext data = new DataDataContext();
            var gr = data.GroupLibraries.OrderBy(m=>m.Level).ToList();
            List<DDL> list = new List<DDL>();
            if (gr.Count > 0)
            {
                for (int i = 0; i < gr.Count; i++)
                {
                    list.Add(new DDL { value = gr[i].Id.ToString(), text = StringClass.ShowNameLevel(gr[i].Name, gr[i].Level) });
                }
            }
            return list;
        }
        #endregion
        #region[DDL]
        public class DDL
        {
            private string _value;
            private string _text;

            public string value { get { return _value; } set { _value = value; } }
            public string text { get { return _text; } set { _text = value; } }

            public DDL()
            {
                this._value = "";
                this._text = "";
            }

            public DDL(string klk, string name)
            {
                this._value = klk;
                this._text = name;
            }
        }
        #endregion
    }
}
