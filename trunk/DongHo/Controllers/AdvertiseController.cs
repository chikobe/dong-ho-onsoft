using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DongHo.Models;

namespace DongHo.Controllers
{
    public class AdvertiseController : Controller
    {
        //
        // GET: /Advertise/
        DataDataContext data = new DataDataContext();
        #region[AdvertiseIndex]
        public ActionResult AdvertiseIndex(int? position=1)
        {
            if (Session["Username"] != null)
            {
                ViewBag.Position = new SelectList(StringClass.DDLTypeAdvertise(), "value", "text");
                string page = "1";//so phan trang hien tai
                var pagesize = "25";//so ban ghi tren 1 trang
                var numOfNews = 0;//tong so ban ghi co duoc truoc khi phan trang
                int curpage = 0; // trang hien tai dung cho phan trang
                if (Request["page"] != null)
                {
                    page = Request["page"];
                    curpage = Convert.ToInt32(page) - 1;
                }
                var all = new List<DongHo.Models.Advertise>();
                all = (from adv in data.Advertises where adv.Position == position select adv).ToList();
                var pages = data.sp_Advertise_Phantrang(page, pagesize, "Position=" + position + "", "Id desc").ToList();
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
        #region[AdvertiseCreate]
        public ActionResult AdvertiseCreate()
        {
            if (Session["Username"] != null)
            {
                ViewBag.Position = new SelectList(StringClass.DDLTypeAdvertise(), "value", "text");
                return View();
            }
            else
            {
                return Redirect("/Admins/admins");
            }
        }
        #endregion
        #region[AdvertiseCreate]
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult AdvertiseCreate(FormCollection collection, Advertise adv)
        {
            if (Session["Username"] != null)
            {
            var Name = collection["Name"];
            var Image = collection["Image"];
            var Width = collection["Width"];
            var Height = collection["Height"];
            var Link = collection["Link"];
            var Content = collection["Content"];
            var Position = collection["Position"];
            var Click = collection["Click"];
            var Ord = collection["Ord"];
            var Active = collection["Actives"];
            var Target = collection["Target"];
            adv.Name = Name;
            adv.Image = Image;
            adv.Width = Convert.ToInt32(Width);
            adv.Height = Convert.ToInt32(Height);
            adv.Link = Link;
            adv.Content = Content;
            adv.Position = Convert.ToInt16(Position);
            adv.Click = Convert.ToInt32(Click);
            adv.Ord = Convert.ToInt32(Ord);
            adv.Active = (Active.Equals("False")) ? false : true;
            data.Advertises.InsertOnSubmit(adv);
            data.SubmitChanges();
            return RedirectToAction("AdvertiseIndex");
            }
            else
            {
                return Redirect("/Admins/admins");
            }
        }
        #endregion
        #region[AdvertiseEdit]
        public ActionResult AdvertiseEdit(int id)
        {
            if (Session["Username"] != null)
            {
                var Edit = data.Advertises.Where(a => a.Id == id).Single();
                ViewBag.Position = new SelectList(StringClass.DDLTypeAdvertise(), "value", "text", Edit.Position);
                return View(Edit);
            }
            else
            {
                return Redirect("/Admins/admins");
            }
        }
        #endregion
        #region[AdvertiseEdit]
        [HttpPost]
        [ValidateInput(true)]
        public ActionResult AdvertiseEdit(int id, FormCollection collection)
        {
            if (Session["Username"] != null)
            {
                var adv = data.Advertises.First(a => a.Id == id);
                var Name = collection["Name"];
                var Image = collection["Image"];
                var Width = collection["Width"];
                var Height = collection["Height"];
                var Link = collection["Link"];
                var Content = collection["Content"];
                var Position = collection["Position"];
                var Click = collection["Click"];
                var Ord = collection["Ord"];
                var Active = collection["Actives"];
                var Target = collection["Target"];
                adv.Name = Name;
                adv.Image = Image;
                adv.Width = Convert.ToInt32(Width);
                adv.Height = Convert.ToInt32(Height);
                adv.Link = Link;
                adv.Content = Content;
                adv.Position = Convert.ToInt16(Position);
                adv.Click = Convert.ToInt32(Click);
                adv.Ord = Convert.ToInt32(Ord);
                adv.Active = (Active.Equals("False")) ? false : true;
                data.SubmitChanges();
                return RedirectToAction("AdvertiseIndex");
            }
            else
            {
                return Redirect("/Admins/admins");
            }
        }
        #endregion
        #region[AdvertiseDelete]
        public ActionResult AdvertiseDelete(int id)
        {
            if (Session["Username"] != null)
            {
                var del = (from adv in data.Advertises where adv.Id == id select adv).Single();
                data.Advertises.DeleteOnSubmit(del);
                data.SubmitChanges();
                return RedirectToAction("AdvertiseIndex");
            }
            else
            {
                return Redirect("/Admins/admins");
            }
        }
        #endregion
        #region[AdvertiseActive]
        public ActionResult AdvertiseActive(int id)
        {
            if (Session["Username"] != null)
            {
                var act = (from a in data.Advertises where a.Id == id select a).Single();
                var obj = data.Advertises.First(b => b.Id == id);
                if (act.Active == true)
                {
                    obj.Active = false;
                }
                else
                {
                    obj.Active = true;
                }
                UpdateModel(obj);
                data.SubmitChanges();
                return RedirectToAction("AdvertiseIndex");
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
                            var Del = (from emp in data.Advertises where emp.Id == id select emp).SingleOrDefault();
                            data.Advertises.DeleteOnSubmit(Del);
                            str += id.ToString() + ",";
                            data.SubmitChanges();
                        }
                    }
                }
                return RedirectToAction("AdvertiseIndex");
            }
            else
            {
                return Redirect("/Admins/admins");
            }
        }
        #endregion
    }
}
