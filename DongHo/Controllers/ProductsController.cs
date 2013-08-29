using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DongHo.Models;
using System.IO;

namespace DongHo.Controllers
{
    public class ProductsController : Controller
    {
        //
        // GET: /Products/
        DataDataContext data = new DataDataContext();
        #region[ProductIndex]
        public ActionResult ProductIndex()
        {
            if (Session["Username"] != null)
            {
                string CatS = "";
                string BrandS = "";
                string NameS = "";
                string chuoi = "1=1";
                string page = "1";//so phan trang hien tai
                var pagesize = "25";//so ban ghi tren 1 trang
                var numOfNews = 0;//tong so ban ghi co duoc truoc khi phan trang
                int curpage = 0; // trang hien tai dung cho phan trang
                if (Request["page"] != null)
                {
                    page = Request["page"];
                    curpage = Convert.ToInt32(page) - 1;
                }
                if (Request["Cat"] != null) { CatS = Request["Cat"]; if (CatS != "") { chuoi += " and CatId = " + CatS + ""; ViewBag.CatS = CatS; } }
                if (Request["Brand"] != null) { BrandS = Request["Brand"]; if (BrandS != "") { chuoi += " and BrandId = " + BrandS + ""; ViewBag.BrandS = BrandS; } }
                if (Request["Name"] != null) { NameS = Request["Name"]; if (NameS != "") { chuoi += "and Name like N'%" + NameS + "%'"; } }
                var all = data.sp_Product_GetByTop("", chuoi, "").ToList();
                var adv = data.sp_Product_Phantrang(page, pagesize, chuoi, "Id desc").ToList();
                numOfNews = all.Count;
                string strUrl = Request.Url.PathAndQuery;
                int u = strUrl.IndexOf("&page=");
                if (u > 0)
                {
                    strUrl = strUrl.Substring(0, u);
                }
                if (chuoi == "1=1")
                {
                    ViewBag.Pager = DongHo.Models.Phantrang.PhanTrang(25, curpage, numOfNews, strUrl);
                }
                else
                {
                    ViewBag.Pager = DongHo.Models.PhantrangQuery.PhanTrangQuery(25, curpage, numOfNews, strUrl);
                }
                ViewBag.dropC = CatS;
                ViewBag.dropB = BrandS;
                return View(adv);
            }
            else
            {
                return Redirect("/Admins/admins");
            }
        }
        #endregion
        #region[Search]
        [ChildActionOnly]
        public ActionResult Search()
        {
            var cat = data.Categories.Where(n => n.Active == 1 && n.Level.Length == 5).ToList();
            for (int i = 0; i < cat.Count; i++)
            {
                ViewBag.Cat = new SelectList(cat, "Id", "Name");
            }
            var brand = data.Brands.ToList();
            for (int i = 0; i < brand.Count; i++)
            {
                ViewBag.Brand = new SelectList(brand, "Id", "Name");
            }
            return PartialView();
        }
        #endregion
        #region[Search]
        [HttpPost]
        public ActionResult Search(FormCollection collect)
        {
            string url = "";
            var Cat = collect["Cat"];
            var Brand = collect["Brand"];
            var Name = collect["Name"];
            if (Cat != "" && Brand == "" && Name == "")
            {
                url = "/Products/ProductIndex?Cat=" + Cat + "";
            }
            else if (Cat == "" && Brand != "" && Name == "")
            {
                url = "/Products/ProductIndex?Brand=" + Brand + "";
            }
            else if (Cat == "" && Brand == "" && Name != "")
            {
                url = "/Products/ProductIndex?Name=" + Name + "";
            }
            else if (Cat != "" && Brand != "" && Name == "")
            {
                url = "/Products/ProductIndex?Cat=" + Cat + "&Brand=" + Brand + "";
            }
            else if (Cat != "" && Brand == "" && Name != "")
            {
                url = "/Products/ProductIndex?Cat=" + Cat + "&Name=" + Name + "";
            }
            else if (Cat == "" && Brand != "" && Name != "")
            {
                url = "/Products/ProductIndex?Brand=" + Brand + "&Name=" + Name + "";
            }
            else if (Cat != "" && Brand != "" && Name != "")
            {
                url = "/Products/ProductIndex?Cat=" + Cat + "&Brand=" + Brand + "&Name=" + Name + "";
            }
            else
            {
                url = "/Products/ProductIndex";
            }
            return Redirect(url);
        }
        #endregion
        #region[ProductCreate]
        public ActionResult ProductCreate()
        {
            if (Session["Username"] != null)
            {
                var cat = data.Categories.Where(n => n.Active == 1 && n.Level.Length == 5).ToList();
                for (int i = 0; i < cat.Count; i++)
                {
                    ViewBag.Cat = new SelectList(cat, "Id", "Name");
                }
                var brand = data.Brands.ToList();
                for (int i = 0; i < brand.Count; i++)
                {
                    ViewBag.Brand = new SelectList(brand, "Id", "Name");
                }
                
                //string strColor = "";
                //var color = data.Colors.ToList();
                //strColor += "<div style=' border:1px solid #a0a0a0; padding:5px;'>";
                //strColor += "<div class='showHideChkColor'>Hiển thị</div>";
                //strColor += "<div id='divChkColor'>";
                //for (int i = 0; i < color.Count; i++)
                //{
                //    strColor += "<p style='width:10%; float:left;'>";
                //    strColor += "<input type='checkbox' value='" + color[i].Id + "' id='chk_" + i + "' name='chkColor'/>";
                //    strColor += "<span style='margin-left:3px;'>" + color[i].Name + "</span>";
                //    strColor += "</p>";
                //}
                //strColor += "<div class='clearfix'></div>";
                //strColor += "</div>";
                //strColor += "</div>";
                //ViewBag.Color = strColor;
                //string strSize = "";
                //var size = data.Sizes.ToList();
                //strSize += "<div style=' border:1px solid #a0a0a0; padding:5px;'>";
                //strSize += "<div class='showHideChkSize'>Hiển thị</div>";
                //strSize += "<div id='divChkSize'>";
                //for (int i = 0; i < size.Count; i++)
                //{
                //    strSize += "<p style='width:10%; float:left;'>";
                //    strSize += "<input type='checkbox' value='" + size[i].Id + "' id='chk_" + i + "' name='chkSize'/>";
                //    strSize += "<span style='margin-left:3px;'>" + size[i].Name + "</span>";
                //    strSize += "</p>";
                //}
                //strSize += "<div class='clearfix'></div>";
                //strSize += "</div>";
                //strSize += "</div>";
                //ViewBag.Size = strSize;
                return View();
            }
            else
            {
                return Redirect("/Admins/admins");
            }
        }
        #endregion
        #region[ProductCreate]
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult ProductCreate(FormCollection collection, Product pro)
        {
            if (Session["Username"] != null)
            {
                pro.Name = collection["Name"];
                pro.Tag = DongHo.Models.StringClass.NameToTag(collection["Name"]);
                pro.Content = collection["Content"];
                pro.Detail = collection["Detail"];
                pro.Image = collection["Image"];
                pro.Date = Convert.ToDateTime(DateTime.Now.ToString());
                pro.CatId = Convert.ToInt32(collection["Cat"]);
                pro.BrandId = Convert.ToInt32(collection["Brand"]);
                pro.CatL2 = Convert.ToInt32(collection["CatL2"]);
                pro.Keyword = collection["Keyword"];
                pro.Description = collection["Description"];
                pro.Title = collection["Title"];
                pro.Price = Convert.ToDouble(collection["Price"]);
                pro.PiceOld = (collection["PiceOld"] != "") ? collection["PiceOld"] : "0";
                var DateBegin = collection["DateBegin"];
                var DateEnd = collection["DateEnd"];
                ProPrice proPrice = new ProPrice();
                if (DateBegin == "" && DateEnd == "")
                {
                    pro.DateBegin = null;
                    pro.DateEnd = null;
                    proPrice.DateBegin = null;
                    proPrice.DateEnd = null;
                }
                else
                {
                    string dateInputBegin="";
                    string dateInputEnd = "";
                    DateTime myDateBegin;
                    DateTime myDateEnd;
                    string[] dateArrBegin = DateBegin.Split('/');
                    string[] dateArrEnd = DateEnd.Split('/');
                    if (dateArrBegin.Length == 3) dateInputBegin = dateArrBegin[2] + "-" + dateArrBegin[1] + "-" + dateArrBegin[0];
                    if (dateArrEnd.Length == 3) dateInputEnd = dateArrEnd[2] + "-" + dateArrEnd[1] + "-" + dateArrEnd[0];
                    if (DateTime.TryParse(dateInputBegin, out myDateBegin))pro.DateBegin = proPrice.DateBegin = myDateBegin;
                    if (DateTime.TryParse(dateInputEnd, out myDateEnd)) pro.DateEnd = proPrice.DateEnd = myDateEnd;
                }
                pro.Ord = Convert.ToInt32(collection["Ord"]);
                var priority = (collection["Prioritys"] == "false") ? 0 : 1;
                pro.Priority = Convert.ToInt16(priority);
                var index = (collection["Indexs"] == "false") ? 0 : 1;
                pro.Index = Convert.ToInt16(index);
                var active = (collection["Actives"] == "false") ? 0 : 1;
                pro.Active = active;
                pro.Codepro = (collection["Percent"] != "") ? int.Parse(collection["Percent"]) : 0;
                pro.View = 0;//so luot xem
                pro.Count = 0;//so luong don hang da dat nhung chua giao hang
                pro.SpTon = 0;//sp con ton trong kho
                data.Products.InsertOnSubmit(pro);
                data.SubmitChanges();
                var product = data.Products.OrderByDescending(m => m.Id).FirstOrDefault();
                if (collection["txtColor"] != "")
                {
                    var strColor = collection["txtColor"].Replace("(all),", " ").Split(',').Select(x => x).ToList();
                    for (int k = 0; k < strColor.Count; k++)
                    {
                        ProColor obj = new ProColor();
                        obj.ProId = product.Id;
                        obj.ColorId = int.Parse(strColor[k]);
                        data.ProColors.InsertOnSubmit(obj);
                        data.SubmitChanges();
                    }
                }
                if (collection["txtSize"] != "")
                {
                    var strSize = collection["txtSize"].Replace("(all),", " ").Split(',').Select(x => x).ToList();
                    for (int k = 0; k < strSize.Count; k++)
                    {
                        ProSize obj = new ProSize();
                        obj.ProId = product.Id;
                        obj.SizeId = int.Parse(strSize[k]);
                        data.ProSizes.InsertOnSubmit(obj);
                        data.SubmitChanges();
                    }
                }
                proPrice.ProId = product.Id;
                proPrice.GiaBanLe = collection["Price"];
                proPrice.PricePromotion = collection["PiceOld"];
                proPrice.Date = DateTime.Now;
                data.ProPrices.InsertOnSubmit(proPrice);
                data.SubmitChanges();
                return RedirectToAction("ProductIndex");
            }
            else
            {
                return Redirect("/Admins/admins");
            }
        }
        #endregion
        #region[ProductEdit]
        public ActionResult ProductEdit(int id)
        {
            if (Session["Username"] != null)
            {
                var Edit = data.Products.First(m => m.Id == id);
                var cat = data.Categories.Where(n => n.Active == 1 && n.Level.Length == 5).ToList();
                for (int i = 0; i < cat.Count; i++)
                {
                    ViewBag.Cat = new SelectList(cat, "Id", "Name", Edit.CatId);
                }
                var catL1 = data.Categories.Where(m => m.Id == Edit.CatId).ToList();
                var catL2 = data.Categories.Where(m => m.Level.Length == (catL1[0].Level.Length + 5) && m.Level.Substring(0, 5) == catL1[0].Level && m.Active == 1).ToList();
                if (catL2.Count > 0)
                {
                    for (int j = 0; j < catL2.Count; j++)
                    {
                        ViewBag.CatL2 = new SelectList(catL2, "Id", "Name", Edit.CatL2);
                    }
                }
                var brand = data.Brands.ToList();
                for (int i = 0; i < brand.Count; i++)
                {
                    ViewBag.Brand = new SelectList(brand, "Id", "Name", Edit.BrandId);
                }
                //string strColor = "";
                //var color = data.Colors.ToList();
                //strColor += "<div style=' border:1px solid #a0a0a0; padding:5px;'>";
                //strColor += "<div class='showHideChkColor'>Hiển thị</div>";
                //strColor += "<div id='divChkColor'>";
                //for (int i = 0; i < color.Count; i++)
                //{
                //    var proC = data.ProColors.Where(m => m.ProId == id && m.ColorId== color[i].Id).ToList();
                //    if (proC.Count>0)
                //    {
                //        strColor += "<p style='width:10%; float:left;'>";
                //        strColor += "<input type='checkbox' value='" + color[i].Id + "' id='chk_" + i + "' name='chkColor' checked='checked'/>";
                //        strColor += "<span style='margin-left:3px;'>" + color[i].Name + "</span>";
                //        strColor += "</p>";
                //    }
                //    else
                //    {
                //        strColor += "<p style='width:10%; float:left;'>";
                //        strColor += "<input type='checkbox' value='" + color[i].Id + "' id='chk_" + i + "' name='chkColor'/>";
                //        strColor += "<span style='margin-left:3px;'>" + color[i].Name + "</span>";
                //        strColor += "</p>";
                //    }
                //}
                //strColor += "<div class='clearfix'></div>";
                //strColor += "</div>";
                //strColor += "</div>";
                //ViewBag.Color = strColor;
                //string strSize = "";
                //var size = data.Sizes.ToList();
                //strSize += "<div style=' border:1px solid #a0a0a0; padding:5px;'>";
                //strSize += "<div class='showHideChkSize'>Hiển thị</div>";
                //strSize += "<div id='divChkSize'>";
                //for (int i = 0; i < size.Count; i++)
                //{
                //    var proS = data.ProSizes.Where(m => m.ProId == id && m.SizeId==size[i].Id).ToList();
                //    if (proS.Count > 0)
                //    {
                //        strSize += "<p style='width:10%; float:left;'>";
                //        strSize += "<input type='checkbox' value='" + size[i].Id + "' id='chk_" + i + "' name='chkSize' checked='checked'/>";
                //        strSize += "<span style='margin-left:3px;'>" + size[i].Name + "</span>";
                //        strSize += "</p>";
                //    }
                //    else
                //    {
                //        strSize += "<p style='width:10%; float:left;'>";
                //        strSize += "<input type='checkbox' value='" + size[i].Id + "' id='chk_" + i + "' name='chkSize'/>";
                //        strSize += "<span style='margin-left:3px;'>" + size[i].Name + "</span>";
                //        strSize += "</p>";
                //    }
                //}
                //strSize += "<div class='clearfix'></div>";
                //strSize += "</div>";
                //strSize += "</div>";
                //ViewBag.Size = strSize;
                return View(Edit);
            }
            else
            {
                return Redirect("/Admins/admins");
            }
        }
        #endregion
        #region[ProductEdit]
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult ProductEdit(int id, FormCollection collection)
        {
            if (Session["Username"] != null)
            {
                var pro = data.Products.First(m => m.Id == id);
                pro.Name = collection["Name"];
                pro.Tag = DongHo.Models.StringClass.NameToTag(collection["Name"]);
                pro.Content = collection["Content"];
                pro.Detail = collection["Detail"];
                pro.Image = collection["Image"];
                pro.Date = Convert.ToDateTime(DateTime.Now.ToString());
                pro.CatId = Convert.ToInt32(collection["Cat"]);
                pro.CatL2 = int.Parse(collection["CatL2"]);
                pro.BrandId = Convert.ToInt32(collection["Brand"]);
                //var c = data.Categories.Where(p => p.Id == Convert.ToInt32(collection["Cat"])).ToList();
                pro.Keyword = collection["Keyword"];
                pro.Description = collection["Description"];
                pro.Title = collection["Title"];
                pro.Price = Convert.ToDouble(collection["Price"]);
                if (collection["PiceOld"] == "")
                {
                    pro.PiceOld = "0";
                }
                else
                {
                    pro.PiceOld = collection["PiceOld"];
                }
                var DateBegin = collection["DateBegin"];
                var DateEnd = collection["DateEnd"];
                if (DateBegin == "" && DateEnd == "")
                {
                    pro.DateBegin = null;
                    pro.DateEnd = null;
                }
                else
                {
                    string dateInputBegin = "";
                    string dateInputEnd = "";
                    DateTime myDateBegin;
                    DateTime myDateEnd;
                    string[] dateArrBegin = DateBegin.Split('/');
                    string[] dateArrEnd = DateEnd.Split('/');
                    if (dateArrBegin.Length == 3) dateInputBegin = dateArrBegin[2] + "-" + dateArrBegin[1] + "-" + dateArrBegin[0];
                    if (dateArrEnd.Length == 3) dateInputEnd = dateArrEnd[2] + "-" + dateArrEnd[1] + "-" + dateArrEnd[0];
                    if (DateTime.TryParse(dateInputBegin, out myDateBegin)) pro.DateBegin = myDateBegin;
                    if (DateTime.TryParse(dateInputEnd, out myDateEnd)) pro.DateEnd = myDateEnd;
                }
                pro.Ord = Convert.ToInt32(collection["Ord"]);
                var priority = (collection["Prioritys"] == "false") ? 0 : 1;
                pro.Priority = Convert.ToInt16(priority);
                var index = (collection["Indexs"] == "false") ? 0 : 1;
                pro.Index = Convert.ToInt16(index);
                var active = (collection["Actives"] == "false") ? 0 : 1;
                pro.Active = active;
                pro.Codepro = (collection["Codepro"] != "") ? int.Parse(collection["Codepro"]) : 0;
                pro.View = 0;
                pro.Count = 0;
                pro.SpTon = 0;//sp con ton trong kho
                data.SubmitChanges();
                #region[Them vao bang ProColor]
                if (collection["txtColor"] != "")
                {
                    var color = (from a in data.Colors select a.Id).ToList();
                    var strColors = collection["txtColor"].Replace("Tất cả,", " ").Split(',').Select(x => int.Parse(x)).ToList();
                    var strDel = color.Except(strColors).ToList();
                    for (int i = 0; i < strColors.Count; i++)
                    {
                        var list = data.ProColors.Where(m => m.ProId == id && m.ColorId == strColors[i]).ToList();
                        if (list.Count == 0)
                        {
                            ProColor obj = new ProColor();
                            obj.ColorId = strColors[i];
                            obj.ProId = id;
                            data.ProColors.InsertOnSubmit(obj);
                            data.SubmitChanges();
                        }
                    }
                    for (int k = 0; k < strDel.Count; k++)
                    {
                        var list = data.ProColors.Where(m => m.ColorId == strDel[k] && m.ProId == id).FirstOrDefault();
                        if (list != null)
                        {
                            data.ProColors.DeleteOnSubmit(list);
                            data.SubmitChanges();
                        }
                    }
                    //for (int i = 0; i < color.Count; i++)
                    //{
                    //    var strColor = collection["txtColor"].Replace("(all),", " ").Split(',').Select(x => int.Parse(x)).ToList();
                    //    for (int k = 0; k < strColor.Count; k++)
                    //    {
                    //        if (color.Count > 0)
                    //        {
                    //            if (color[i] == strColor[k])
                    //            {
                    //                var proC = data.ProColors.Where(m => m.ProId == id && m.ColorId == color[i]).FirstOrDefault();
                    //                if (proC == null)
                    //                {
                    //                    ProColor obj = new ProColor();
                    //                    obj.ColorId = color[i];
                    //                    obj.ProId = id;
                    //                    data.ProColors.InsertOnSubmit(obj);
                    //                    data.SubmitChanges();
                    //                }
                    //                color.RemoveAt(i);
                    //                i = (i != 0) ? (i -= 1) : 0;
                    //            }
                    //        }
                    //        else
                    //        {
                    //            break;
                    //        }
                    //    }
                    //}
                    //for (int i = 0; i < color.Count; i++)
                    //{
                    //    var proC = data.ProColors.Where(m => m.ProId == id && m.ColorId == color[i]).FirstOrDefault();
                    //    if (proC != null)
                    //    {
                    //        data.ProColors.DeleteOnSubmit(proC);
                    //        data.SubmitChanges();
                    //    }
                    //}
                }
                #endregion
                #region[Them vao bang ProSize]
                if (collection["txtSize"] != "")
                {
                    var size = (from b in data.Sizes select b.Id).ToList();
                    var strSize = collection["txtSize"].Replace("Tất cả,", " ").Split(',').Select(x => int.Parse(x)).ToList();
                    var strDel = size.Except(strSize).ToList();
                    for (int i = 0; i < strSize.Count; i++)
                    {
                        var list = data.ProSizes.Where(m => m.ProId == id && m.SizeId == strSize[i]).ToList();
                        if (list.Count == 0)
                        {
                            ProSize obj = new ProSize();
                            obj.SizeId = strSize[i];
                            obj.ProId = id;
                            data.ProSizes.InsertOnSubmit(obj);
                            data.SubmitChanges();
                        }
                    }
                    for (int k = 0; k < strDel.Count; k++)
                    {
                        var list = data.ProSizes.Where(m => m.SizeId == strDel[k] && m.ProId == id).FirstOrDefault();
                        if (list != null)
                        {
                            data.ProSizes.DeleteOnSubmit(list);
                            data.SubmitChanges();
                        }
                    }
                }
                #endregion
                #region[Them vao bang ProPrice]
                var proPrice = data.ProPrices.Where(m => m.ProId == id).OrderByDescending(m => m.Id).FirstOrDefault();
                if (proPrice != null)
                {
                    if (collection["Price"] != proPrice.GiaBanLe || collection["PiceOld"] != proPrice.PricePromotion)
                    {
                        ProPrice proP = new ProPrice();
                        proP.ProId = id;
                        proP.GiaBanLe = collection["Price"];
                        proP.PricePromotion = collection["PiceOld"];
                        if (DateBegin == "" && DateEnd == "")
                        {
                            proP.DateBegin = null;
                            proP.DateEnd = null;
                        }
                        else
                        {
                            proP.DateBegin = Convert.ToDateTime(DateBegin);
                            proP.DateEnd = Convert.ToDateTime(DateEnd);
                        }
                        proP.Date = DateTime.Now;
                        data.ProPrices.InsertOnSubmit(proP);
                        data.SubmitChanges();
                    }
                }
                else
                {
                    ProPrice proP = new ProPrice();
                    proP.ProId = id;
                    proP.GiaBanLe = collection["Price"];
                    proP.PricePromotion = collection["PiceOld"];
                    if (DateBegin == "" && DateEnd == "")
                    {
                        proP.DateBegin = null;
                        proP.DateEnd = null;
                    }
                    else
                    {
                        proP.DateBegin = Convert.ToDateTime(DateBegin);
                        proP.DateEnd = Convert.ToDateTime(DateEnd);
                    }
                    proP.Date = DateTime.Now;
                    data.ProPrices.InsertOnSubmit(proP);
                    data.SubmitChanges();
                }
                #endregion
                return RedirectToAction("ProductIndex");
            }
            else
            {
                return Redirect("/Admins/admins");
            }
        }
        #endregion
        #region[ProductActive]
        public ActionResult ProductActive(int id)
        {
            if (Session["Username"] != null)
            {
                var list = data.Products.First(m => m.Id == id);
                var pro = data.Products.First(m => m.Id == id);
                if (list.Active == 1)
                {
                    pro.Active = 0;
                }
                else
                {
                    pro.Active = 1;
                }
                data.SubmitChanges();
                return RedirectToAction("ProductIndex");
            }
            else
            {
                return Redirect("/Admins/admins");
            }
        }
        #endregion
        #region[ProductDelete]
        public ActionResult ProductDelete(int id)
        {
            if (Session["Username"] != null)
            {
                var del = data.Products.First(m => m.Id == id);
                data.Products.DeleteOnSubmit(del);
                data.SubmitChanges();
                return RedirectToAction("ProductIndex");
            }
            else
            {
                return Redirect("/Admins/admins");
            }
        }
        #endregion
        #region[MultiCommand]
        public ActionResult MultiCommand(string command, FormCollection collect)
        {
            if (Session["Username"] != null)
            {
                List<Product> list;
                if (command == "MultiDelete")
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
                                var Del = (from del in data.Products where del.Id == id select del).SingleOrDefault();
                                data.Products.DeleteOnSubmit(Del);
                                str += id.ToString() + ",";
                                data.SubmitChanges();
                            }
                        }
                    }
                    return RedirectToAction("ProductIndex");
                }
                else if (command == "AddImport")
                {
                    foreach (string key in Request.Form)
                    {
                        var checkbox = "";
                        if (key.StartsWith("chk"))
                        {
                            checkbox = Request.Form["" + key];
                            if (checkbox != "false")
                            {
                                if (Session["AddProductImport"] == null)
                                {
                                    list = new List<Product>();
                                    Int32 id = Convert.ToInt32(key.Remove(0, 3));
                                    var imp = (from im in data.Products where im.Id == id select im).SingleOrDefault();
                                    list.Add(imp);
                                }
                                else
                                {
                                    list = (List<Product>)Session["AddProductImport"];
                                    Int32 id = Convert.ToInt32(key.Remove(0, 3));
                                    var imp = (from im in data.Products where im.Id == id select im).SingleOrDefault();
                                    list.Add(imp);
                                }
                                Session["AddProductImport"] = list;
                            }
                        }
                    }
                    return Redirect("/Import/ImportCreate");
                }
                else//command == AddExport
                {
                    foreach (string key in Request.Form)
                    {
                        var checkbox = "";
                        if (key.StartsWith("chk"))
                        {
                            checkbox = Request.Form["" + key];
                            if (checkbox != "false")
                            {
                                if (Session["AddProductExport"] == null)
                                {
                                    list = new List<Product>();
                                    Int32 id = Convert.ToInt32(key.Remove(0, 3));
                                    var imp = (from im in data.Products where im.Id == id select im).SingleOrDefault();
                                    list.Add(imp);
                                }
                                else
                                {
                                    list = (List<Product>)Session["AddProductExport"];
                                    Int32 id = Convert.ToInt32(key.Remove(0, 3));
                                    var imp = (from im in data.Products where im.Id == id select im).SingleOrDefault();
                                    list.Add(imp);
                                }
                                Session["AddProductExport"] = list;
                            }
                        }
                    }
                    return Redirect("/Export/ExportCreate");
                }
            }
            else
            {
                return Redirect("/Admins/admins");
            }
        }
        #endregion
        #region[DropDownCheckboxColor]
        public ActionResult _DropDownCheckboxColor(int id=0)
        {
            string chuoi = "";
            var color = data.Colors.ToList();
            chuoi += "<select id=\"s5\" class=\"s5\" multiple=\"multiple\">";
            if (color.Count > 0)
            {
                chuoi += "<option>Tất cả</option>";
                for (int i = 0; i < color.Count; i++)
                {
                    if (id != 0)
                    {
                        var proC = data.ProColors.Where(m => m.ColorId == color[i].Id && m.ProId == id).FirstOrDefault();
                        if (proC != null)
                        {
                            chuoi += "<option id=\"chkId_" + color[i].Id + "\" name=\"chkId\" value=\"" + color[i].Id + "\" selected=\"selected\">" + color[i].Name + "</option>";
                        }
                        else
                        {
                            chuoi += "<option id=\"chkId_" + color[i].Id + "\" name=\"chkId\" value=\"" + color[i].Id + "\">" + color[i].Name + "</option>";
                        }
                    }
                    else
                    {
                        chuoi += "<option id=\"chkId_" + color[i].Id + "\" name=\"chkId\" value=\"" + color[i].Id + "\">" + color[i].Name + "</option>";
                    }
                }
            }
            chuoi += "</select>";
            ViewBag.Color = chuoi;
            return PartialView();
        }
        #endregion
        #region[DropDownCheckboxSize]
        public ActionResult _DropDownCheckboxSize(int id = 0)
        {
            string chuoi = "";
            var size = data.Sizes.ToList();
            chuoi += "<select id=\"s51\" class=\"s51\" multiple=\"multiple\">";
            if (size.Count > 0)
            {
                chuoi += "<option>Tất cả</option>";
                for (int i = 0; i < size.Count; i++)
                {
                    if (id != 0)
                    {
                        var proC = data.ProSizes.Where(m => m.SizeId == size[i].Id && m.ProId == id).FirstOrDefault();
                        if (proC != null)
                        {
                            chuoi += "<option id=\"chkId_" + size[i].Id + "\" name=\"chkId\" value=\"" + size[i].Id + "\" selected=\"selected\">" + size[i].Name + "</option>";
                        }
                        else
                        {
                            chuoi += "<option id=\"chkId_" + size[i].Id + "\" name=\"chkId\" value=\"" + size[i].Id + "\">" + size[i].Name + "</option>";
                        }
                    }
                    else
                    {
                        chuoi += "<option id=\"chkId_" + size[i].Id + "\" name=\"chkId\" value=\"" + size[i].Id + "\">" + size[i].Name + "</option>";
                    }
                }
            }
            chuoi += "</select>";
            ViewBag.Size = chuoi;
            return PartialView();
        }
        #endregion
        #region[ProductPriceView]
        public ActionResult ProductPriceView(int id)
        {
            if (Session["Username"] != null)
            {
                var pro = data.Products.Where(m => m.Id == id).FirstOrDefault();
                var proPrice = data.ProPrices.Where(m => m.ProId == id).OrderByDescending(m => m.DateBegin).ToList();
                string chuoi = "";
                chuoi += "<h2>Xem thông tin sản phẩm</h2>";
                chuoi += "<div class=\"viewInfo\">";
                chuoi += "<div>";
                chuoi += "<p>Tên sản phẩm</p>";
                chuoi += "<p>Giá bán lẻ hiện tại</p>";
                chuoi += "<p>Giá bán sỉ hiện tại</p>";
                chuoi += "<p>Giá nhập hàng mới nhất</p>";
                chuoi += "<p>Giá khuyến mãi hiện tại</p>";
                chuoi += "<p>Ngày bắt đầu áp dụng</p>";
                chuoi += "<p>Ngày kết thúc áp dụng</p>";
                chuoi += "</div>";
                chuoi += "<div>";
                chuoi += "<p>" + pro.Name + "</p>";
                chuoi += "<p>" + Format_Price(proPrice[0].GiaBanLe) + " VNĐ</p>";
                chuoi += "<p>" + Format_Price(proPrice[0].GiaBanSi) + " VNĐ</p>";
                chuoi += "<p>" + Format_Price(proPrice[0].PriceImport) + " VNĐ</p>";
                if (pro.PiceOld != null)
                {
                    chuoi += "<p>" + Format_Price(pro.PiceOld.ToString()) + " VNĐ</p>";
                    chuoi += "<p>" + DateTimeClass.ConvertDateTimeddMMyyyy(pro.DateBegin.ToString()) + "</p>";
                    chuoi += "<p>" + DateTimeClass.ConvertDateTimeddMMyyyy(pro.DateEnd.ToString()) + "</p>";
                }
                else
                {
                    chuoi += "<p>0 VNĐ</p>";
                    chuoi += "<p>Không khuyến mãi</p>";
                    chuoi += "<p>Không khuyến mãi</p>";
                }
                chuoi += "</div>";
                chuoi += "<div>";
                chuoi += "<img src=\"" + pro.Image + "\" style=\"width:100px; height:100px; margin-left: 50px;\" />";
                chuoi += "</div>";
                chuoi += "</div>";
                chuoi += "<div class=\"clearfix\"></div>";
                if (proPrice.Count > 0)
                {
                    chuoi += "<div class=\"divShowHideHistory\">";
                    chuoi += "<div class=\"showHideHistory\">Hiển thị thông tin lịch sử giá</div>";
                    chuoi += "<div id=\"divHistory\">";
                    chuoi += "<table border=\"1\">";
                    chuoi += "<tr>";
                    chuoi += "<th>STT</th>";
                    chuoi += "<th>Giá nhập</th>";
                    chuoi += "<th>Giá bán bán sỉ</th>";
                    chuoi += "<th>Giá bán lẻ</th>";
                    chuoi += "<th>Giá khuyến mãi</th>";
                    chuoi += "<th>Ngày bắt đầu</th>";
                    chuoi += "<th>Ngày kết thúc</th>";
                    chuoi += "<th>Ngày nhập</th>";
                    chuoi += "<th>Chức năng</th>";
                    chuoi += "</tr>";
                    for (int i = 0; i < proPrice.Count; i++)
                    {
                        chuoi += "<tr>";
                        chuoi += "<td>" + (i + 1) + "</td>";
                        chuoi += "<td>" + Format_Price(proPrice[i].PriceImport) + " VNĐ</td>";
                        chuoi += "<td>" + Format_Price(proPrice[i].GiaBanSi) + " VNĐ</td>";
                        chuoi += "<td>" + Format_Price(proPrice[i].GiaBanLe) + " VNĐ</td>";
                        if (proPrice[i].PricePromotion != null)
                        {
                            chuoi += "<td>" + proPrice[i].PricePromotion + "</td>";
                            chuoi += "<td>" + DateTimeClass.ConvertDateTimeddMMyyyy(proPrice[i].DateBegin.ToString()) + "</td>";
                            chuoi += "<td>" + DateTimeClass.ConvertDateTimeddMMyyyy(proPrice[i].DateEnd.ToString()) + "</td>";
                        }
                        else
                        {
                            chuoi += "<td>0 VNĐ</td>";
                            chuoi += "<td>Không khuyến mãi</td>";
                            chuoi += "<td>Không khuyến mãi</td>";
                        }
                        chuoi += "<td>" + DateTimeClass.ConvertDateTimeddMMyyyy(proPrice[i].Date.ToString()) + "</td>";
                        chuoi += "<td><a href='/Products/ProductPriceEdit/" + proPrice[i].Id + "' title='Chỉnh sửa khung giá' class='edit'>Sửa</a><a href='/Products/ProductPriceDelete/" + proPrice[i].Id + "' title='Xóa' class='vdel'>Xóa</a></td>";
                        chuoi += "</tr>";
                    }
                    chuoi += "</table>";
                    chuoi += "</div>";
                    chuoi += "</div>";
                    chuoi += "<a href=\"/Products/ProductPriceCreate/" + pro.Id + "\">Thêm khung giá mới</a>";
                }
                ViewBag.View = chuoi;
                return View();
            }
            else
            {
                return Redirect("/Admins/admins");
            }
        }
        #endregion
        #region[ProductPriceCreate]
        public ActionResult ProductPriceCreate(int id)
        {
            var imp = data.ImportDetails.Where(m => m.ProId == id).OrderByDescending(m => m.Date).FirstOrDefault();
            if (imp != null)
            {
                ViewBag.Imp = imp.Price;
            }
            return View();
        }
        #endregion
        #region[ProductPriceCreate]
        [HttpPost]
        public ActionResult ProductPriceCreate(FormCollection collect, ProPrice pp,int id)
        {
            pp.ProId = id;
            pp.GiaBanLe = collect["GiaBanLe"];
            pp.GiaBanSi = collect["GiaBanSi"];
            pp.PriceImport = collect["PriceImport"];
            pp.PricePromotion = collect["PricePromotion"];
            var datebegin = collect["DateBegin"];
            var dateend = collect["DateEnd"];
            string dateInputBegin = "";
            string dateInputEnd = "";
            DateTime myDateBegin;
            DateTime myDateEnd;
            string[] dateArrBegin = datebegin.Split('/');
            string[] dateArrEnd = dateend.Split('/');
            if (dateArrBegin.Length == 3) dateInputBegin = dateArrBegin[2] + "-" + dateArrBegin[1] + "-" + dateArrBegin[0];
            if (dateArrEnd.Length == 3) dateInputEnd = dateArrEnd[2] + "-" + dateArrEnd[1] + "-" + dateArrEnd[0];
            if (DateTime.TryParse(dateInputBegin, out myDateBegin)) pp.DateBegin = myDateBegin;
            if (DateTime.TryParse(dateInputEnd, out myDateEnd)) pp.DateEnd = myDateEnd;
            pp.Date = DateTime.Now;
            data.ProPrices.InsertOnSubmit(pp);
            data.SubmitChanges();
            return RedirectToAction("ProductIndex");
        }
        #endregion
        #region[ProductPriceEdit]
        public ActionResult ProductPriceEdit(int id)
        {
            var edit = data.ProPrices.Where(m => m.Id == id).FirstOrDefault();
            return View(edit);
        }
        #endregion
        #region[ProductPriceEdit]
        [HttpPost]
        public ActionResult ProductPriceEdit(int id, FormCollection collect)
        {
            var edit = data.ProPrices.Where(m => m.Id == id).FirstOrDefault();
            edit.GiaBanLe = collect["GiaBanLe"];
            edit.GiaBanSi = collect["GiaBanSi"];
            edit.PriceImport = collect["PriceImport"];
            edit.PricePromotion = collect["PricePromotion"];
            edit.Ord = int.Parse(collect["PricePromotion"]);
            edit.Date = DateTime.Now;
            var datebegin = collect["DateBegin"];
            var dateend = collect["DateEnd"];
            string dateInputBegin = "";
            string dateInputEnd = "";
            DateTime myDateBegin;
            DateTime myDateEnd;
            string[] dateArrBegin = datebegin.Split('/');
            string[] dateArrEnd = dateend.Split('/');
            if (dateArrBegin.Length == 3) dateInputBegin = dateArrBegin[2] + "-" + dateArrBegin[1] + "-" + dateArrBegin[0];
            if (dateArrEnd.Length == 3) dateInputEnd = dateArrEnd[2] + "-" + dateArrEnd[1] + "-" + dateArrEnd[0];
            if (DateTime.TryParse(dateInputBegin, out myDateBegin)) edit.DateBegin = myDateBegin;
            if (DateTime.TryParse(dateInputEnd, out myDateEnd)) edit.DateEnd = myDateEnd;
            data.SubmitChanges();
            return RedirectToAction("ProductIndex");
        }
        #endregion
        #region[ProductPriceDelete]
        public ActionResult ProductPriceDelete(int id)
        {
            var del = data.ProPrices.Where(m => m.Id == id).FirstOrDefault();
            data.ProPrices.DeleteOnSubmit(del);
            return RedirectToAction("ProductIndex");
        }
        #endregion
        #region [Format_Price]
        protected string Format_Price(string Price)
        {
            Price = Price.Replace(".", "");
            Price = Price.Replace(",", "");
            string tmp = "";
            while (Price.Length > 3)
            {
                tmp = "." + Price.Substring(Price.Length - 3) + tmp;
                Price = Price.Substring(0, Price.Length - 3);
            }
            tmp = Price + tmp;
            return tmp;
        }
        #endregion
        #region [NumberStr]
        protected string NumberStr(string str)
        {
            return str.Replace(".", "");
        }
        #endregion
        #region[CascadingDropDown - Lay CatL2]
        public ActionResult GetCatL2(int id)
        {
            var cat = data.Categories.Where(m => m.Id == id).ToList();
            var catL2 = data.Categories.Where(m => m.Level.Length == (cat[0].Level.Length + 5) && m.Level.Substring(0, 5) == cat[0].Level && m.Active == 1).ToList();
            List<ddl> list = new List<ddl>();
            for (int i = 0; i < catL2.Count; i++)
            {
                list.Add(new ddl { value = catL2[i].Id.ToString(), text = catL2[i].Name });
            }
            if (list.Count > 0)
            {
                ViewBag.CatL2 = new SelectList(list, "value", "text");
            }
            return PartialView();
        }
        #endregion
        #region[ddl - get - set]
        public class ddl
        {
            public string value { get; set; }
            public string text { get; set; }
        }
        #endregion
        #region[Xem anh]
        public ActionResult ProductViewImg(int id)
        {
            var chuoiA = "";
            var chuoiB = "";
            var chuoiC = "";
            var chuoiD = "";
            var chuoiE = "";
            var list = data.ProImages.Where(m => m.ProId == id).ToList();
            chuoiA += "<h4>Ảnh lớn nhất (_huge):</h4>";
            chuoiB += "<h4>Ảnh lớn (_big):</h4>";
            chuoiC += "<h4>Ảnh vừa (_noz):</h4>";
            chuoiD += "<h4>Ảnh nhỏ (_small):</h4>";
            chuoiE += "<h4>Ảnh khác</h4>";
            for (int i = 0; i < list.Count; i++)
            {
                var a = list[i].Image.IndexOf("_huge");
                var b = list[i].Image.IndexOf("_big");
                var c = list[i].Image.IndexOf("_noz");
                var d = list[i].Image.IndexOf("_small");
                if (a > 0)
                {
                    chuoiA += "<div class=\"imgView\"><img src=\"" + list[i].Image + "\" /><div class=\"funcImg\"><a href=\"/Products/ProductEditImg/" + list[i].Id + "\">Sửa</a><a href=\"/Products/ProductDelImg/" + list[i].Id + "\">Xóa</a></div></div>";
                    //chuoiA += "<a href='/Products/ProductEditImg/" + list[i].Id + "' class='editImg'><img src='" + list[i].Image + "' id='fileImg' name='fileImg'/></a>";
                }
                else if (b > 0)
                {
                    chuoiB += "<div class=\"imgView\"><img src=\"" + list[i].Image + "\" /><div class=\"funcImg\"><a href=\"/Products/ProductEditImg/" + list[i].Id + "\">Sửa</a><a href=\"/Products/ProductDelImg/" + list[i].Id + "\">Xóa</a></div></div>";
                    //chuoiB += "<a href='/Products/ProductEditImg/" + list[i].Id + "' class='editImg'><img src='" + list[i].Image + "' id='fileImg' name='fileImg'/></a>";
                }
                else if (c > 0)
                {
                    chuoiC += "<div class=\"imgView\"><img src=\"" + list[i].Image + "\" /><div class=\"funcImg\"><a href=\"/Products/ProductEditImg/" + list[i].Id + "\">Sửa</a><a href=\"/Products/ProductDelImg/" + list[i].Id + "\">Xóa</a></div></div>";
                    //chuoiC += "<a href='/Products/ProductEditImg/" + list[i].Id + "' class='editImg'><img src='" + list[i].Image + "' id='fileImg' name='fileImg'/></a>";
                }
                else if (d > 0)
                {
                    chuoiD += "<div class=\"imgView\"><img src=\"" + list[i].Image + "\" /><div class=\"funcImg\"><a href=\"/Products/ProductEditImg/" + list[i].Id + "\">Sửa</a><a href=\"/Products/ProductDelImg/" + list[i].Id + "\">Xóa</a></div></div>";
                    //chuoiD += "<a href='/Products/ProductEditImg/" + list[i].Id + "' class='editImg'><img src='" + list[i].Image + "' id='fileImg' name='fileImg'/></a>";
                }
                else
                {
                    chuoiE += "<div class=\"imgView\"><img src=\"" + list[i].Image + "\" /><div class=\"funcImg\"><a href=\"/Products/ProductEditImg/" + list[i].Id + "\">Sửa</a><a href=\"/Products/ProductDelImg/" + list[i].Id + "\">Xóa</a></div></div>";
                    //chuoiE += "<a href='/Products/ProductEditImg/" + list[i].Id + "' class='editImg'><img src='" + list[i].Image + "' id='fileImg' name='fileImg'/></a>";
                }
            }
            chuoiE += "<div class='clearfix'></div><a href='/Products/ProductAddImg/" + id + "' class='addImg'>Thêm ảnh mới</a>";
            ViewBag.ViewA = chuoiA;
            ViewBag.ViewB = chuoiB;
            ViewBag.ViewC = chuoiC;
            ViewBag.ViewD = chuoiD;
            ViewBag.ViewE = chuoiE;
            return View();
        }
        #endregion
        #region[Sua anh]
        public ActionResult ProductEditImg(int id)
        {
            var list = data.ProImages.Where(m => m.Id == id).FirstOrDefault();
            return View(list);
        }
        #endregion
        #region[Sua anh]
        [HttpPost]
        public ActionResult ProductEditImg(int id, HttpPostedFileBase fileImg)
        {
            if (fileImg.ContentLength > 0)
            {
                String FileExtn = System.IO.Path.GetExtension(fileImg.FileName).ToLower();
                if (!(FileExtn == ".jpg" || FileExtn == ".png" || FileExtn == ".gif"))
                {
                    ViewBag.error = "Only jpg, gif and png files are allowed!";
                }
                else
                {
                    var u = fileImg.FileName;
                    List<string> sizeImg = new List<string>();
                    sizeImg.Add("_huge");
                    sizeImg.Add("_big");
                    sizeImg.Add("_noz");
                    sizeImg.Add("_small");
                    string co = "";
                    string kco = "";
                    for (int i = 0; i < sizeImg.Count; i++)
                    {
                        var a = u.LastIndexOf(sizeImg[i]);
                        if (a > 0)
                        {
                            co = u.Substring(0, a);
                            kco = sizeImg[i];
                            break;
                        }
                    }
                    var fileName = String.Format("{0}"+kco+".jpg", Guid.NewGuid().ToString());
                    var imagePath = Path.Combine(Server.MapPath(Url.Content("/Uploads")), fileName);
                    fileImg.SaveAs(imagePath);
                    var ProImg = data.ProImages.First(m => m.Id == id);
                    var im = ProImg.Image.IndexOf("/Uploads/");
                    if (im > -1)
                    {
                        var anh = ProImg.Image.Substring(im);
                        var d = Request.PhysicalApplicationPath + anh;
                        System.IO.File.Delete(d);
                    }
                    ProImg.Image = "/Uploads/" + fileName;
                    ProImg.Date = DateTime.Now;
                    data.SubmitChanges();
                }
            }
            return RedirectToAction("ProductIndex");
        }
        #endregion
        #region[Them anh]
        public ActionResult ProductAddImg()
        {
            return View();
        }
        #endregion
        #region[Them anh]
        [HttpPost]
        public ActionResult ProductAddImg( IEnumerable<HttpPostedFileBase> fileImg, int id)
        {
            foreach (var file in fileImg)
            {
                if (file.ContentLength > 0)
                {
                    var b = (from k in data.ProImages select k.Id).Max();
                    var ab = Request.Files["fileImg"];
                    String FileExtn = System.IO.Path.GetExtension(file.FileName).ToLower();
                    if (!(FileExtn == ".jpg" || FileExtn == ".png" || FileExtn == ".gif"))
                    {
                        ViewBag.error = "Only jpg, gif and png files are allowed!";
                    }
                    else
                    {
                        ProImage img = new ProImage();
                        var Filename = Path.GetFileName(file.FileName);
                        List<string> sizeImg = new List<string>();
                        sizeImg.Add("_huge");
                        sizeImg.Add("_big");
                        sizeImg.Add("_noz");
                        sizeImg.Add("_small");
                        string co = "";
                        string kco = "";
                        for (int i = 0; i < sizeImg.Count; i++)
                        {
                            var a = Filename.LastIndexOf(sizeImg[i]);
                            if (a > 0)
                            {
                                co = Filename.Substring(0, a);
                                kco = sizeImg[i];
                                break;
                            }
                        }
                        var fileName = String.Format("{0}" + kco + ".jpg", Guid.NewGuid().ToString());
                        //String imgPath = String.Format("Uploads/{0}{1}", file.FileName, FileExtn);
                        //file.Save(String.Format("{0}{1}", Server.MapPath("~"), imgPath), Img.RawFormat);
                        var path = Path.Combine(Server.MapPath(Url.Content("/Uploads")), fileName);
                        file.SaveAs(path);
                        img.ProId = id;
                        img.Image = "/Uploads/" + fileName;
                        img.Date = DateTime.Now;
                        data.ProImages.InsertOnSubmit(img);
                        data.SubmitChanges();
                    }
                }
                var fd = file;
            }
            return RedirectToAction("ProductIndex");
        }
        #endregion
        #region [Xoa anh]
        public ActionResult ProductDelImg(int id)
        {
            var pro = data.ProImages.Where(m => m.Id == id).FirstOrDefault();
            var a = Request.PhysicalApplicationPath + pro.Image;
            System.IO.File.Delete(a);
            data.ProImages.DeleteOnSubmit(pro);
            data.SubmitChanges();
            return RedirectToAction("ProductIndex");
        }
        #endregion
    }
}
