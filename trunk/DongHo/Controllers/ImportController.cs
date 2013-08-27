using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DongHo.Models;

namespace DongHo.Controllers
{
    public class ImportController : Controller
    {
        //
        // GET: /Import/
        DataDataContext data = new DataDataContext();
        #region[ImportIndex]
        public ActionResult ImportIndex()
        {
            if (Session["Username"] != null)
            {
                string page = "1";//so phan trang hien tai
                var pagesize = "25";//so ban ghi tren 1 trang
                var numOfNews = 0;//tong so ban ghi co duoc truoc khi phan trang
                int curpage = 0; // trang hien tai dung cho phan trang
                var DateBegin = "";
                var DateEnd = "";
                var SupId = "";
                var EmpName = "";
                var PriceFrom = "";
                var PriceTo = "";
                var ImpId = "";
                var str = "1=1";
                #region[check Request]
                if (Request["page"] != null) { page = Request["page"]; curpage = Convert.ToInt32(page) - 1; }
                if (Request["dateForm"] != null) { DateBegin = Request["dateForm"].ToString(); if (DateBegin != "") { str += " and idate >= '" + DateTimeClass.ConvertDateTimeMMddyyyy(DateBegin) + "'"; } }
                if (Request["dateTo"] != null) { DateEnd = Request["dateTo"].ToString(); if (DateEnd != "") { str += " and idate <= '" + DateTimeClass.ConvertDateTimeMMddyyyy(DateEnd) + "'"; } }
                if (Request["sup"] != null) { SupId = Request["sup"].ToString(); if (SupId != "") { str += " and UserName = '" + SupId + "'"; } }
                if (Request["emp"] != null) { EmpName = Request["emp"].ToString(); if (EmpName != "") { str += " and UserId = " + EmpName + ""; } }
                if (Request["priceForm"] != null) { PriceFrom = Request["priceForm"].ToString(); if (PriceFrom != "") { str += " and TotalMoney <= '" + PriceFrom + "'"; } }
                if (Request["priceTo"] != null) { PriceTo = Request["priceTo"].ToString(); if (PriceTo != "") { str += " and TotalMoney >= '" + PriceTo + "'"; } }
                if (Request["impCode"] != null) { ImpId = Request["impCode"].ToString(); if (ImpId != "") { str += " and Id = " + ImpId + ""; } }
                #endregion
                var all = data.sp_Import_GetByTop("", str, "").ToList();
                var pages = data.sp_Import_Phantrang(page, pagesize, str, "idate desc").ToList();
                var url = Request.Path;
                numOfNews = all.Count;

                string chuoi = "";
                if (pages.Count > 0)
                {
                    for (int i = 0; i < pages.Count; i++)
                    {
                        var wh = data.WareHouses.First(m => m.Id == pages[i].WareHouseId);
                        var user = data.Members.First(m => m.Id == pages[i].UserId);
                        var sup = data.Suppliers.First(m => m.Id == int.Parse(pages[i].UserName));
                        if (i % 2 == 0)
                        {
                            chuoi += "<tr id=\"trOdd_" + i + "\" class=\"trOdd\" onmousemove=\"Javascript:chkSelect_OnMouseMove1(this)\" onmouseout=\"Javascript:chkSelect_OnMouseOut1(this,0)\">";
                            chuoi += "<td class=\"CheckBoxsmall\"><input id='chkSelect' name='chk" + pages[i].Id + "' onclick = 'Javascript:chkSelect_OnClick(this,0)' class='chk' type='checkbox'/></td>";
                            chuoi += "<td class='Text'>" + wh.Name + "</td>";
                            chuoi += "<td class='Text'>" + user.Name + "</td>";
                            chuoi += "<td class='Text'>" + sup.Name + "</td>";
                            chuoi += "<td class='Number'>" + Format_Price(pages[i].TotalMoney) + " VNĐ</td>";
                            chuoi += "<td class='DateTimesmal'>" + Models.DateTimeClass.ConvertDateTimeddMMyyyy(pages[i].idate.ToString()) + "</td>";
                            chuoi += "<td class='Function'>";
                            chuoi += "<a href='/Import/ImportViewDetail/" + pages[i].Id + "' class='views' title='Xem'>Xem</a>";
                            chuoi += "<a href='/Import/ImportEdit/" + pages[i].Id + "' class='vedit' title='Sửa'>Sửa</a>";
                            chuoi += "<a href='/Import/ImportDelete/" + pages[i].Id + "' class='vdelete' title='Xóa'>Xóa</a>";
                            chuoi += "</td>";
                            chuoi += "</tr>";
                        }
                        else
                        {
                            chuoi += "<tr id=\"trEven_" + i + "\" class=\"trEven\" onmousemove=\"Javascript:chkSelect_OnMouseMove1(this)\" onmouseout=\"Javascript:chkSelect_OnMouseOut1(this," + i + ")\">";
                            chuoi += "<td class=\"CheckBoxsmall\"><input id='chkSelect' name='chk" + pages[i].Id + "' onclick = 'Javascript:chkSelect_OnClick(this,0)' class='chk' type='checkbox'/></td>";
                            chuoi += "<td class='Text'>" + wh.Name + "</td>";
                            chuoi += "<td class='Text'>" + user.Name + "</td>";
                            chuoi += "<td class='Text'>" + sup.Name + "</td>";
                            chuoi += "<td class='Number'>" + Format_Price(pages[i].TotalMoney) + " VNĐ</td>";
                            chuoi += "<td class='DateTimesmal'>" + Models.DateTimeClass.ConvertDateTimeddMMyyyy(pages[i].idate.ToString()) + "</td>";
                            chuoi += "<td class='Function'>";
                            chuoi += "<a href='/Import/ImportViewDetail/" + pages[i].Id + "' class='views' title='Xem'>Xem</a>";
                            chuoi += "<a href='/Import/ImportEdit/" + pages[i].Id + "' class='vedit' title='Sửa'>Sửa</a>";
                            chuoi += "<a href='/Import/ImportDelete/" + pages[i].Id + "' class='vdelete' title='Xóa'>Xóa</a>";
                            chuoi += "</td>";
                            chuoi += "</tr>";
                        }
                    }
                }
                ViewBag.View = chuoi;
                if (pages.Count > 0)
                {
                    ViewBag.Pager = DongHo.Models.Phantrang.PhanTrang(25, curpage, numOfNews, url);
                }
                else
                {
                    ViewBag.Pager = "";
                }
                return View();
            }
            else
            {
                return Redirect("/Admins/admins");
            }
        }
        #endregion
        #region[ImportViewDetail]
        public ActionResult ImportViewDetail(int id)
        {
            if (Session["Username"] != null)
            {
                string chuoi = "";
                var view = data.Imports.First(m => m.Id == id);
                var wh = data.WareHouses.First(m => m.Id == view.WareHouseId);
                var detail = data.ImportDetails.Where(m => m.ImportId == view.Id).ToList();
                var user = data.Users.First(m => m.Id == view.UserId);
                chuoi += "<h2>Thông tin chi tiết phiếu nhập</h2>";
                chuoi += "<p>Ngày: " + DateTimeClass.ConvertDateTimeddMMyyyy(view.idate.ToString()) + "</p>";
                chuoi += "<div class=\"viewInfo\">";
                chuoi += "<div>";
                chuoi += "<p>Kho nhập</p>";
                chuoi += "<p>Địa chỉ kho</p>";
                chuoi += "<p>Điện thoại kho</p>";
                chuoi += "<p>Người lập phiếu</p>";
                chuoi += "</div>";
                chuoi += "<div>";
                chuoi += "<p>" + wh.Name + "</p>";
                chuoi += "<p>" + wh.Address + "</p>";
                chuoi += "<p>" + wh.Phone + "</p>";
                chuoi += "<p>" + user.Name + "</p>";
                chuoi += "</div>";
                chuoi += "</div>";
                chuoi += "<div class='clearfix'></div>";
                chuoi += "<table border=\"1\">";
                chuoi += "<tr>";
                chuoi += "<th class=\"STT\">STT</th>";
                chuoi += "<th class=\"NamePro\">Tên sản phẩm</th>";
                chuoi += "<th>Số lượng sản phẩm</th>";
                chuoi += "<th>Thành tiền</th>";
                chuoi += "</tr>";
                for (int i = 0; i < detail.Count; i++)
                {
                    var pro = data.Products.Where(m => m.Id == detail[i].ProId).FirstOrDefault();
                    chuoi += "<tr>";
                    chuoi += "<td class=\"STT\">" + (i + 1) + "</td>";
                    if (pro != null)
                    {
                        chuoi += "<td class=\"NamePro\">" + pro.Name + "</td>";
                    }
                    else
                    {
                        chuoi += "<td class=\"NamePro\">" + detail[i].ProId + " (Đã xóa khỏi danh sách sản phẩm)</td>";
                    }
                    chuoi += "<td>" + detail[i].Quantity + "</td>";
                    chuoi += "<td>" + Format_Price(detail[i].Money) + " VNĐ</td>";
                    chuoi += "</tr>";
                }
                chuoi += "<tr>";
                chuoi += "<td colspan=\"3\">Tổng tiền</td>";
                chuoi += "<td>" + Format_Price(view.TotalMoney) + " VNĐ</td>";
                chuoi += "</tr>";
                chuoi += "</table>";
                chuoi += "<h4>Ghi chú</h4><br/>";
                if (view.Note == "")
                {
                    chuoi += "<span>Không có ghi chú nào</span>";
                }
                else
                {
                    chuoi += "<span>" + view.Note + "</span>";
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
        #region[ImportCreate]
        public ActionResult ImportCreate()
        {
            if (Session["Username"] != null)
            {
                string chuoiEmp = "";
                string chuoiCat = "";
                ViewBag.WareHouse = new SelectList(data.WareHouses, "Id", "Name");
                #region[View DDL Ten nhan vien]
                var grmem = data.GroupMembers.ToList();
                chuoiEmp += "<select id='NameEmp' name='NameEmp'>";
                chuoiEmp += "<option value>==== Chọn tên nhân viên ====</option>";
                for (int i = 0; i < grmem.Count; i++)
                {
                    var mem = data.Members.Where(m => m.GroupMemberId == grmem[i].Id && m.Active == 1).ToList();
                    if (mem.Count > 0)
                    {
                        chuoiEmp += "<optgroup label=\"" + grmem[i].Name + "\"></optgroup>";
                        for (int k = 0; k < mem.Count; k++)
                        {
                            if (Session["Fullname"].ToString() == mem[k].Name)
                            {
                                chuoiEmp += "<option value='" + mem[k].Id + "' selected='selected'>" + mem[k].Name + "</option>";
                            }
                            else
                            {
                                chuoiEmp += "<option value='" + mem[k].Id + "'>" + mem[k].Name + "</option>";
                            }
                        }
                    }
                }
                chuoiEmp += "</select>";
                ViewBag.NameEmp = chuoiEmp;
                #endregion
                var imp = data.Imports.OrderByDescending(m => m.Id).FirstOrDefault();
                ViewBag.ImpId = (imp.Id + 1);//Ma phieu nhap moi cua phieu nhap chuan bi create
                #region[View DDL Nhom san pham]
                var cat = data.Categories.OrderBy(m => m.Level).ToList();
                chuoiCat += "<select id='proCat' name='proCat'>";
                chuoiCat += "<option value>==== Chọn nhóm sản phẩm ====</option>";
                for (int j = 0; j < cat.Count; j++)
                {
                    chuoiCat += "<option value='" + cat[j].Id + "'>" + StringClass.ShowNameLevel(cat[j].Name, cat[j].Level) + "</option>";
                }
                chuoiCat += "</select>";
                ViewBag.proCat = chuoiCat;
                #endregion
                ViewBag.Supplier = new SelectList(data.Suppliers, "Id", "Name");
                #region[View table Phieu nhap]
                string chuoipro = "";
                chuoipro += "<table border=\"1\" class=\"proImport\">";
                chuoipro += "<tr>";
                chuoipro += "<th>TT</th>";
                chuoipro += "<th>Sản phẩm</th>";
                chuoipro += "<th>Số lượng</th>";
                chuoipro += "<th>Đơn giá xuất</th>";
                chuoipro += "<th>Thành tiền</th>";
                chuoipro += "<th>Chức năng</th>";
                chuoipro += "</tr>";
                if (Session["AddProductImport"] != null)
                {
                    List<Product> list = (List<Product>)Session["AddProductImport"];
                    for (int i = 0; i < list.Count; i++)
                    {
                        chuoipro += "<tr>";
                        chuoipro += "<td>" + (i + 1) + "</td>";
                        chuoipro += "<td>" + list[i].Name + "</td>";
                        chuoipro += "<td><input type=\"text\" value=\"0\" name=\"txtQuantity_" + i + "\" id=\"txtQuantity_" + i + "\" class='quantity'/></td>";
                        chuoipro += "<td><span class='price'>" + list[i].Price + "</span><input type='hidden' name='hidePrice_" + i + "' id='hidePrice_" + i + "' value='" + list[i].Price + "'/> VNĐ</td>";
                        chuoipro += "<td><span class='money'>0</span><input type='hidden' name='hideMoney_" + i + "' id='hideMoney_" + i + "' value='0'/> VNĐ</td>";
                        chuoipro += "<td><a href=\"/Import/ImportRemove/" + i + "\" class=\"vdel\" title='Xóa'>Xóa</a></td>";
                        chuoipro += "</tr>";
                    }
                    chuoipro += "<tr class='trTotal'>";
                    chuoipro += "<td colspan=\"4\" class=\"trTilte\"><input type='hidden' name='hideTotal' id='hideTotal' value='0'/>Tổng tiền</td>";
                    chuoipro += "<td colspan=\"2\" class=\"trTotal\"><span id='total'>0</span> VNĐ</td>";
                    chuoipro += "</tr>";
                }
                chuoipro += "</table>";
                ViewBag.tbImport = chuoipro;
                #endregion
                string page = "1";//so phan trang hien tai
                var pagesize = "10";//so ban ghi tren 1 trang
                var numOfNews = 0;//tong so ban ghi co duoc truoc khi phan trang
                int curpage = 0; // trang hien tai dung cho phan trang
                var str = "1=1";
                var cats = "";
                var sup = "";
                #region[check request category]
                if (Request["cat"] != null)
                {
                    cats = Request["cat"].ToString();
                    if (cats != "")
                    {
                        var c = data.Categories.Where(m => m.Id == int.Parse(cats)).FirstOrDefault();
                        if (c.Level.Length == 5) { str += " and CatId = '" + cats + "'"; }
                        else if (c.Level.Length == 10) { str += " and CatL2 = '" + cats + "'"; }
                    }
                }
                #endregion
                #region[check request supplier]
                if (Request["cc"] != null)
                {
                    sup = Request["cc"].ToString();
                    if (sup != "")
                    {
                        var brand = data.Brands.Where(m => m.SupplierId == int.Parse(sup)).ToList();
                        if (brand.Count > 0)
                        {
                            for (int i = 0; i < brand.Count; i++)
                            {
                                if (i == 0) { str += " and BrandId='" + brand[i].Id + "'"; }
                                else { str += " or BrandId='" + brand[i].Id + "'"; }
                            }
                        }
                    }
                }
                #endregion
                if (Request["page"] != null) { page = Request["page"]; curpage = Convert.ToInt32(page) - 1; }
                var all = data.sp_Product_GetByTop("", str, "").ToList();
                var pages = data.sp_Product_Phantrang(page, pagesize, str, "SpTon desc").ToList();
                numOfNews = all.Count;
                if (pages.Count > 0)
                {
                    if (sup == "" && cats == "")
                    {
                        var url = Request.Path;
                        ViewBag.Pager = DongHo.Models.Phantrang.PhanTrang(10, curpage, numOfNews, url);
                    }
                    else
                    {
                        var url = Request.Url.PathAndQuery;
                        int u = url.IndexOf("&page=");
                        if (u > 0) { url = url.Substring(0, u); }
                        ViewBag.Pager = DongHo.Models.PhantrangQuery.PhanTrangQuery(10, curpage, numOfNews, url);
                    }
                }
                else
                {
                    ViewBag.Pager = "";
                }
                return View(pages);
                //if (Session["AddProductImport"] != null)
                //{
                //    List<Product> listPro = (List<Product>)Session["AddProductImport"];
                //    chuoipro += "<table class='proImport'>";
                //    chuoipro += "<tr>";
                //    chuoipro += "<th style='width:60px'>Mã sản phẩm</th>";
                //    chuoipro += "<th>Tên sản phẩm</th>";
                //    chuoipro += "<th style='width:90px'>Số lượng nhập</th>";
                //    chuoipro += "<th style='width:90px'>Thành tiền</th>";
                //    chuoipro += "<th style='width:60px'>Chức năng</th>";
                //    chuoipro += "</tr>";
                //    for (int i = 0; i < listPro.Count; i++)
                //    {
                //        chuoipro += "<tr>";
                //        chuoipro += "<td style='width:60px'>" + listPro[i].Id + "</td>";
                //        chuoipro += "<td>" + listPro[i].Name + "</td>";
                //        chuoipro += "<td style='width:90px'><input type='text' id='quantity_" + i + "' name='quantity_" + i + "' value='0'/></td>";
                //        chuoipro += "<td style='width:90px'><input type='text' id='money_" + i + "' name='money_" + i + "' value='0' class='money'/> VNĐ</td>";
                //        chuoipro += "<td style='width:60px'><a href='/Import/ImportRemove/" + i + "' title='Xóa khỏi danh sách nhập' class='vdel'>Xóa</a></td>";
                //        chuoipro += "</tr>";
                //    }
                //    chuoipro += "<tr>";
                //    chuoipro += "<td colspan='3'>Tổng tiền</td>";
                //    chuoipro += "<td colspan='2' class='last'><input type='text' id='total' name='total' value='0'/> VNĐ</td>";
                //    chuoipro += "</tr>";
                //    chuoipro += "</table>";
                //    ViewBag.ProImport = chuoipro;
                //}
                //else
                //{
                //    TempData["alertMessage"] = "Bạn phải chọn sản phẩm được nhập về từ bảng Sản phẩm trước khi có thể tạo mới 1 Phiếu nhập";
                //    return new JavaScriptResult { Script = "alert('Saved successfully');" };
                //}
            }
            else
            {
                return Redirect("/Admins/admins");
            }
        }
        #endregion
        #region[ImportCreate]
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult ImportCreate(FormCollection collect)
        {
            if (Session["Username"] != null)
            {
                Import imp = new Import();
                imp.WareHouseId = Convert.ToInt32(collect["WareHouse"]);
                //var user = data.Users.First(m => m.Username == Session["Username"].ToString());
                imp.UserId = 2;
                imp.TotalMoney = collect["hideTotal"];
                imp.idate = DateTime.Now;
                imp.Note = collect["Note"];
                imp.UserName = collect["Supplier"];
                data.Imports.InsertOnSubmit(imp);
                data.SubmitChanges();
                var impId = data.Imports.OrderByDescending(m => m.Id).FirstOrDefault();
                List<Product> listpro = (List<Product>)Session["AddProductImport"];
                if (listpro.Count > 0)
                {
                    for (int i = 0; i < listpro.Count; i++)
                    {
                        ImportDetail impDetail = new ImportDetail();
                        impDetail.ImportId = impId.Id;
                        impDetail.ProId = listpro[i].Id;
                        impDetail.Quantity = int.Parse(collect["txtQuantity_" + i]);
                        impDetail.Money = collect["hideMoney_" + i];
                        impDetail.Price = collect["hidePrice_" + i];
                        impDetail.Date = DateTime.Now;
                        impDetail.Ord = (i + 1);
                        data.ImportDetails.InsertOnSubmit(impDetail);
                        data.SubmitChanges();
                        var obj = data.Products.Where(m => m.Id == listpro[i].Id).FirstOrDefault();
                        obj.SpTon = (obj.SpTon + int.Parse(collect["txtQuantity_" + i]));
                        data.SubmitChanges();
                    }
                }
                Session["AddProductImport"] = null;
                return RedirectToAction("ImportIndex");
            }
            else
            {
                return Redirect("/Admins/admins");
            }
        }
        #endregion
        #region[ImportEdit]
        public ActionResult ImportEdit(int id)
        {
            if (Session["Username"] != null)
            {
                var Edit = data.Imports.First(m => m.Id == id);
                var impDetail = data.ImportDetails.Where(m => m.ImportId == Edit.Id).ToList();
                var wh = data.WareHouses.ToList();
                for (int k = 0; k < wh.Count; k++)
                {
                    ViewBag.WareHouse = new SelectList(wh, "Id", "Name", Edit.WareHouseId);
                }
                ViewBag.ViewDate = DateTimeClass.ConvertDateTimeddMMyyyy(Edit.idate.ToString());
                string chuoipro = "";
                chuoipro += "<table class='proImport'>";
                chuoipro += "<tr>";
                chuoipro += "<th>Mã sản phẩm</th>";
                chuoipro += "<th>Tên sản phẩm</th>";
                chuoipro += "<th>Số lượng nhập</th>";
                chuoipro += "<th>Thành tiền</th>";
                chuoipro += "</tr>";
                for (int i = 0; i < impDetail.Count; i++)
                {
                    var listPro = data.Products.Where(m => m.Id == impDetail[i].ProId).FirstOrDefault();
                    if (listPro != null)
                    {
                        chuoipro += "<tr>";
                        chuoipro += "<td>" + listPro.Id + "</td>";
                        chuoipro += "<td>" + listPro.Name + "</td>";
                        chuoipro += "<td><input type='text' id='quantity_" + i + "' name='quantity_" + i + "' value='" + impDetail[i].Quantity + "'/></td>";
                        chuoipro += "<td><input type='text' id='money_" + i + "' name='money_" + i + "' value='" + impDetail[i].Money + "' class='money'/> VNĐ</td>";
                        chuoipro += "</tr>";
                    }
                }
                chuoipro += "<tr>";
                chuoipro += "<td colspan='3'>Tổng tiền</td>";
                chuoipro += "<td><input type='text' id='total' name='total' value='" + Edit.TotalMoney + "'/> VNĐ</td>";
                chuoipro += "</tr>";
                chuoipro += "</table>";
                ViewBag.ProImport = chuoipro;
                return View(Edit);
            }
            else
            {
                return Redirect("/Admins/admins");
            }
        }
        #endregion
        #region[ImportEdit]
        [HttpPost]
        public ActionResult ImportEdit(int id, FormCollection collect)
        {
            if (Session["Username"] != null)
            {
                var imp = data.Imports.First(m => m.Id == id);
                imp.WareHouseId = int.Parse(collect["WareHouse"]);
                imp.Note = collect["Note"];
                imp.Ord = int.Parse(collect["Ord"]);
                imp.TotalMoney = collect["total"];
                data.SubmitChanges();
                var impDetail = data.ImportDetails.Where(m => m.ImportId == imp.Id).ToList();
                for (int i = 0; i < impDetail.Count; i++)
                {
                    var impDe = data.ImportDetails.Where(m => m.Id == impDetail[i].Id).FirstOrDefault();
                    if (impDe != null)
                    {
                        impDe.Money = collect["money_" + i];
                        impDe.Quantity = int.Parse(collect["quantity_" + i]);
                        data.SubmitChanges();
                    }
                }
                return RedirectToAction("ImportIndex");
            }
            else
            {
                return Redirect("/Admins/admins");
            }
        }
        #endregion
        #region[ImportDelete]
        public ActionResult ImportDelete(int id)
        {
            if (Session["Username"] != null)
            {
                var del = data.ImportDetails.Where(m => m.ImportId == id).ToList();
                for (int i = 0; i < del.Count; i++)
                {
                    data.ImportDetails.DeleteOnSubmit(del[i]);
                    data.SubmitChanges();
                }
                var deleteImport = data.Imports.First(m => m.Id == id);
                data.Imports.DeleteOnSubmit(deleteImport);
                data.SubmitChanges();
                return RedirectToAction("ImportIndex");
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
                            var del = data.ImportDetails.Where(m => m.ImportId == id).ToList();
                            for (int i = 0; i < del.Count; i++)
                            {
                                data.ImportDetails.DeleteOnSubmit(del[i]);
                                data.SubmitChanges();
                            }
                            var Del = (from emp in data.Imports where emp.Id == id select emp).SingleOrDefault();
                            data.Imports.DeleteOnSubmit(Del);
                            str += id.ToString() + ",";
                            data.SubmitChanges();
                        }
                    }
                }
                return RedirectToAction("ImportIndex");
            }
            else
            {
                return Redirect("/Admins/admins");
            }
        }
        #endregion
        #region[ImportRemove]
        public ActionResult ImportRemove(int id)
        {
            List<Product> listPro = (List<Product>)Session["AddProductImport"];
            listPro.RemoveAt(id);
            return RedirectToAction("ImportCreate");
        }
        #endregion
        #region[Format_Price]
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
        #region[_search top]
        public ActionResult _search()
        {
            string chuoi = "";
            var grmem = data.GroupMembers.ToList();
            chuoi += "<select id='NameEmp' name='NameEmp'>";
            chuoi += "<option value>==== Chọn tên nhân viên ====</option>";
            for (int i = 0; i < grmem.Count; i++)
            {
                var mem = data.Members.Where(m => m.GroupMemberId == grmem[i].Id && m.Active == 1).ToList();
                if (mem.Count > 0)
                {
                    chuoi += "<optgroup label=\"" + grmem[i].Name + "\"></optgroup>";
                    for (int k = 0; k < mem.Count; k++)
                    {
                        chuoi += "<option value='" + mem[k].Id + "'>" + mem[k].Name + "</option>";
                    }
                }
            }
            chuoi += "</select>";
            ViewBag.NameEmp = chuoi;
            return PartialView();
        }
        #endregion
        #region[_search top]
        [HttpPost]
        public ActionResult _search(FormCollection collect)
        {
            var DateBegin = "";
            var DateEnd = "";
            var SupId = "";
            var EmpName = "";
            var PriceFrom = "";
            var PriceTo = "";
            var ImpId = "";
            DateBegin = collect["dateBegin"];
            DateEnd = collect["dateEnd"];
            SupId = collect["Supplier"];
            EmpName = collect["NameEmp"];
            PriceFrom = collect["PriceFrom"];
            PriceTo = collect["PriceTo"];
            ImpId = collect["Id"];
            var url = "/Import/ImportIndex?dateForm=" + DateBegin + "&dateTo=" + DateEnd + "&sup=" + SupId + "&emp=" + EmpName + "&priceForm=" + PriceFrom + "&priceTo=" + PriceTo + "&impCode=" + ImpId + "";
            return Redirect(url);
        }
        #endregion
        #region[ImportSearchPro]
        [HttpPost]
        public ActionResult ImportSearchPro(FormCollection collect)
        {
            var cat = collect["proCat"];
            var sup = collect["Supplier"];
            var url = "/Import/ImportCreate?cat=" + cat + "&cc=" + sup + "";
            return Redirect(url);
        }
        #endregion
        #region[AddProductImport]
        public ActionResult AddProductImport(int id)
        {
            List<Product> list;
            var pro = data.Products.Where(m => m.Id == id).FirstOrDefault();
            if (pro != null)
            {
                if (Session["AddProductImport"] == null)
                {
                    list = new List<Product>();
                    list.Add(pro);
                }
                else
                {
                    list = (List<Product>)Session["AddProductImport"];
                    list.Add(pro);
                }
                Session["AddProductImport"] = list;
            }
            return Redirect("/Import/ImportCreate");
        }
        #endregion
        #region[ImportProAddMulti]
        [HttpPost]
        public ActionResult ImportProAddMulti(FormCollection collect)
        {
            List<Product> list;
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
        #endregion
    }
}
