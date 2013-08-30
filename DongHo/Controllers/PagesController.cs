using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DongHo.Models;
using System.Web.Security;
using System.Net.Mail;
using System.Globalization;
using DongHo.ViewModels;

namespace DongHo.Controllers
{
    public class PagesController : Controller
    {
        //
        // GET: /Pages/
        DataDataContext data = new DataDataContext();
        #region[Dang ki]
        public ActionResult dangki()
        {
            return View();
        }
        #endregion
        #region[Dang ki]
        [HttpPost]
        public ActionResult dangki(FormCollection collect, tbCUSTOMER cus)
        {
            var Name = collect["Name"];
            var Pass = collect["Pass"];
            var Email = collect["Email"];
            var Phone = collect["Phone"];
            var Captcha = collect["Captcha"];
            var pro = data.Products.Where(m => m.Index == 1 && m.Active == 1).OrderByDescending(m => m.Id).Take(3).ToList();
            string sptt = "";
            sptt += "<ul>";
            for (int i = 0; i < pro.Count; i++)
            {
                sptt += "<li>";
                sptt += "<h3><a href=\"/sanpham/chitiet/" + pro[i].Tag + "\">" + pro[i].Name + "</a></h3>";
                sptt += "<a href=\"/sanpham/chitiet/" + pro[i].Tag + "\"><img src=\"" + pro[i].Image + "\" /></a>";
                sptt += "<p>Model: <span>" + pro[i].Id + "</span></p>";
                if (pro[i].PiceOld == "0")
                {
                    sptt += "<p>Giá: <span class=\"require\">" + StringClass.Format_Price(pro[i].Price.ToString()) + " VNĐ</span></p>";
                    sptt += "<p class=\"txtout\">" + StringClass.Format_Price(pro[i].PiceOld.ToString()) + " VNĐ</p>";
                }
                else
                {
                    sptt += "<p>Giá: <span class=\"require\">" + StringClass.Format_Price(pro[i].PiceOld.ToString()) + " VNĐ</span></p>";
                    sptt += "<p class=\"txtout\">" + StringClass.Format_Price(pro[i].Price.ToString()) + " VNĐ</p>";
                }
                sptt += "</li>";
            }
            sptt += "</ul>";
            ViewBag.Spnb = sptt;
            CaptchaProvider captchaPro = new CaptchaProvider();
            if (Name == "" || Pass == "" || Email == "" || Phone == "" || Captcha == "")
            {
                ViewBag.Err = "<p class='pErr'>Bạn phải điền đầy đủ thông tin đăng ký</p>";
                return View();
            }
            else if (!captchaPro.IsValidCode(Captcha))
            {
                ViewBag.Err = "<p class='pErr'>Mã an toàn chưa đúng</p>";
                return View();
            }
            else
            {
                var list = data.tbCUSTOMERs.Where(m => m.vemail == Email).ToList();
                if (list.Count == 0)
                {
                    cus.vcusname = collect["Name"];
                    cus.vpassword = StringClass.Encrypt(collect["Pass"]);
                    cus.vemail = collect["Email"];
                    cus.vphone = collect["Phone"];
                    cus.vaddress = collect["Address"];
                    cus.vusername = "KH" + DateTime.Now.Year + DateTime.Now.Month + DateTime.Now.Day + DateTime.Now.Hour + DateTime.Now.Minute + DateTime.Now.Millisecond;
                    cus.istatus = 0;
                    cus.dcreatedate = DateTime.Now;
                    data.tbCUSTOMERs.InsertOnSubmit(cus);
                    data.SubmitChanges();
                    var listMem = data.tbCUSTOMERs.OrderByDescending(m => m.iusid).ToList();
                    string chuoi = "";
                    chuoi += "<div style=\"margin:20px auto; padding:20px;\">";
                    chuoi += "<h3>Xác nhận đăng kí thành viên trên DongHo</h3>";
                    chuoi += "<p>Bạn vừa đăng kí thành viên trên DongHo.</p>";
                    chuoi += "<p>Vui lòng click vào <a href=\"http://dongho.hoccungdoanhnghiep.vn/Pages/xacnhan/" + listMem[0].iusid + "\">đây</a> để hoàn tất đăng kí</p>";
                    chuoi += "<p>Nếu không phải bạn đã đăng kí vui lòng click vào <a href=\"http://dongho.hoccungdoanhnghiep.vn/Pages/huy/" + listMem[0].iusid + "\">đây</a></p>";
                    chuoi += "<p>Xin cảm ơn!</p>";
                    #region [Sendmail]
                    System.Net.Mail.MailMessage mailMessage = new System.Net.Mail.MailMessage();
                    string mailto = listMem[0].vemail;
                    var listconfig = data.Configs.ToList();
                    string pass = listconfig[0].Mail_Password;
                    string host = listconfig[0].Mail_Smtp;
                    int post = 0;
                    if (Convert.ToInt32(listconfig[0].Mail_Port) > 0)
                    {
                        post = Convert.ToInt32(listconfig[0].Mail_Port);
                    }
                    else
                    {
                        post = 587;
                    }
                    mailMessage.From = (new MailAddress(mailto, "no-reply", System.Text.Encoding.UTF8));
                    mailMessage.To.Add(mailto);
                    mailMessage.Subject = "Yeu cau kich hoat tai khoan tai f1mua.com";
                    mailMessage.SubjectEncoding = System.Text.Encoding.UTF8;
                    mailMessage.Body = chuoi;
                    mailMessage.IsBodyHtml = true;
                    mailMessage.BodyEncoding = System.Text.Encoding.UTF8;
                    System.Net.NetworkCredential mailAuthentication = new System.Net.NetworkCredential();
                    if (listconfig[0].Mail_Noreply == null || listconfig[0].Mail_Noreply == "" || listconfig[0].Mail_Password == null || listconfig[0].Mail_Password == "")
                    {
                        mailAuthentication.UserName = "chikn92@gmail.com";
                        mailAuthentication.Password = "G0odLuck";
                    }
                    else
                    {
                        mailAuthentication.UserName = listconfig[0].Mail_Noreply;
                        mailAuthentication.Password = listconfig[0].Mail_Password;
                    }
                    System.Net.Mail.SmtpClient mailClient = new SmtpClient("smtp.gmail.com", 587);
                    mailClient.EnableSsl = true;
                    mailClient.UseDefaultCredentials = false;
                    mailClient.Credentials = mailAuthentication;
                    try
                    {
                        mailClient.Send(mailMessage);
                        Session.Clear();
                        list.Clear();
                        list = null;
                    }
                    catch (Exception ex)
                    {
                        //Raovat.Models.StringClass.Show("Đã có lỗi xảy ra trong quá trình gửi mail");
                    }
                    #endregion
                    return Redirect("/Home/Index");
                }
                else
                {
                    ViewBag.Err = "<p class='pErr'>Email này đã tồn tại trong hệ thống!</p>";
                    return View();
                }
            }
        }
        #endregion
        #region[Hoan tat dang ki]
        public ActionResult hoantat()
        {
            return View();
        }
        #endregion
        #region[xac nhan dang ki]
        public ActionResult xacnhan()
        {
            string id = "";
            if (RouteData.Values["id"] != null)
            {
                id = RouteData.Values["id"].ToString();
            }
            var list = data.tbCUSTOMERs.Where(m => m.iusid == Convert.ToInt32(id)).FirstOrDefault();
            if (list != null)
            {
                list.istatus = 1;
                data.SubmitChanges();
            }
            var pro = data.Products.Where(m => m.Index == 1 && m.Active == 1).OrderByDescending(m => m.Id).Take(3).ToList();
            string sptt = "";
            sptt += "<ul>";
            for (int i = 0; i < pro.Count; i++)
            {
                sptt += "<li>";
                sptt += "<h3><a href=\"/sanpham/chitiet/" + pro[i].Tag + "\">" + pro[i].Name + "</a></h3>";
                sptt += "<a href=\"/sanpham/chitiet/" + pro[i].Tag + "\"><img src=\"" + pro[i].Image + "\" /></a>";
                sptt += "<p>Model: <span>" + pro[i].Id + "</span></p>";
                if (pro[i].PiceOld == "0")
                {
                    sptt += "<p>Giá: <span class=\"require\">" + StringClass.Format_Price(pro[i].Price.ToString()) + " VNĐ</span></p>";
                    sptt += "<p class=\"txtout\">" + StringClass.Format_Price(pro[i].PiceOld.ToString()) + " VNĐ</p>";
                }
                else
                {
                    sptt += "<p>Giá: <span class=\"require\">" + StringClass.Format_Price(pro[i].PiceOld.ToString()) + " VNĐ</span></p>";
                    sptt += "<p class=\"txtout\">" + StringClass.Format_Price(pro[i].Price.ToString()) + " VNĐ</p>";
                }
                sptt += "</li>";
            }
            sptt += "</ul>";
            ViewBag.Spnb = sptt;
            return View();
        }
        #endregion
        #region[huy dang ky]
        public ActionResult huy()
        {
            string id = "";
            if (RouteData.Values["id"] != null)
            {
                id = RouteData.Values["id"].ToString();
            }
            var list = data.tbCUSTOMERs.Where(m => m.iusid == Convert.ToInt32(id)).FirstOrDefault();
            if (list != null)
            {
                data.tbCUSTOMERs.DeleteOnSubmit(list);
                data.SubmitChanges();
            }
            var pro = data.Products.Where(m => m.Index == 1 && m.Active == 1).OrderByDescending(m => m.Id).Take(3).ToList();
            string sptt = "";
            sptt += "<ul>";
            for (int i = 0; i < pro.Count; i++)
            {
                sptt += "<li>";
                sptt += "<h3><a href=\"/sanpham/chitiet/" + pro[i].Tag + "\">" + pro[i].Name + "</a></h3>";
                sptt += "<a href=\"/sanpham/chitiet/" + pro[i].Tag + "\"><img src=\"" + pro[i].Image + "\" /></a>";
                sptt += "<p>Model: <span>" + pro[i].Id + "</span></p>";
                if (pro[i].PiceOld == "0")
                {
                    sptt += "<p>Giá: <span class=\"require\">" + StringClass.Format_Price(pro[i].Price.ToString()) + " VNĐ</span></p>";
                    sptt += "<p class=\"txtout\">" + StringClass.Format_Price(pro[i].PiceOld.ToString()) + " VNĐ</p>";
                }
                else
                {
                    sptt += "<p>Giá: <span class=\"require\">" + StringClass.Format_Price(pro[i].PiceOld.ToString()) + " VNĐ</span></p>";
                    sptt += "<p class=\"txtout\">" + StringClass.Format_Price(pro[i].Price.ToString()) + " VNĐ</p>";
                }
                sptt += "</li>";
            }
            sptt += "</ul>";
            ViewBag.Spnb = sptt;
            return View();
        }
        #endregion
        #region[Dang nhap]
        public ActionResult dangnhap()
        {
            Session["Email"] = null;
            var pro = data.Products.Where(m => m.Index == 1 && m.Active == 1).OrderByDescending(m => m.Id).Take(3).ToList();
            string sptt = "";
            sptt += "<ul>";
            for (int i = 0; i < pro.Count; i++)
            {
                sptt += "<li>";
                sptt += "<h3><a href=\"/sanpham/chitiet/" + pro[i].Tag + "\">" + pro[i].Name + "</a></h3>";
                sptt += "<a href=\"/sanpham/chitiet/" + pro[i].Tag + "\"><img src=\"" + pro[i].Image + "\" /></a>";
                sptt += "<p>Model: <span>" + pro[i].Id + "</span></p>";
                if (pro[i].PiceOld == "0")
                {
                    sptt += "<p>Giá: <span class=\"require\">" + StringClass.Format_Price(pro[i].Price.ToString()) + " VNĐ</span></p>";
                    sptt += "<p class=\"txtout\">" + StringClass.Format_Price(pro[i].PiceOld.ToString()) + " VNĐ</p>";
                }
                else
                {
                    sptt += "<p>Giá: <span class=\"require\">" + StringClass.Format_Price(pro[i].PiceOld.ToString()) + " VNĐ</span></p>";
                    sptt += "<p class=\"txtout\">" + StringClass.Format_Price(pro[i].Price.ToString()) + " VNĐ</p>";
                }
                sptt += "</li>";
            }
            sptt += "</ul>";
            ViewBag.Spnb = sptt;
            return View();
        }
        #endregion
        #region[Dang nhap]
        [HttpPost]
        public ActionResult dangnhap(FormCollection collect)
        {
            var Email = collect["Email"];
            var Pass = collect["Pass"];
            var list = data.tbCUSTOMERs.Where(m => m.vemail == Email && m.vpassword == StringClass.Encrypt(Pass)).ToList();
            if (list.Count > 0)
            {
                FormsAuthentication.SetAuthCookie(Email, false);
                Session["Email"] = list[0].vemail;
                return Redirect("/Home/Index");
            }
            else
            {
                ViewBag.Err = "Đăng nhập không thành công!";
                return View();
            }
        }
        #endregion
        #region[hien thi box san pham noi bat ben phai]
        public ActionResult _boxspNoibat()
        {
            var pro = data.Products.Where(m => m.Index == 1 && m.Active == 1).OrderByDescending(m => m.Id).Take(3).ToList();
            string sptt = "";
            sptt += "<ul>";
            for (int i = 0; i < pro.Count; i++)
            {
                var proImg = data.ProImages.Where(m => m.ProId == pro[i].Id).ToList();
                string k = "";
                for (int j = 0; j < proImg.Count; j++)
                {
                    var a = proImg[j].Image.IndexOf("_noz");
                    if (a > -1)
                    {
                        k = proImg[j].Image;
                        break;
                    }
                }
                sptt += "<li>";
                sptt += "<h3><a href=\"/sanpham/chitiet/" + pro[i].Tag + "\">" + pro[i].Name + "</a></h3>";
                sptt += "<a href=\"/sanpham/chitiet/" + pro[i].Tag + "\"><img src=\"" + k + "\" /></a>";
                sptt += "<p>Model: <span>" + pro[i].Id + "</span></p>";
                if (pro[i].PiceOld == "0")
                {
                    sptt += "<p>Giá: <span class=\"require\">" + StringClass.Format_Price(pro[i].Price.ToString()) + " VNĐ</span></p>";
                    sptt += "<p class=\"txtout\">" + StringClass.Format_Price(pro[i].PiceOld.ToString()) + " VNĐ</p>";
                }
                else
                {
                    sptt += "<p>Giá: <span class=\"require\">" + StringClass.Format_Price(pro[i].PiceOld.ToString()) + " VNĐ</span></p>";
                    sptt += "<p class=\"txtout\">" + StringClass.Format_Price(pro[i].Price.ToString()) + " VNĐ</p>";
                }
                sptt += "</li>";
            }
            sptt += "</ul>";
            ViewBag.Spnb = sptt;
            return PartialView();
        }
        #endregion
        #region[thong tin ca nhan]
        public ActionResult member()
        {
            if (Session["Email"] != null)
            {
                Session["colFunc"] = 1;
                var list = data.tbCUSTOMERs.Where(m => m.vemail == Session["Email"].ToString()).FirstOrDefault();
                ViewBag.Name = list.vcusname;
                string chuoi = "";
                chuoi += "<h3>Thông tin tài khoản</h3>";
                chuoi += "<p>Họ và tên: " + list.vcusname + "</p>";
                chuoi += "<p>Email: " + list.vemail + "</p>";
                chuoi += "<p>Địa chỉ: " + list.vaddress + "</p>";
                chuoi += "<p>Điện thoại: " + list.vphone + "</p>";
                ViewBag.Info = chuoi;
                string str = "";
                var bill = data.tbBill_customers.Where(m => m.userid == list.iusid).OrderByDescending(m => m.billid).Take(3).ToList();
                for (int i = 0; i < bill.Count; i++)
                {
                    str += "<tr>";
                    str += "<td>" + (i + 1) + "</td>";
                    str += "<td>" + bill[i].billid + "</td>";
                    str += "<td>" + DateTimeClass.ConvertDateTimeddMMyyyy(bill[i].idate.ToString()) + "</td>";
                    str += "<td>" + list.vcusname + "</td>";
                    str += "<td>" + StringClass.Format_Price(bill[i].totalmoney) + " VNĐ</td>";
                    str += "<td>" + StringClass.ShowStateBill(bill[i].state.ToString()) + "</td>";
                    str += "<td><a href=\"/Pages/chitiet\">Chi tiết</a></td>";
                    str += "</tr>";
                }
                ViewBag.View = str;
                return View();
            }
            else
            {
                return RedirectToAction("dangnhap");
            }
        }
        #endregion
        #region[hien thi cot chuc nang ben trai trong trang ca nhan cua khach hang]
        public ActionResult colFunctionCus()
        {
            string chuoi = "";
            if (Session["colFunc"].ToString() == "1")
            {
                chuoi += "<li class=\"current\"><a href=\"/Pages/member\">Thông tin tài khoản chung</a></li>";
                chuoi += "<li><a href=\"/Pages/edit_info\">Chỉnh sửa thông tin</a></li>";
                chuoi += "<li><a href=\"/Pages/doi_mat_khau\">Đổi mật khẩu</a></li>";
                chuoi += "<li><a href=\"/Pages/quan_ly_don_hang\">Quản lý đơn hàng</a></li>";
            }
            else if (Session["colFunc"].ToString() == "2")
            {
                chuoi += "<li><a href=\"/Pages/member\">Thông tin tài khoản chung</a></li>";
                chuoi += "<li class=\"current\"><a href=\"/Pages/edit_info\">Chỉnh sửa thông tin</a></li>";
                chuoi += "<li><a href=\"/Pages/doi_mat_khau\">Đổi mật khẩu</a></li>";
                chuoi += "<li><a href=\"/Pages/quan_ly_don_hang\">Quản lý đơn hàng</a></li>";
            }
            else if (Session["colFunc"].ToString() == "3")
            {
                chuoi += "<li><a href=\"/Pages/member\">Thông tin tài khoản chung</a></li>";
                chuoi += "<li><a href=\"/Pages/edit_info\">Chỉnh sửa thông tin</a></li>";
                chuoi += "<li class=\"current\"><a href=\"/Pages/doi_mat_khau\">Đổi mật khẩu</a></li>";
                chuoi += "<li><a href=\"/Pages/quan_ly_don_hang\">Quản lý đơn hàng</a></li>";
            }
            else
            {
                chuoi += "<li><a href=\"/Pages/member\">Thông tin tài khoản chung</a></li>";
                chuoi += "<li><a href=\"/Pages/edit_info\">Chỉnh sửa thông tin</a></li>";
                chuoi += "<li><a href=\"/Pages/doi_mat_khau\">Đổi mật khẩu</a></li>";
                chuoi += "<li class=\"current\"><a href=\"/Pages/quan_ly_don_hang\">Quản lý đơn hàng</a></li>";
            }
            ViewBag.View = chuoi;
            return PartialView();
        }
        #endregion
        #region[Chinh sua thong tin ca nhan]
        public ActionResult edit_info()
        {
            tbCUSTOMER list = new tbCUSTOMER();
            if (Session["Email"] != null)
            {
                Session["colFunc"] = 2;
                list = data.tbCUSTOMERs.Where(m => m.vemail == Session["Email"].ToString()).FirstOrDefault();
                return View(list);
            }
            else
            {
                return RedirectToAction("dangnhap");
            }
        }
        #endregion
        #region[Chinh sua thong tin ca nhan]
        [HttpPost]
        public ActionResult edit_info(FormCollection collect)
        {
            if (Session["Email"] != null)
            {
                var list = data.tbCUSTOMERs.Where(m => m.vemail == Session["Email"].ToString()).FirstOrDefault();
                var edit = data.tbCUSTOMERs.First(m => m.iusid == list.iusid);
                edit.vcusname = collect["vcusname"];
                edit.vphone = collect["vphone"];
                edit.vaddress = collect["vaddress"];
                data.SubmitChanges();
            }
            return RedirectToAction("member");
        }
        #endregion
        #region[Doi mat khau]
        public ActionResult doi_mat_khau()
        {
            if (Session["Email"] != null)
            {
                Session["colFunc"] = 3;
                return View();
            }
            else
            {
                return RedirectToAction("dangnhap");
            }
        }
        #endregion
        #region[Doi mat khau]
        [HttpPost]
        public ActionResult doi_mat_khau(FormCollection collect)
        {
            if (Session["Email"] != null)
            {
                var list = data.tbCUSTOMERs.Where(m => m.vemail == Session["Email"].ToString()).FirstOrDefault();
                if (list != null)
                {
                    if (list.vpassword == StringClass.Encrypt(collect["Pass"]))
                    {
                        list.vpassword = StringClass.Encrypt(collect["NewPass"]);
                        data.SubmitChanges();
                        return RedirectToAction("member");
                    }
                    else
                    {
                        ViewBag.Err = "Mật khẩu chưa đúng";
                    }
                }
                return View();
            }
            else
            {
                return RedirectToAction("dangnhap");
            }
        }
        #endregion
        #region[Quen mat khau]
        public ActionResult forget_pass()
        {
            return View();
        }
        #endregion
        #region[Quen mat khau]
        [HttpPost]
        public ActionResult forget_pass(FormCollection collect)
        {
            string chuoi = "";
            var email = collect["Email"];
            if (email == "")
            {
                ViewBag.Err = "<div class=\"err\">Bạn phải nhập email.</div>";
            }
            else
            {
                var list = data.tbCUSTOMERs.Where(m => m.vemail == email && m.vpassword!=null).FirstOrDefault();
                if (list != null)
                {
                    var Pass = StringClass.RandomString(8);
                    list.vpassword = StringClass.Encrypt(Pass);
                    data.SubmitChanges();
                    chuoi += "<p>Bạn nhận được mail này vì bạn đã thực hiện chức năng quên mật khẩu trên DongHo</p>";
                    chuoi += "<p>Đây là thông tin đăng nhập mới của bạn:</p>";
                    chuoi += "<p>Email: " + list.vemail + "</p>";
                    chuoi += "<p>Mật khẩu: " + Pass + "</p>";
                    chuoi += "<p>Cảm ơn bạn đã sử dụng DongHo</p>";
                    #region [Sendmail]
                    System.Net.Mail.MailMessage mailMessage = new System.Net.Mail.MailMessage();
                    string mailto = list.vemail;
                    var listconfig = data.Configs.ToList();
                    string pass = listconfig[0].Mail_Password;
                    string host = listconfig[0].Mail_Smtp;
                    int post = 0;
                    if (Convert.ToInt32(listconfig[0].Mail_Port) > 0)
                    {
                        post = Convert.ToInt32(listconfig[0].Mail_Port);
                    }
                    else
                    {
                        post = 587;
                    }
                    mailMessage.From = (new MailAddress(mailto, "no-reply", System.Text.Encoding.UTF8));
                    mailMessage.To.Add(list.vemail);
                    mailMessage.Subject = "Thong tin dang nhap moi tai DongHo";
                    mailMessage.SubjectEncoding = System.Text.Encoding.UTF8;
                    mailMessage.Body = chuoi;
                    mailMessage.IsBodyHtml = true;
                    mailMessage.BodyEncoding = System.Text.Encoding.UTF8;
                    System.Net.NetworkCredential mailAuthentication = new System.Net.NetworkCredential();
                    if (listconfig[0].Mail_Noreply == null || listconfig[0].Mail_Noreply == "" || listconfig[0].Mail_Password == null || listconfig[0].Mail_Password == "")
                    {
                        mailAuthentication.UserName = "chikn92@gmail.com";
                        mailAuthentication.Password = "G0odLuck";
                    }
                    else
                    {
                        mailAuthentication.UserName = listconfig[0].Mail_Noreply;
                        mailAuthentication.Password = listconfig[0].Mail_Password;
                    }
                    System.Net.Mail.SmtpClient mailClient = new SmtpClient("smtp.gmail.com", 587);
                    mailClient.EnableSsl = true;
                    mailClient.UseDefaultCredentials = false;
                    mailClient.Credentials = mailAuthentication;
                    try
                    {
                        mailClient.Send(mailMessage);
                        Session.Clear();
                        list = null;
                        Response.Redirect("/Home/Index");
                    }
                    catch (Exception ex)
                    {
                        //Raovat.Models.StringClass.Show("Đã có lỗi xảy ra trong quá trình gửi mail");
                    }
                    #endregion
                }
                else
                {
                    ViewBag.Err = "<div class=\"err\">Email này không tồn tại trong hệ thống.</div>";
                }
            }
            return View();
        }
        #endregion
        #region[Quan ly don hang]
        public ActionResult quan_ly_don_hang()
        {
            if (Session["Email"] != null)
            {
                Session["colFunc"] = 4;
                string chuoi = "";
                var list = data.tbCUSTOMERs.Where(m => m.vemail == Session["Email"].ToString()).FirstOrDefault();
                var bill = data.tbBill_customers.Where(m => m.userid == list.iusid).ToList();
                for (int i = 0; i < bill.Count; i++)
                {
                    chuoi += "<tr>";
                    chuoi += "<td>" + (i + 1) + "</td>";
                    chuoi += "<td>" + bill[i].billid + "</td>";
                    chuoi += "<td>" + DateTimeClass.ConvertDateTimeddMMyyyy(bill[i].idate.ToString()) + "</td>";
                    chuoi += "<td>" + list.vcusname + "</td>";
                    chuoi += "<td>" + StringClass.Format_Price(bill[i].totalmoney) + " VNĐ</td>";
                    chuoi += "<td>" + StringClass.ShowStateBill(bill[i].state.ToString()) + "</td>";
                    chuoi += "<td><a href=\"/Pages/chitiet\">Chi tiết</a></td>";
                    chuoi += "</tr>";
                }
                ViewBag.View = chuoi;
                return View();
            }
            else
            {
                return RedirectToAction("dangnhap");
            }
        }
        #endregion
        #region[Tim kiem]
        public ActionResult timkiem()
        {
            #region[khoi tao bien]
            string str = "";
            string searchC = "";
            string chuoi = "";
            string chuoiList = "";
            string groupFunc = "";
            string chuoiPrice = "";
            string sr = Request["searchword"];
            Session["Timkiem"] = sr;
            string srC = Request["c"];
            string order = Request["order"];
            string pri = Request["pr"];
            if (order != null)
            {
                if (order == "price_up") { ddlvalue = " Price asc"; }
                else if (order == "price_down") { ddlvalue = " Price desc"; }
                else if (order == "date_up") { ddlvalue = " [Date] asc"; }
                else if (order == "date_down") { ddlvalue = " [Date] desc"; }
            }
            string page = "1";//so phan trang hien tai
            var pagesize = "1";//so ban ghi tren 1 trang
            var numOfNews = 0;//tong so ban ghi co duoc truoc khi phan trang
            int curpage = 0; // trang hien tai dung cho phan trang
            if (Request["page"] != null)
            {
                page = Request["page"];
                curpage = Convert.ToInt32(page) - 1;
            }
            #endregion
            #region[tao cau lenh tim kiem ]
            string[] array = sr.Split(' ');
            if (array.Length == 1)
            {
                string patternSearch = RemoveUnicode(array[0].Replace(" ", "%%"), true);
                str += " Product.Name like N'%" + patternSearch + "%'";
            }
            else
            {
                for (int i = 0; i < array.Length; i++)
                {
                    string patternSearch = RemoveUnicode(array[i].Replace(" ", "%%"), true);
                    if (i == 0)
                    {
                        str += "(Product.Name like N'%" + patternSearch + "%'";
                    }
                    else if (i == (array.Length - 1))
                    {
                        str += " or Product.Name like N'%" + patternSearch + "%')";
                    }
                    else
                    {
                        str += " or Product.Name like N'%" + patternSearch + "%'";
                    }
                }
            }
            #endregion
            var searchCat = ViewModels.SearchCatBusiness.Product_SearchByCat(str).ToList();
            var seachWord = sr.Replace(" ", "+");
            #region[Hien thi tim kiem theo danh muc - Category]
            searchC += "<ul>";
            for (int i = 0; i < searchCat.Count; i++)
            {
                if (pri != null)
                {
                    searchC += "<li><a href=\"/Pages/timkiem?searchword=" + seachWord + "&c=" + searchCat[i].Id + "&pr=" + pri + "\">" + searchCat[i].Name + " (" + searchCat[i].NumberOfCat + ")</a></li>";
                }
                else
                {
                    searchC += "<li><a href=\"/Pages/timkiem?searchword=" + seachWord + "&c=" + searchCat[i].Id + "\">" + searchCat[i].Name + " (" + searchCat[i].NumberOfCat + ")</a></li>";
                }
            }
            searchC += "</ul>";
            ViewBag.searchCat = searchC;
            #endregion
            #region[Hien thi tim kiem theo gia - Prices]
            var price = data.Prices.ToList();
            chuoiPrice += "<ul>";
            for (int i = 0; i < price.Count; i++)
            {
                if (pri == null)
                {
                    if (srC != null)
                    {
                        chuoiPrice += "<li><a href=\"/Pages/timkiem?searchword=" + seachWord + "&c=" + srC + "&pr=" + price[i].Id + "\">" + price[i].Name + "</a></li>";
                    }
                    else
                    {
                        chuoiPrice += "<li><a href=\"/Pages/timkiem?searchword=" + seachWord + "&pr=" + price[i].Id + "\">" + price[i].Name + "</a></li>";
                    }
                }
                else
                {
                    if (price[i].Id != int.Parse(pri))
                    {
                        if (srC != null)
                        {
                            chuoiPrice += "<li><a href=\"/Pages/timkiem?searchword=" + seachWord + "&c=" + srC + "&pr=" + price[i].Id + "\">" + price[i].Name + "</a></li>";
                        }
                        else
                        {
                            chuoiPrice += "<li><a href=\"/Pages/timkiem?searchword=" + seachWord + "&pr=" + price[i].Id + "\">" + price[i].Name + "</a></li>";
                        }
                    }
                    else
                    {
                        if (srC != null)
                        {
                            chuoiPrice += "<li><a href=\"/Pages/timkiem?searchword=" + seachWord + "&c=" + srC + "&pr=" + price[i].Id + "\" class='check'>" + price[i].Name + "</a></li>";
                        }
                        else
                        {
                            chuoiPrice += "<li><a href=\"/Pages/timkiem?searchword=" + seachWord + "&pr=" + price[i].Id + "\" class='check'>" + price[i].Name + "</a></li>";
                        }
                    }
                }
            }
            ViewBag.searchPrice = chuoiPrice;
            chuoiPrice += "</ul>";
            #endregion
            if (srC != null)
            {
                str += "and CatId = " + srC + "";
            }
            if (pri != null)
            {
                var p = data.Prices.Where(m => m.Id == int.Parse(pri)).FirstOrDefault();
                if (p.PriceTo != 0)
                {
                    str += "and Price > " + p.PriceFrom + " and Price < " + p.PriceTo + "";
                }
                else
                {
                    str += "and Price > " + p.PriceFrom + "";
                }
            }
            var cmdText1 = data.sp_Product_Search(str, ddlvalue).ToList();
            var pages = data.sp_Product_Phantrang(page, pagesize, str, ddlvalue).ToList();
            numOfNews = cmdText1.Count;
            var url = Request.Url.PathAndQuery;
            var a = url.LastIndexOf("&page=");
            string k = "";
            if (a > 0)
            {
                k = url.Substring(0, a);
            }
            else
            {
                k = url;
            }
            ViewBag.Pager = DongHo.Models.PhantrangQuery.PhanTrangQuery(1, curpage, numOfNews, k);
            #region[Hien thi ket qua tim kiem - phan sp]
            if (sr != null)
            {
                chuoiList += "<ul class='box-pro'>";
                foreach (var item in pages)
                {
                    chuoi += "<div class=\"div-pro\">";
                    chuoi += "<a href=\"/sanpham/chitiet/" + item.Tag + "\">";
                    chuoi += "<img src=\"" + item.Image + "\" />";
                    chuoi += "<div class=\"titlePro\">";
                    chuoi += "<p>" + FormatContentNews(item.Name,50) + "</p>";
                    //if (item.PiceOld == "0")
                    //{
                        chuoi += "<span class=\"require\">" + StringClass.Format_Price(item.Price.ToString()) + " VNĐ</span>";
                        chuoi += "<span class=\"oldPrice\">" + StringClass.Format_Price(item.PiceOld.ToString()) + " VNĐ</span>";
                    //}
                    //else
                    //{
                    //    chuoi += "<span class=\"require\">" + StringClass.Format_Price(item.PiceOld.ToString()) + " VNĐ</span>";
                    //    chuoi += "<span class=\"oldPrice\">" + StringClass.Format_Price(item.Price.ToString()) + " VNĐ</span>";
                    //}
                    chuoi += "<div class=\"hideTitlePro\">";
                    chuoi += "<span class=\"require\">Giảm giá " + item.Codepro + " %</span>";
                    chuoi += "<div style=\"padding:8px;\">" + item.Content + "</div>";
                    chuoi += "</div>";
                    chuoi += "</div>";
                    chuoi += "</a>";
                    chuoi += "</div>";
                    var brand = data.Brands.Where(m => m.Id == item.BrandId).ToList();
                    chuoiList += "<li class=\"clearfix\">";
                    chuoiList += "<a href=\"/sanpham/chitiet/" + item.Tag + "\"><img src=\"" + item.Image + "\" class=\"fl\"/></a>";
                    chuoiList += "<div class=\"listDetail fl\">";
                    chuoiList += "<a href=\"/sanpham/chitiet/" + item.Tag + "\">" + FormatContentNews(item.Name,50) + "</a>";
                    chuoiList += "<p>Thương hiệu: " + brand[0].Name + "</p>";
                    chuoiList += "<p>Tình trạng: còn hàng</p>";
                    chuoiList += "</div>";
                    chuoiList += "<div class=\"listprice fr\">";
                    //if (item.PiceOld == "0")
                    //{
                    //    chuoiList += "<p>Giá bán: <span>" + StringClass.Format_Price(item.Price.ToString()) + " VNĐ</span></p>";
                    //    chuoiList += "<p>Giá gốc: " + StringClass.Format_Price(item.PiceOld.ToString()) + " VNĐ</p>";
                    //}
                    //else
                    //{
                    chuoiList += "<p>Giá bán: <span>" + StringClass.Format_Price(item.Price.ToString()) + " VNĐ</span></p>";
                    chuoiList += "<p>Giá gốc: " + StringClass.Format_Price(item.PiceOld.ToString()) + " VNĐ</p>";
                    //}
                    chuoiList += "<a href=\"javascript:;\" class=\"btn-cart\"><span><b>&nbsp;</b>Mua ngay</span></a>";
                    chuoiList += "</div>";
                    chuoiList += "</li>";
                }
                chuoiList += "</ul>";
            }
            #endregion
            #region[Hien thi ket qua tim kiem - phan cac chuc nang]
            if (sr == "")
            {
                groupFunc += "<p class=\"ketquaSearh\">Có tổng cộng <span>0</span> kết quả phù hợp với từ khóa: <span></span></p>";
            }
            else
            {
                groupFunc += "<p class=\"ketquaSearh\">Có tổng cộng <span>" + cmdText1.Count + "</span> kết quả phù hợp với từ khóa: <span>\"" + sr + "\"</span></p>";
            }
            groupFunc += "<div class=\"boxFuncSearch\">";
            groupFunc += "<p>Hiển thị từ 13 - 25 trong số "+cmdText1.Count+" kết quả</p>";
            groupFunc += "<div>";
            groupFunc += "<p>Hiển thị theo</p>";
            groupFunc += "<a href=\"#\" class=\"iconGridView grid-view-click act\">&nbsp;</a>";
            groupFunc += "<a href=\"#\" class=\"iconListView list-view-click\">&nbsp;</a>";
            groupFunc += "</div>";
            groupFunc += "<form action='/Pages/timkiem' method='post'>";
            groupFunc += "<select id='sx' name='sx' onchange='this.form.submit()'>";
            groupFunc += "<option> ---- Sắp xếp ---- </option>";
            groupFunc += "<option value='price_up'> ---- Giá tăng dần ---- </option>";
            groupFunc += "<option value='price_down'> ---- Giá giảm dần ---- </option>";
            groupFunc += "<option value='date_up'> ---- Ngày đăng cũ dần ---- </option>";
            groupFunc += "<option value='date_down'> ---- Ngày đăng mới dần ---- </option>";
            groupFunc += "</select>";
            groupFunc += "</form>";
            groupFunc += "<div class=\"clear\"></div>";
            groupFunc += "</div>";
            ViewBag.groupFunc = groupFunc;
            #endregion
            if (chuoi == "" && chuoiList == "" || str=="")
            {
                ViewBag.viewGrid = "Không có kết quả!";
                ViewBag.viewList = "Không có kết quả!";
            }
            else
            {
                ViewBag.viewGrid = chuoi;
                ViewBag.viewList = chuoiList;
            }
            return View();
        }
        #endregion
        #region[timkiem]
        string ddlvalue = "";
        [HttpPost]
        public ActionResult timkiem(FormCollection collect)
        {
            string ddl = collect["sx"];
            var seachWord = Session["Timkiem"].ToString().Replace(" ", "+");
            string url = "/Pages/timkiem?searchword=" + seachWord + "&order=" + ddl + "";
            return Redirect(url);
        }
        #endregion
        #region[Xoa the html khoi text]
        private string RemoveHTMLTag(string HTML)
        {
            // Xóa các thẻ html
            System.Text.RegularExpressions.Regex objRegEx = new System.Text.RegularExpressions.Regex("<[^>]*>");

            return objRegEx.Replace(HTML, "");
        }
        #endregion
        #region[Cat chuoi text de hien thi]
        protected string FormatContentNews(string value, int count)
        {
            string _value = value;
            if (_value.Length >= count)
            {
                string ValueCut = _value.Substring(0, count - 3);
                string[] valuearray = ValueCut.Split(' ');
                string valuereturn = "";
                for (int i = 0; i < valuearray.Length - 1; i++)
                {
                    valuereturn = valuereturn + " " + valuearray[i];
                }
                return valuereturn;
            }
            else
            {
                return _value;
            }
        }
        #endregion
        #region[Them dau Tieng Viet doi voi tung chu]
        public string ReplaceCharSet(string input)
        {
            string charSet = input.ToLower();
            if (charSet == "a")
                return "[aàảãáạăằẳẵắặâầẩẫấậ]";
            else if (charSet == "e")
                return "[eèẻẽéẹêềểễếệ]";
            else if (charSet == "i")
                return "[iìỉĩíị]";
            else if (charSet == "o")
                return "[oòỏõóọôồổỗốộơờởỡớợ]";
            else if (charSet == "u")
                return "[uùủũúụưừửữứự]";
            else if (charSet == "y")
                return "[yỳỷỹýỵ]";
            else if (charSet == "d")
                return "[dđ]";
            return charSet;
        }
        #endregion
        #region[Loai bo dau Tieng Viet doi voi tung chu - chuyen doi chu tu Unicode sang ASCII]
        public string RemoveUnicode(string inputText, bool sqlSearch)
        {
            string stFormD = inputText.Normalize(System.Text.NormalizationForm.FormD);
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            string str = "";
            for (int i = 0; i <= stFormD.Length - 1; i++)
            {
                UnicodeCategory uc = CharUnicodeInfo.GetUnicodeCategory(stFormD[i]);
                if (uc == UnicodeCategory.NonSpacingMark == false)
                {
                    if (stFormD[i] == 'đ')
                        str = "d";
                    else if (stFormD[i] == 'Đ')
                        str = "D";
                    else
                        str = stFormD[i].ToString();
                    //Neu sqlSearch = true thi sau khi khu dau xong o tren se tien hanh them dau, con neu sqlSearch = false thi nguoc lai
                    if (sqlSearch) str = ReplaceCharSet(str);
                    sb.Append(str);
                }
            }
            return sb.ToString().ToLower();
        }
        #endregion
        #region[view chon lua dang nhap hoac mua hang luon k can tai khoan dk]
        public ActionResult notLogon()
        {
            return View();
        }
        #endregion
        #region[Thanh toan]
        public ActionResult order_pay()
        {
            string chuoi = "";
            string chuoiToTal = "";
            if (Session["ShoppingCart"] != null)
            {
                var cart = (ShoppingCartViewModel)Session["ShoppingCart"];
                chuoi += "<h3>Thông tin giỏ hàng</h3>";
                foreach (var item in cart.CartItems)
                {
                    chuoi += "<div class=\"infoPro\">";
                    chuoi += "<img src=\"" + item.productImage + "\" />";
                    chuoi += "<h4>" + item.productName + "</h4>";
                    chuoi += "<p>Giá bán: <span>" + StringClass.Format_Price(item.price) + " VNĐ</span></p>";
                    chuoi += "<p>Số lượng: <span>" + item.count + "</span></p>";
                    chuoi += "</div>";
                    chuoi += "<div class=\"clear\"></div>";
                }

                chuoiToTal += "<div class=\"totalCart\">";
                chuoiToTal += "<p class=\"fl\">Giá trị đơn hàng</p>";
                chuoiToTal += "<span class=\"fr\">" + StringClass.Format_Price(cart.CartTotal.ToString()) + " VNĐ</span>";
                chuoiToTal += "<div class=\"clear\"></div>";
                chuoiToTal += "<p class=\"fl\">Phí vận chuyển</p>";
                chuoiToTal += "<span class=\"fr\">0 VNĐ</span>";
                chuoiToTal += "<div class=\"clear\"></div>";
                chuoiToTal += "<p class=\"fl\">Tổng cộng</p>";
                chuoiToTal += "<span class=\"fr\">" + StringClass.Format_Price(cart.CartTotal.ToString()) + " VNĐ</span>";
                chuoiToTal += "<div class=\"clear\"></div>";
                chuoiToTal += "<input type=\"submit\" value=\"Xác nhận thanh toán\" />";
                chuoiToTal += "</div>";
                ViewBag.ViewPro = chuoi;
                ViewBag.ViewTotal = chuoiToTal;
                tbCUSTOMER mem = new tbCUSTOMER();
                if (Session["Email"] != null)
                {
                    mem = data.tbCUSTOMERs.Where(m => m.vemail == Session["Email"].ToString()).FirstOrDefault();
                    ViewBag.Cus = 1;
                }
                return View(mem);
            }
            else
            {
                return Redirect("/Home/Index");
            }
        }
        #endregion
        #region[Thanh toan]
        [HttpPost]
        public ActionResult order_pay(FormCollection collect)
        {
            string chuoi = "";
            var Name = collect["vcusname"];
            var Email = collect["vemail"];
            var Phone = collect["vphone"];
            var Address = collect["vaddress"];
            var Note = collect["Note"];
            var radioBtn = collect["rdbPay"];
            if (Name == "" || Email == "" || Phone == "" || Address == "")
            {
                ViewBag.Err = "<div class=\"err\">Bạn phải điền đầy đủ thông tin</div>";
                return View();
            }
            else
            {
                tbCUSTOMER listcus = new tbCUSTOMER();
                if (Session["Email"] == null)
                {
                    tbCUSTOMER cus = new tbCUSTOMER();
                    cus.vcusname = Name;
                    cus.vemail = Email;
                    cus.vphone = Phone;
                    cus.vaddress = Address;
                    data.tbCUSTOMERs.InsertOnSubmit(cus);
                    data.SubmitChanges();
                    listcus = data.tbCUSTOMERs.OrderByDescending(m => m.iusid).FirstOrDefault();
                }
                else
                {
                    listcus = data.tbCUSTOMERs.Where(m => m.vemail == Session["Email"].ToString()).FirstOrDefault();
                }
                ShoppingCartViewModel shoppCart = (ShoppingCartViewModel)Session["ShoppingCart"];
                tbBill_customer billcus = new tbBill_customer();
                billcus.userid = listcus.iusid;
                billcus.totalmoney = shoppCart.CartTotal.ToString();
                billcus.idate = DateTime.Now;
                billcus.request = Note;
                billcus.typepay = radioBtn;
                billcus.state = 0;
                data.tbBill_customers.InsertOnSubmit(billcus);
                data.SubmitChanges();
                #region[Noi dung mail gui]
                chuoi += "<div style=\" margin:20px auto; width:800px; background:#6badf6; padding-bottom:10px\">";
                chuoi += "<div style=\"border-bottom:1px solid #dfdfdf; margin:20px; line-height:30px; font-size:16px;\">";
                chuoi += "<a href=\"#\" style=\"float:left; font-size:20px; margin-top:20px\">dongho.com</a>";
                chuoi += "<p style=\"float:right; color:#525461; font-weight:bold\">Ngày "+ DateTime.Now.Day +" Tháng "+ DateTime.Now.Month +" Năm "+ DateTime.Now.Year +"</p>";
                chuoi += "<div style=\"clear:both;\"></div>";
                chuoi += "</div>";
                chuoi += "<div style=\"background:#fff; border-radius:15px 0 15px 0; padding:10px; margin:20px; overflow:hidden\">";
                chuoi += "<h2 style=\"padding-bottom:20px; border-bottom:1px solid #e8e8e8\">Cám ơn bạn đã mua sản phẩm của chúng tôi!</h2>";
                chuoi += "<h3 style=\"text-align:center; margin:10px 0; font-size:16px;\">Thông tin đơn hàng của bạn</h3>";
                chuoi += "<h4 style=\"margin:10px 0; font-size:14px;\">Thông tin người mua</h4>";
                chuoi += "<p style=\"line-height:20px;\"><span style=\"padding-right:10px; font-weight:bold;\">Họ và tên:</span> "+ listcus.vcusname +"</p>";
                chuoi += "<p style=\"line-height:20px\"><span style=\"padding-right:36px; font-weight:bold;\">SĐT:</span> "+ listcus.vphone +"</p>";
                chuoi += "<p style=\"line-height:20px\"><span style=\"padding-right:20px; font-weight:bold;\">Địa chỉ:</span> "+ listcus.vaddress +"</p>";
                chuoi += "<table width=\"100%\" style=\"border-collapse:collapse; margin:10px 0; border:1px solid #efefef; font-size:14px;\" border=\"1\">";
                chuoi += "<tr style=\" background:#91d9d9; border: 1px solid #efefef; height:30px;\">";
                chuoi += "<th style=\"width:40px; border:1px solid #efefef\">STT</th>";
                chuoi += "<th style=\"boder:1px solid #91d9d9; text-align:left; padding-left:5px;\">Tên sản phẩm</th>";
                chuoi += "<th style=\"boder:1px solid #91d9d9; width:80px;\">Số lượng</th>";
                chuoi += "<th style=\"boder:1px solid #91d9d9; width:130px;\">Đơn giá</th>";
                chuoi += "<th style=\"boder:1px solid #91d9d9; width:130px;\">Thành tiền</th>";
                chuoi += "</tr>";
                var listbillcus = data.tbBill_customers.OrderByDescending(m => m.billid).FirstOrDefault();
                int i = 1;
                foreach (var item in shoppCart.CartItems)
                {
                    tbBilldetail billdetail = new tbBilldetail();
                    billdetail.bilid = listbillcus.billid;
                    billdetail.proid = item.productId;
                    billdetail.quantity = item.count;
                    billdetail.bilprice = item.price;
                    billdetail.bilmoney = item.total.ToString();
                    billdetail.Date = DateTime.Now;
                    data.tbBilldetails.InsertOnSubmit(billdetail);
                    data.SubmitChanges();
                    var sp = data.Products.Where(m => m.Id == item.productId).FirstOrDefault();
                    sp.Count += item.count;
                    data.SubmitChanges();
                    chuoi += "<tr style=\"boder:1px solid #efefef; height:40px;\">";
                    chuoi += "<td style=\"boder:1px solid #efefef; width:30px; text-align:center\">"+ i +"</td>";
                    chuoi += "<td style=\"boder:1px solid #efefef; text-align:left; padding-left:5px;\">" + item.productName + "</td>";
                    chuoi += "<td style=\"boder:1px solid #efefef; width:80px; text-align:center\">"+ item.count +"</td>";
                    chuoi += "<td style=\"boder:1px solid #efefef; width:130px; text-align:center\">"+ StringClass.Format_Price(item.price) +" VNĐ</td>";
                    chuoi += "<td style=\"boder:1px solid #efefef; width:130px; text-align:center\">"+ StringClass.Format_Price(item.total.ToString()) +" VNĐ</td>";
                    chuoi += "</tr>";
                    i++;
                }
                chuoi += "<tr style=\"boder:1px solid #efefef; height:30px; background:#f2f5ee\">";
                chuoi += "<td colspan=\"5\" style=\"text-align:right; padding-right:18px; boder:1px solid #efefef\">"+ StringClass.Format_Price(shoppCart.CartTotal.ToString()) +" VNĐ</td>";
                chuoi += "</tr>";
                chuoi += "</table>";
                chuoi += "<hr style=\"color:#dfdfdf; margin:10px 0;\"/>";
                chuoi += "<p style=\"width:50%; float:left; margin:10px 0; color:#333333; font-size:15px; line-height:22px;\"><a href=\"\">dongho.com</a> hy vọng luôn mang lại nhiều niềm vui cho các bạn. Và lúc nào cũng mong đợi thông tin phản hồi từ các bạn. Hãy gọi cho chúng tôi qua</p>";
                chuoi += "<div style=\" float:right; width:244px; background:#ff6801; border-radius:10px; padding:5px; text-align:center;\">";
                chuoi += "<p style=\"color:#525461; font-size:14px; padding:5px 0; margin:0;\">Để biết thêm thông tin vui lòng gọi</p>";
                chuoi += "<h2 style=\"color:#f5f5f5; padding:5px 0; margin:0; text-shadow:1px 1px 3px #5c625e\">098.765.4321</h2>";
                chuoi += "</div>";
                chuoi += "<div style=\"clear:both;\"><p>   </p></div>";
                chuoi += "</div>";
                chuoi += "<div style=\"clear:both;\"><p>   </p></div>";
                chuoi += "</div>";
                #endregion
                #region [Sendmail]
                System.Net.Mail.MailMessage mailMessage = new System.Net.Mail.MailMessage();
                string mailto = listcus.vemail;
                var listconfig = data.Configs.ToList();
                string pass = listconfig[0].Mail_Password;
                string host = listconfig[0].Mail_Smtp;
                int post = 0;
                if (Convert.ToInt32(listconfig[0].Mail_Port) > 0)
                {
                    post = Convert.ToInt32(listconfig[0].Mail_Port);
                }
                else
                {
                    post = 587;
                }
                mailMessage.From = (new MailAddress(mailto, "no-reply", System.Text.Encoding.UTF8));
                mailMessage.To.Add(listcus.vemail);
                mailMessage.Bcc.Add(listconfig[0].Mail_Info);
                mailMessage.Subject = "Thong tin don hang tai DongHo";
                mailMessage.SubjectEncoding = System.Text.Encoding.UTF8;
                mailMessage.Body = chuoi;
                mailMessage.IsBodyHtml = true;
                mailMessage.BodyEncoding = System.Text.Encoding.UTF8;
                System.Net.NetworkCredential mailAuthentication = new System.Net.NetworkCredential();
                if (listconfig[0].Mail_Noreply == null || listconfig[0].Mail_Noreply == "" || listconfig[0].Mail_Password == null || listconfig[0].Mail_Password == "")
                {
                    mailAuthentication.UserName = "chikn92@gmail.com";
                    mailAuthentication.Password = "G0odLuck";
                }
                else
                {
                    mailAuthentication.UserName = listconfig[0].Mail_Noreply;
                    mailAuthentication.Password = listconfig[0].Mail_Password;
                }
                System.Net.Mail.SmtpClient mailClient = new SmtpClient("smtp.gmail.com", 587);
                mailClient.EnableSsl = true;
                mailClient.UseDefaultCredentials = false;
                mailClient.Credentials = mailAuthentication;
                try
                {
                    mailClient.Send(mailMessage);
                    Session.Clear();
                    listcus = null;
                    return RedirectToAction("order_success");
                }
                catch (Exception ex)
                {
                    return Redirect("/Home/Index");
                    //Raovat.Models.StringClass.Show("Đã có lỗi xảy ra trong quá trình gửi mail");
                }
                #endregion
            }
        }
        #endregion
        #region[dat hang thanh cong]
        public ActionResult order_success()
        {
            return View();
        }
        #endregion
    }
}
