using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DongHo.Models;

namespace DongHo.Controllers
{
    public class tbBill_customersController : Controller
    {
        //
        // GET: /tbBill_customers/
        DataDataContext data = new DataDataContext();
        #region[tbBill_customersIndex]
        public ActionResult tbBill_customersIndex()
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
            var all = data.tbBill_customers.ToList();
            var pages = all.Skip(curpage * pagesize).Take(pagesize).ToList();
            //var pages = data.sp_tbBill_customers_Phantrang(page, pagesize, "", "").ToList();
            var url = Request.Path;
            numOfNews = all.Count;
            ViewBag.Pager = DongHo.Models.Phantrang.PhanTrang(25, curpage, numOfNews, url);
            string chuoi = "";
            for (int i = 0; i < pages.Count; i++)
            {
                var cus = data.tbCUSTOMERs.First(m => m.iusid == pages[i].userid);
                if (i % 2 == 0)
                {
                    chuoi += "<tr id=\"trOdd_" + i + "\" class=\"trOdd\" onmousemove=\"Javascript:chkSelect_OnMouseMove1(this)\" onmouseout=\"Javascript:chkSelect_OnMouseOut1(this," + i + ")\">";
                    chuoi += "<td class=\"CheckBoxsmall\"><input id='chkSelect' name='chk" + pages[i].billid + "' onclick = 'Javascript:chkSelect_OnClick(this,0)' class=\"chk\" type='checkbox'/></td>";
                    chuoi += "<td class='Text'>" + cus.vcusname + "</td>";
                    chuoi += "<td class='Date'>" + cus.dbirthday + "</td>";
                    chuoi += "<td class='Text'>" + cus.vaddress + "</td>";
                    chuoi += "<td class='DateTime'>" + pages[i].idate + "</td>";
                    chuoi += "<td class='TextSmall'>" + StringClass.ShowStateBill(pages[i].state.ToString()) + "</td>";
                    chuoi += "<td class='Function'>";
                    chuoi += "<a href='/tbBill_customers/tbBill_customersEdit/" + pages[i].billid + "' class='vedit'>Sửa</a>";
                    if (Request.Cookies["Username"] != null)
                    {
                        chuoi += "<a href='/tbBill_customers/tbBill_customersDelete/" + pages[i].billid + "' class='vdelete'>Xóa</a>";
                    }
                    else
                    {
                        chuoi += "<p class='vdelete' onclick='AlertErr()'>Xóa</p>";
                    }
                    chuoi += "</td>";
                    chuoi += "</tr>";
                }
                else
                {
                    chuoi += "<tr id=\"trEven_" + i + "\" class=\"trEven\" onmousemove=\"Javascript:chkSelect_OnMouseMove1(this)\" onmouseout=\"Javascript:chkSelect_OnMouseOut1(this," + i + ")\">";
                    chuoi += "<td class=\"CheckBoxsmall\"><input id='chkSelect' name='chk" + pages[i].billid + "' onclick = 'Javascript:chkSelect_OnClick(this,0)' class=\"chk\" type='checkbox'/></td>";
                    chuoi += "<td class='Text'>" + cus.vcusname + "</td>";
                    chuoi += "<td class='Date'>" + cus.dbirthday + "</td>";
                    chuoi += "<td class='Text'>" + cus.vaddress + "</td>";
                    chuoi += "<td class='DateTime'>" + pages[i].idate + "</td>";
                    chuoi += "<td class='TextSmall'>" + StringClass.ShowStateBill(pages[i].state.ToString()) + "</td>";
                    chuoi += "<td class='Function'>";
                    chuoi += "<a href='/tbBill_customers/tbBill_customersEdit/" + pages[i].billid + "' class='vedit'>Sửa</a>";
                    if (Request.Cookies["Username"] != null)
                    {
                        chuoi += "<a href='/tbBill_customers/tbBill_customersDelete/" + pages[i].billid + "' class='vdelete'>Xóa</a>";
                    }
                    else
                    {
                        chuoi += "<p class='vdelete' onclick='AlertErr()'>Xóa</p>";
                    }
                    chuoi += "</td>";
                    chuoi += "</tr>";
                }
            }
            ViewBag.View = chuoi;
            return View();
        }
        #endregion
        #region[tbBill_customersEdit]
        public ActionResult tbBill_customersEdit(int id)
        {
            var Edit = data.tbBill_customers.First(m => m.billid == id);
            string chuoicus = "";
            string chuoirec = "";
            string chuoibill = "";
            string chuoi = "";
            var cus = data.tbCUSTOMERs.Where(m => m.iusid == Edit.userid).ToList();
            var rec = data.tbRecipients.Where(m => m.iusid == Edit.userid).ToList();
            var bill = data.tbBilldetails.Where(m => m.bilid == Edit.billid).ToList();
            chuoi += "<ul>";
            if (Request.Cookies["Username"] != null)
            {
                chuoi += "<li><a href='/tbBill_customers/tbBill_customersUpdateState/" + Edit.billid + "' class='uupdate'>Đã giao hàng</a></li>";
                chuoi += "<li><a href='/tbBill_customers/tbBill_customersUpdateState/" + Edit.billid + "?del=2' class='uupdate'>Hủy đơn hàng</a></li>";
            }
            else
            {
                chuoi += "<li><p class='uupdate' onclick='AlertErr()'>Đã giao hàng</p></li>";
                chuoi += "<li><p class='uupdate' onclick='AlertErr()'>Hủy đơn hàng</p></li>";
            }
            chuoi += "<li><a href='/tbBill_customers/tbBill_customersIndex' class='uback'>Trở lại</a></li>";
            chuoi += "</ul>";
            if (cus.Count > 0)
            {
                chuoicus += "<div class=\"ViewBill\">";
                chuoicus += "<h2>Thông tin khách hàng</h2>";
                chuoicus += "<div class=\"title\">";
                chuoicus += "<p>Họ tên</p>";
                chuoicus += "<p>Ngày sinh</p>";
                chuoicus += "<p>Địa chỉ</p>";
                chuoicus += "<p>Điện thoại</p>";
                chuoicus += "<p>Email</p>";
                chuoicus += "<p>Tỉnh/Thành phố</p>";
                chuoicus += "</div>";
                chuoicus += "<div class=\"info\">";
                chuoicus += "<p> " + cus[0].vcusname + "</p>";
                chuoicus += "<p> " + cus[0].dbirthday + "</p>";
                chuoicus += "<p> " + cus[0].vaddress + "</p>";
                chuoicus += "<p> " + cus[0].vphone + "</p>";
                chuoicus += "<p> " + cus[0].vemail + "</p>";
                chuoicus += "<p> " + cus[0].vprovince + "</p>";
                chuoicus += "</div>";
                chuoicus += "<div class=\"clearfix\"></div>";
                chuoicus += "</div>";
            }
            if (rec.Count > 0)
            {

                chuoirec += "<div class=\"ViewBill\">";
                chuoirec += "<h2>Thông tin người nhận hàng</h2>";
                chuoirec += "<div class=\"title\">";
                chuoirec += "<p>Họ tên</p>";
                chuoirec += "<p>Ngày sinh</p>";
                chuoirec += "<p>Địa chỉ</p>";
                chuoirec += "<p>Điện thoại</p>";
                chuoirec += "<p>Email</p>";
                chuoirec += "<p>Tỉnh/Thành phố</p>";
                chuoirec += "</div>";
                chuoirec += "<div class=\"info\">";
                chuoirec += "<p>" + rec[0].vcusname + "</p>";
                chuoirec += "<p>" + rec[0].dbirthday + "</p>";
                chuoirec += "<p>" + rec[0].vaddress + "</p>";
                chuoirec += "<p>" + rec[0].vphone + "</p>";
                chuoirec += "<p>" + rec[0].vemail + "</p>";
                chuoirec += "<p>" + rec[0].vprovince + "</p>";
                chuoirec += "</div>";
                chuoirec += "<div class=\"clearfix\"></div>";
                chuoirec += "</div>";
            }
            chuoibill += "<table class='billdetail'>";
            chuoibill += "<tr>";
            chuoibill += "<th class='namePro'>Sản phẩm</th>";
            chuoibill += "<th>Số lượng</th>";
            chuoibill += "<th>Giá bán</th>";
            chuoibill += "<th>Thành tiền(VNĐ)</th>";
            chuoibill += "</tr>";
            for (int i = 0; i < bill.Count; i++)
            {
                var pro = data.Products.Where(m => m.Id == bill[i].proid).ToList();
                if (pro.Count > 0)
                {
                    chuoibill += "<tr>";
                    chuoibill += "<td class='namePro'>" + pro[0].Name + "</td>";
                    chuoibill += "<td>" + bill[i].quantity + "</td>";
                    chuoibill += "<td>" + Format_Price(bill[i].bilprice) + " VNĐ</td>";
                    chuoibill += "<td>" + Format_Price(bill[i].bilmoney) + " VNĐ</td>";
                    chuoibill += "</tr>";
                }
                else
                {
                    chuoibill += "<tr>";
                    chuoibill += "<td class='namePro'>" + bill[i].proid + "(đã xóa khỏi danh sách sản phẩm)</td>";
                    chuoibill += "<td>" + bill[i].quantity + "</td>";
                    chuoibill += "<td>" + Format_Price(bill[i].bilprice) + " VNĐ</td>";
                    chuoibill += "<td>" + Format_Price(bill[i].bilmoney) + " VNĐ</td>";
                    chuoibill += "</tr>";
                }
            }
            chuoibill += "<tr>";
            chuoibill += "<td colspan='3' class='totalprice'>Tổng tiền</td>";
            chuoibill += "<td>" + Format_Price(Edit.totalmoney) + " VNĐ</td>";
            chuoibill += "</tr>";
            chuoibill += "</table>";
            ViewBag.cus = chuoicus;
            ViewBag.rec = chuoirec;
            ViewBag.billdetail = chuoibill;
            ViewBag.li = chuoi;
            return View();
        }
        #endregion
        #region[Cap nhat trang thai don hang]
        public ActionResult tbBill_customersUpdateState(int id)
        {
            if (Request.Cookies["Username"] != null)
            {
                var up = data.tbBill_customers.First(m => m.billid == id);
                if (up.state == 0)
                {
                    if (Request["del"] != null)
                    {
                        var detail = data.tbBilldetails.Where(m => m.bilid == up.billid).ToList();
                        for (int k = 0; k < detail.Count; k++)
                        {
                            var sp = data.Products.Where(m => m.Id == detail[k].proid).FirstOrDefault();
                            sp.Count = (sp.Count - detail[k].quantity);
                            data.SubmitChanges();
                        }
                        up.state = 2;
                    }
                    else
                    {
                        up.state = 1;
                    }
                    data.SubmitChanges();
                }
                else
                {
                    Response.Write("alert('Đơn hàng này đã bị hủy')");
                }
                return RedirectToAction("tbBill_customersIndex");
            }
            else
            {
                return Redirect("/Admins/admins");
            }
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
        #region[MultiDelete]
        [HttpPost]
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
                            var Del = (from emp in data.tbBill_customers where emp.billid == id select emp).SingleOrDefault();
                            data.tbBill_customers.DeleteOnSubmit(Del);
                            data.SubmitChanges();
                        }
                    }
                }
                return RedirectToAction("tbBill_customersIndex");
            }
            else
            {
                return Redirect("/Admins/admins");
            }
        }
        #endregion
    }
}
