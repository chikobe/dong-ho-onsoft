﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DongHo.Models;

namespace DongHo.Controllers
{
    public class PricesController : Controller
    {
        //
        // GET: /Prices/
        DataDataContext data = new DataDataContext();
        #region[PricesIndex]
        public ActionResult PricesIndex()
        {
            var list = data.Prices.ToList();
            return View(list);
        }
        #endregion
        #region[PricesCreate]
        public ActionResult PricesCreate()
        {
            return View();
        }
        #endregion
        #region[PricesCreate]
        [HttpPost]
        public ActionResult PricesCreate(FormCollection collect, Price price)
        {
            if (Request.Cookies["Username"] != null)
            {
                price.Name = collect["Name"];
                price.PriceFrom = int.Parse(collect["PriceFrom"]);
                price.PriceTo = int.Parse(collect["PriceTo"]);
                price.Ord = int.Parse(collect["Ord"]);
                data.Prices.InsertOnSubmit(price);
                data.SubmitChanges();
                return RedirectToAction("PricesIndex");
            }
            else
            {
                return Redirect("/Admins/admins");
            }
        }
        #endregion
        #region[PricesEdit]
        public ActionResult PricesEdit(int id)
        {
            var edit = data.Prices.First(m => m.Id == id);
            return View(edit);
        }
        #endregion
        #region[PricesEdit]
        [HttpPost]
        public ActionResult PricesEdit(FormCollection collect, int id)
        {
            if (Request.Cookies["Username"] != null)
            {
                var price = data.Prices.First(m => m.Id == id);
                price.Name = collect["Name"];
                price.PriceFrom = int.Parse(collect["PriceFrom"]);
                price.PriceTo = int.Parse(collect["PriceTo"]);
                price.Ord = int.Parse(collect["Ord"]);
                data.SubmitChanges();
                return RedirectToAction("PricesIndex");
            }
            else
            {
                return Redirect("/Admins/admins");
            }
        }
        #endregion
        #region[PricesDelete]
        public ActionResult PricesDelete(int id)
        {
            if (Request.Cookies["Username"] != null)
            {
                var del = data.Prices.First(m => m.Id == id);
                data.Prices.DeleteOnSubmit(del);
                data.SubmitChanges();
                return RedirectToAction("PricesIndex");
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
                            var Del = (from del in data.Prices where del.Id == id select del).SingleOrDefault();
                            data.Prices.DeleteOnSubmit(Del);
                            data.SubmitChanges();
                        }
                    }
                }
                return RedirectToAction("ProductIndex");
            }
            else
            {
                return Redirect("/Admins/admins");
            }
        }
        #endregion
    }
}
