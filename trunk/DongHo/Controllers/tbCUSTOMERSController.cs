using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DongHo.Models;

namespace DongHo.Controllers
{
    public class tbCUSTOMERSController : Controller
    {
        //
        // GET: /tbCUSTOMERS/
        DataDataContext data = new DataDataContext();
        #region[tbCUSTOMERSIndex]
        public ActionResult tbCUSTOMERSIndex()
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
                var all = data.tbCUSTOMERs.ToList();
                var pages = data.sp_tbCUSTOMERS_Phantrang(page, pagesize, "", "").ToList();
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
        #region[tbCUSTOMERSEdit]
        public ActionResult tbCUSTOMERSEdit(int id)
        {
            if (Session["Username"] != null)
            {
                var Edit = data.tbCUSTOMERs.First(m => m.iusid == id);
                return View(Edit);
            }
            else
            {
                return Redirect("/Admins/admins");
            }
        }
        #endregion
        #region[tbCUSTOMERSEdit]
        [HttpPost]
        public ActionResult tbCUSTOMERSEdit(FormCollection collection, int id)
        {
            if (Session["Username"] != null)
            {
                var cus = data.tbCUSTOMERs.First(m => m.iusid == id);
                cus.vcusname = collection["vcusname"];
                cus.vaddress = collection["vaddress"];
                cus.vemail = collection["vemail"];
                cus.vprovince = collection["vprovince"];
                cus.vphone = collection["vphone"];
                cus.dbirthday = collection["dbirthday"];
                var istatus = (collection["istatus"] == "false") ? 0 : 1;
                cus.istatus = Convert.ToInt16(istatus);
                data.SubmitChanges();
                return RedirectToAction("tbCUSTOMERSIndex");
            }
            else
            {
                return Redirect("/Admins/admins");
            }
        }
        #endregion
        #region[tbCUSTOMERSDelete]
        public ActionResult tbCUSTOMERSDelete(int id)
        {
            if (Session["Username"] != null)
            {
                if (ModelState.IsValid)
                {
                    var del = data.tbCUSTOMERs.First(m => m.iusid == id);
                    data.tbCUSTOMERs.DeleteOnSubmit(del);
                    data.SubmitChanges();
                }
                return RedirectToAction("tbCUSTOMERSIndex");
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
                            var Del = (from emp in data.tbCUSTOMERs where emp.iusid == id select emp).SingleOrDefault();
                            data.tbCUSTOMERs.DeleteOnSubmit(Del);
                            str += id.ToString() + ",";
                            data.SubmitChanges();
                        }
                    }
                }
                return RedirectToAction("tbCUSTOMERSIndex");
            }
            else
            {
                return Redirect("/Admins/admins");
            }
        }
        #endregion
    }
}
