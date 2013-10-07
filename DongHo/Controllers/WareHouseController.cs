using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DongHo.Models;

namespace DongHo.Controllers
{
    public class WareHouseController : Controller
    {
        //
        // GET: /WareHouse/
        DataDataContext data = new DataDataContext();
        #region[WareHouseIndex]
        public ActionResult WareHouseIndex()
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
            var all = data.WareHouses.ToList();
            var pages = all.Skip(curpage * pagesize).Take(pagesize).ToList();
            //var pages = data.sp_WareHouse_Phantrang(page, pagesize, "", "").ToList();
            var url = Request.Path;
            numOfNews = all.Count;
            ViewBag.Pager = DongHo.Models.Phantrang.PhanTrang(pagesize, curpage, numOfNews, url);
            return View(pages);
        }
        #endregion
        #region[CascadingDropdown - GetMember]
        public ActionResult _GetMember(int id)
        {
            var mem = data.Members.Where(m => m.GroupMemberId == id).ToList();
            for (int i = 0; i < mem.Count; i++)
            {
                ViewBag.MemId = new SelectList(mem, "Id", "Name");
            }
            return PartialView();
        }
        #endregion
        #region[WareHouseCreate]
        public ActionResult WareHouseCreate()
        {
            ViewBag.GroupMember = new SelectList(data.GroupMembers, "Id", "Name");
            return View();
        }
        #endregion
        #region[WareHouseCreate]
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult WareHouseCreate(FormCollection collec, WareHouse wh)
        {
            if (Request.Cookies["Username"] != null)
            {
                wh.Name = collec["Name"];
                wh.Address = collec["Address"];
                wh.Phone = collec["Phone"];
                wh.Ord = Convert.ToInt32(collec["Ord"]);
                wh.Note = collec["Note"];
                data.WareHouses.InsertOnSubmit(wh);
                data.SubmitChanges();
                var list = data.WareHouses.OrderByDescending(m => m.Id).ToList();
                var u = collec["MemId"];
                var user = data.Users.Where(m => m.Id == int.Parse(u)).FirstOrDefault();
                UserWareHouse userWh = new UserWareHouse();
                userWh.UserId = user.Id;
                userWh.WareHouseId = list[0].Id;
                userWh.DateReceive = DateTime.Now;
                data.UserWareHouses.InsertOnSubmit(userWh);
                data.SubmitChanges();
                return RedirectToAction("WareHouseIndex");
            }
            else
            {
                return Redirect("/Admins/admins");
            }
        }
        #endregion
        #region[WareHouseEdit]
        public ActionResult WareHouseEdit(int id)
        {
            var Edit = data.WareHouses.First(m => m.Id == id);
            var uWh = data.UserWareHouses.Where(m => m.WareHouseId == Edit.Id).OrderByDescending(m => m.DateReceive).ToList();
            var u = data.Members.Where(m => m.Id == uWh[0].UserId).FirstOrDefault();
            ViewBag.GroupMember = new SelectList(data.GroupMembers, "Id", "Name", u.GroupMemberId);
            var mem = data.Members.Where(m => m.GroupMemberId == u.GroupMemberId).ToList();
            ViewBag.MemId = new SelectList(mem, "Id", "Name", uWh[0].UserId);
            return View(Edit);
        }
        #endregion
        #region[WareHouseEdit]
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult WareHouseEdit(FormCollection collec, int id)
        {
            if (Request.Cookies["Username"] != null)
            {
                var wh = data.WareHouses.First(m => m.Id == id);
                wh.Name = collec["Name"];
                wh.Address = collec["Address"];
                wh.Phone = collec["Phone"];
                wh.Ord = Convert.ToInt32(collec["Ord"]);
                wh.Note = collec["Note"];
                var u = int.Parse(collec["MemId"]);
                data.SubmitChanges();
                var uWh = data.UserWareHouses.Where(m => m.WareHouseId == id).OrderByDescending(m => m.DateReceive).FirstOrDefault();
                if (u != uWh.UserId)
                {
                    uWh.DateLeave = DateTime.Now;
                    data.SubmitChanges();
                    UserWareHouse userWh = new UserWareHouse();
                    userWh.UserId = u;
                    userWh.WareHouseId = id;
                    userWh.DateReceive = DateTime.Now;
                    data.UserWareHouses.InsertOnSubmit(userWh);
                    data.SubmitChanges();
                }
                return RedirectToAction("WareHouseIndex");
            }
            else
            {
                return Redirect("/Admins/admins");
            }
        }
        #endregion
        #region[WareHouseDelete]
        public ActionResult WareHouseDelete(int id)
        {
            if (Request.Cookies["Username"] != null)
            {
                if (ModelState.IsValid)
                {
                    var del = data.WareHouses.First(m => m.Id == id);
                    data.WareHouses.DeleteOnSubmit(del);
                    data.SubmitChanges();
                }
                return RedirectToAction("WareHouseIndex");
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
                            var Del = (from emp in data.WareHouses where emp.Id == id select emp).SingleOrDefault();
                            data.WareHouses.DeleteOnSubmit(Del);
                            str += id.ToString() + ",";
                            data.SubmitChanges();
                        }
                    }
                }
                return RedirectToAction("WareHouseIndex");
            }
            else
            {
                return Redirect("/Admins/admins");
            }
        }
        #endregion
        #region[HangTon]
        public ActionResult HangTon()
        {
            string chuoi = "";
            int? countPro;
            var imp = (from b in data.Imports select b).ToList();
            //Lay tong so sp da nhap theo tung sp
            var impTotal = (from a in data.ImportDetails group a by a.ProId into s select new { ProImpId = s.Key, total = s.Sum(a => a.Quantity) }).ToList();
            //Lay tong so sp da xuat theo tung sp
            var expTotal = (from a in data.ExportDetails group a by a.ProId into s select new { ProExpId = s.Key, total = s.Sum(a => a.Quantity) }).ToList();
            var im = impTotal;
            if (expTotal.Count > 0)
            {
                for (int i = 0; i < expTotal.Count; i++)
                {
                    var ProExp = expTotal[i].ProExpId;
                    var countExp = expTotal[i].total;
                    for (int k = 0; k < impTotal.Count; k++)
                    {
                        var ProImp = impTotal[k].ProImpId;
                        var countImp = impTotal[k].total;
                        if (ProExp == ProImp)
                        {
                            var pro = data.Products.Where(m => m.Id == ProExp).FirstOrDefault();
                            countPro = countImp - countExp;
                            if (pro != null)
                            {
                                if (i % 2 == 0)
                                {
                                    chuoi += "<tr id=\"trOdd_" + i + "\" class=\"trOdd\" onmousemove=\"Javascript:chkSelect_OnMouseMove1(this)\" onmouseout=\"Javascript:chkSelect_OnMouseOut1(this,0)\">";
                                    chuoi += "<td class=\"NumberM\">" + (i + 1) + " _ " + pro.Id + "</td>";
                                    chuoi += "<td class=\"Text\">" + pro.Name + "</td>";
                                    chuoi += "<td class=\"Number\">" + countPro + "</td>";
                                    chuoi += "<td class=\"Function\"><a href='/WareHouse/ChiTietHangTon/" + pro.Id + "' class='views'>Xem</a></td>";
                                    chuoi += "</tr>";
                                }
                                else
                                {
                                    chuoi += "<tr id=\"trEven_" + i + "\" class=\"trEven\" onmousemove=\"Javascript:chkSelect_OnMouseMove1(this)\" onmouseout=\"Javascript:chkSelect_OnMouseOut1(this," + i + ")\">";
                                    chuoi += "<td class=\"NumberM\">" + (i + 1) + " _ " + pro.Id + "</td>";
                                    chuoi += "<td class=\"Text\">" + pro.Name + "</td>";
                                    chuoi += "<td class=\"Number\">" + countPro + "</td>";
                                    chuoi += "<td class=\"Function\"><a href='/WareHouse/ChiTietHangTon/" + pro.Id + "' class='views'>Xem</a></td>";
                                    chuoi += "</tr>";
                                }
                                pro.SpTon = countPro;
                                data.SubmitChanges();
                            }
                            else
                            {
                                if (i % 2 == 0)
                                {
                                    chuoi += "<tr id=\"trOdd_" + i + "\" class=\"trOdd\" onmousemove=\"Javascript:chkSelect_OnMouseMove1(this)\" onmouseout=\"Javascript:chkSelect_OnMouseOut1(this,0)\">";
                                    chuoi += "<td class=\"NumberM\">" + (i + 1) + " _ " + impTotal[k].ProImpId + "</td>";
                                    chuoi += "<td class=\"Text\">" + impTotal[k].ProImpId + "(Đã xóa khỏi danh sách sản phẩm)</td>";
                                    chuoi += "<td class=\"Number\">" + countPro + "</td>";
                                    chuoi += "<td class=\"Function\"><a href='/WareHouse/ChiTietHangTon/" + impTotal[k].ProImpId + "' class='views'>Xem</a></td>";
                                    chuoi += "</tr>";
                                }
                                else
                                {
                                    chuoi += "<tr id=\"trEven_" + i + "\" class=\"trEven\" onmousemove=\"Javascript:chkSelect_OnMouseMove1(this)\" onmouseout=\"Javascript:chkSelect_OnMouseOut1(this," + i + ")\">";
                                    chuoi += "<td class=\"NumberM\">" + (i + 1) + " _ " + impTotal[k].ProImpId + "</td>";
                                    chuoi += "<td class=\"Text\">" + impTotal[k].ProImpId + "(Đã xóa khỏi danh sách sản phẩm)</td>";
                                    chuoi += "<td class=\"Number\">" + countPro + "</td>";
                                    chuoi += "<td class=\"Function\"><a href='/WareHouse/ChiTietHangTon/" + impTotal[k].ProImpId + "' class='views'>Xem</a></td>";
                                    chuoi += "</tr>";
                                }
                            }
                            im.RemoveAt(k);
                            break;
                        }
                    }
                }
                for (int j = 0; j < im.Count; j++)
                {
                    var ProImp = im[j].ProImpId;
                    var countImp = im[j].total;
                    var pro = data.Products.Where(m => m.Id == ProImp).FirstOrDefault();
                    if (pro != null)
                    {
                        if (j % 2 == 0)
                        {
                            chuoi += "<tr id=\"trOdd_" + j + "\" class=\"trOdd\" onmousemove=\"Javascript:chkSelect_OnMouseMove1(this)\" onmouseout=\"Javascript:chkSelect_OnMouseOut1(this,0)\">";
                            chuoi += "<td class=\"NumberM\">" + (j + 1) + " _ " + pro.Id + "</td>";
                            chuoi += "<td class=\"Text\">" + pro.Name + "</td>";
                            chuoi += "<td class=\"Number\">" + countImp + "</td>";
                            chuoi += "<td class=\"Function\"><a href='/WareHouse/ChiTietHangTon/" + pro.Id + "' class='views'>Xem</a></td>";
                            chuoi += "</tr>";
                        }
                        else
                        {
                            chuoi += "<tr id=\"trEven_" + j + "\" class=\"trEven\" onmousemove=\"Javascript:chkSelect_OnMouseMove1(this)\" onmouseout=\"Javascript:chkSelect_OnMouseOut1(this," + j + ")\">";
                            chuoi += "<td class=\"NumberM\">" + (j + 1) + " _ " + pro.Id + "</td>";
                            chuoi += "<td class=\"Text\">" + pro.Name + "</td>";
                            chuoi += "<td class=\"Number\">" + countImp + "</td>";
                            chuoi += "<td class=\"Function\"><a href='/WareHouse/ChiTietHangTon/" + pro.Id + "' class='views'>Xem</a></td>";
                            chuoi += "</tr>";
                        }
                        pro.SpTon = countImp;
                        data.SubmitChanges();
                    }
                    else
                    {
                        if (j % 2 == 0)
                        {
                            chuoi += "<tr id=\"trOdd_" + j + "\" class=\"trOdd\" onmousemove=\"Javascript:chkSelect_OnMouseMove1(this)\" onmouseout=\"Javascript:chkSelect_OnMouseOut1(this,0)\">";
                            chuoi += "<td class=\"NumberM\">" + (j + 1) + " _ " + impTotal[j].ProImpId + "</td>";
                            chuoi += "<td class=\"Text\">" + impTotal[j].ProImpId + "(Đã xóa khỏi danh sách sản phẩm)</td>";
                            chuoi += "<td class=\"Number\">" + countImp + "</td>";
                            chuoi += "<td class=\"Function\"><a href='/WareHouse/ChiTietHangTon/" + impTotal[j].ProImpId + "' class='views'>Xem</a></td>";
                            chuoi += "</tr>";
                        }
                        else
                        {
                            chuoi += "<tr id=\"trEven_" + j + "\" class=\"trEven\" onmousemove=\"Javascript:chkSelect_OnMouseMove1(this)\" onmouseout=\"Javascript:chkSelect_OnMouseOut1(this," + j + ")\">";
                            chuoi += "<td class=\"NumberM\">" + (j + 1) + " _ " + impTotal[j].ProImpId + "</td>";
                            chuoi += "<td class=\"Text\">" + impTotal[j].ProImpId + "(Đã xóa khỏi danh sách sản phẩm)</td>";
                            chuoi += "<td class=\"Number\">" + countImp + "</td>";
                            chuoi += "<td class=\"Function\"><a href='/WareHouse/ChiTietHangTon/" + impTotal[j].ProImpId + "' class='views'>Xem</a></td>";
                            chuoi += "</tr>";
                        }
                    }
                }
            }
            else
            {
                if (impTotal.Count > 0)
                {
                    for (int k = 0; k < impTotal.Count; k++)
                    {
                        var ProImp = impTotal[k].ProImpId;
                        var countImp = impTotal[k].total;
                        var pro = data.Products.Where(m => m.Id == ProImp).FirstOrDefault();
                        if (pro != null)
                        {
                            if (k % 2 == 0)
                            {
                                chuoi += "<tr id=\"trOdd_" + k + "\" class=\"trOdd\" onmousemove=\"Javascript:chkSelect_OnMouseMove1(this)\" onmouseout=\"Javascript:chkSelect_OnMouseOut1(this,0)\">";
                                chuoi += "<td class=\"NumberM\">" + (k + 1) + " _ " + pro.Id + "</td>";
                                chuoi += "<td class=\"Text\">" + pro.Name + "</td>";
                                chuoi += "<td class=\"Number\">" + countImp + "</td>";
                                chuoi += "<td class=\"Function\"><a href='/WareHouse/ChiTietHangTon/" + pro.Id + "' class='views'>Xem</a></td>";
                                chuoi += "</tr>";
                            }
                            else
                            {
                                chuoi += "<tr id=\"trEven_" + k + "\" class=\"trEven\" onmousemove=\"Javascript:chkSelect_OnMouseMove1(this)\" onmouseout=\"Javascript:chkSelect_OnMouseOut1(this," + k + ")\">";
                                chuoi += "<td class=\"NumberM\">" + (k + 1) + " _ " + pro.Id + "</td>";
                                chuoi += "<td class=\"Text\">" + pro.Name + "</td>";
                                chuoi += "<td class=\"Number\">" + countImp + "</td>";
                                chuoi += "<td class=\"Function\"><a href='/WareHouse/ChiTietHangTon/" + pro.Id + "' class='views'>Xem</a></td>";
                                chuoi += "</tr>";
                            }
                            pro.SpTon = countImp;
                        }
                        break;
                    }
                }
            }
            ViewBag.View = chuoi;
            return View();
        }
        #endregion
        #region[ChiTietHangTon]
        public ActionResult ChiTietHangTon(int id)
        {
             string chuoi = "";
            int? countPro;
            int? countTotal = 0;
            //Lay ra so san pham da nhap theo id tren cua tung kho
            var impTotal = (from a in data.ImportDetails join b in data.Imports on a.ImportId equals b.Id where a.ProId == id group a by new { a.ProId, b.WareHouseId } into s select new { ProImpId = s.Key.ProId, WHImpId = s.Key.WareHouseId, total = s.Sum(a => a.Quantity) }).ToList();
            //Lay ra tong so san pham da xuat theo id tren cua tung kho
            var expTotal = (from a in data.ExportDetails join b in data.Exports on a.ExportId equals b.Id where a.ProId == id group a by new { a.ProId, b.WareHouseId } into s select new { ProExpId = s.Key.ProId, WHExpId = s.Key.WareHouseId, total = s.Sum(a => a.Quantity) }).ToList();
            var product = data.Products.Where(m => m.Id == id).FirstOrDefault();
            chuoi += "<div class=\"viewInfo\">";
            chuoi += "<div>";
            chuoi += "<p>Mã sản phẩm</p>";
            chuoi += "<p>Tên sản phẩm</p>";
            chuoi += "</div>";
            chuoi += "<div>";
            if (product != null)
            {
                chuoi += "<p>" + product.Id + "</p>";
                chuoi += "<p>" + product.Name + "</p>";
            }
            else
            {
                chuoi += "<p>" + impTotal[0].ProImpId + "</p>";
                chuoi += "<p>" + impTotal[0].ProImpId + "(Đã xóa khỏi danh sách sản phẩm)</p>";
            }
            chuoi += "</div>";
            chuoi += "<table border=\"1\">";
            chuoi += "<tr>";
            chuoi += "<th>Kho</th>";
            chuoi += "<th>Hàng nhập</th>";
            chuoi += "<th>Hàng xuất</th>";
            chuoi += "<th>Hàng tồn</th>";
            chuoi += "</tr>";
            if (expTotal.Count > 0)
            {
                for (int j = 0; j < expTotal.Count; j++)
                {
                    var ProExp = expTotal[j].ProExpId;
                    var countExp = expTotal[j].total;
                    for (int t = 0; t < impTotal.Count; t++)
                    {
                        var ProImp = impTotal[t].ProImpId;
                        var countImp = impTotal[t].total;
                        if (expTotal[j].WHExpId == impTotal[t].WHImpId)
                        {
                            countPro = countImp - countExp;
                            var wh = data.WareHouses.First(m => m.Id == impTotal[t].WHImpId);
                            chuoi += "<tr>";
                            chuoi += "<td>" + wh.Name + "</td>";
                            chuoi += "<td>" + countImp + "</td>";
                            chuoi += "<td>" + countExp + "</td>";
                            chuoi += "<td>" + countPro + "</td>";
                            chuoi += "</tr>";
                            countTotal = countTotal + countPro;
                            //impTotal.RemoveAt(t);
                        }
                        else
                        {
                            var wh = data.WareHouses.First(m => m.Id == impTotal[t].WHImpId);
                            chuoi += "<tr>";
                            chuoi += "<td>" + wh.Name + "</td>";
                            chuoi += "<td>" + countImp + "</td>";
                            chuoi += "<td>0</td>";
                            chuoi += "<td>" + impTotal[t].total + "</td>";
                            chuoi += "</tr>";
                            countTotal = countTotal + impTotal[t].total;
                        }
                    }
                }
            }
            else
            {
                for (int p = 0; p < impTotal.Count; p++)
                {
                    var wh = data.WareHouses.First(m => m.Id == impTotal[p].WHImpId);
                    countPro = impTotal[p].total;
                    chuoi += "<tr>";
                    chuoi += "<td>" + wh.Name + "</td>";
                    chuoi += "<td>" + impTotal[p].total + "</td>";
                    chuoi += "<td> 0 </td>";
                    chuoi += "<td>" + countPro + "</td>";
                    chuoi += "</tr>";
                    countTotal = countTotal + countPro;
                }
            }
            chuoi += "<tr>";
            chuoi += "<td colspan=\"3\">Tổng</td>";
            chuoi += "<td>" + countTotal + "</td>";
            chuoi += "</tr>";
            chuoi += "</table>";
            chuoi += "</div>";
            ViewBag.View = chuoi;
            return View();
        }
        #endregion
        #region[WareHouseViews]
        public ActionResult WareHouseViews(int id)
        {
            string chuoi = "";
            var wh = data.WareHouses.Where(m => m.Id == id).FirstOrDefault();
            var userWh = data.UserWareHouses.Where(m => m.WareHouseId == wh.Id).ToList();
            chuoi += "<h2>Xem thông tin kho hàng</h2>";
            chuoi += "<div class=\"viewInfo\">";
            chuoi += "<div>";
            chuoi += "<p>Tên kho</p>";
            chuoi += "<p>Địa chỉ kho</p>";
            chuoi += "<p>Số điện thoại</p>";
            chuoi += "<div>Ghi chú</div>";
            chuoi += "</div>";
            chuoi += "<div>";
            chuoi += "<p>" + wh.Name + "</p>";
            chuoi += "<p>" + wh.Address + "</p>";
            chuoi += "<p>" + wh.Phone + "</p>";
            chuoi += "</div>";
            chuoi += "</div>";
            chuoi += "<div class=\"clearfix\"></div>";
            chuoi += "<div class='note'>" + wh.Note + "</div>";
            if (userWh.Count > 0)
            {
                chuoi += "<div class=\"divShowHideHistory\">";
                chuoi += "<div class=\"showHideHistory\">Xem thông tin lịch sử nhân viên quản kho</div>";
                chuoi += "<div id=\"divHistory\">";
                chuoi += "<table border=\"1\">";
                chuoi += "<tr>";
                chuoi += "<th>STT</th>";
                chuoi += "<th>Tên nhân viên</th>";
                chuoi += "<th>Ngày bắt đầu</th>";
                chuoi += "<th>Ngày kết thúc</th>";
                chuoi += "</tr>";
                for (int i = 0; i < userWh.Count; i++)
                {
                    var user = data.Members.Where(m => m.Id == userWh[i].UserId).FirstOrDefault();
                    chuoi += "<tr>";
                    chuoi += "<td>" + (i + 1) + "</td>";
                    chuoi += "<td>" + user.Name + "</td>";
                    chuoi += "<td>" + DateTimeClass.ConvertDateTimeddMMyyyy(userWh[i].DateReceive.ToString()) + "</td>";
                    if (userWh[i].DateLeave == null)
                    {
                        chuoi += "<td>Hiện vẫn đang làm</td>";
                    }
                    else
                    {
                        chuoi += "<td>" + DateTimeClass.ConvertDateTimeddMMyyyy(userWh[i].DateLeave.ToString()) + "</td>";
                    }
                    chuoi += "</tr>";
                }
                chuoi += "</table>";
                chuoi += "</div>";
                chuoi += "</div>";
            }
            ViewBag.View = chuoi;
            return View();
        }
        #endregion
    }
}
