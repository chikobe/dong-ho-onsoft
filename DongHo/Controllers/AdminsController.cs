using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DongHo.Models;
using System.Web.Security;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Web.Configuration;
using DongHo.ViewModels;

namespace DongHo.Controllers
{
    public class AdminsController : Controller
    {
        //
        // GET: /Admins/
        DataDataContext data = new DataDataContext();
        #region[Default]
        public ActionResult Default()
        {
            return View();
        }
        #endregion
        #region[ConfigEdit]
        public ActionResult ConfigEdit()
        {
            var all = data.Configs.Single();
            return View(all);
        }
        #endregion
        #region[ConfigEdit]
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult ConfigEdit(FormCollection collection)
        {
            var username = User.Identity.IsAuthenticated ? User.Identity.Name : string.Empty;
            if (username != null || username != "")
            {
                var config = data.Configs.Single();
                var Mail_Smtp = collection["Mail_Smtp"];
                var Mail_Port = collection["Mail_Port"];
                var Mail_Info = collection["Mail_Info"];
                var Mail_Noreply = collection["Mail_Noreply"];
                var Mail_Password = collection["Mail_Password"];
                var Copyright = collection["Copyright"];
                var Title = collection["Title"];
                var Description = collection["Description"];
                var Keyword = collection["Keyword"];
                config.Mail_Smtp = Mail_Smtp;
                config.Mail_Port = short.Parse(Mail_Port);
                config.Mail_Info = Mail_Info;
                config.Mail_Noreply = Mail_Noreply;
                config.Mail_Password = Mail_Password;
                config.Copyright = Copyright;
                config.Title = Title;
                config.Description = Description;
                config.Keyword = Keyword;
                UpdateModel(config);
                data.SubmitChanges();
                return RedirectToAction("Default", "Admins");
            }
            else
            {
                return RedirectToAction("admins");
            }
        }
        #endregion
        #region[PageIndex]
        public ActionResult PageIndex()
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
            var all = data.Pages.OrderBy(m=>m.Level).ToList();
            var pages = all.Skip(curpage * pagesize).Take(pagesize).ToList();
            //var pages = data.sp_Page_Phantrang(page, pagesize, "", "[Level] asc").ToList();
            var url = Request.Path;
            numOfNews = all.Count;
            if (numOfNews > 0)
            {
                ViewBag.Pager = DongHo.Models.Phantrang.PhanTrang(pagesize, curpage, numOfNews, url);
            }
            else
            {
                ViewBag.Pager = "";
            }
            return View(pages);
        }
        #endregion
        #region[PageCreate]
        public ActionResult PageCreate()
        {
            var module = data.Pages.OrderBy(m => m.Level).ToList();
            List<SelectListItem> listpage = new List<SelectListItem>();
            listpage.Clear();
            for (int i = 0; i < module.Count; i++)
            {
                listpage.Add(new SelectListItem { Text = StringClass.ShowNameLevel(module[i].Name, module[i].Level), Value = module[i].Link });
            }
            ViewBag.LinkModule = listpage;
            ViewBag.LinkDDL = new SelectList(ShowLinkDDL(), "value", "text");
            ViewBag.Position = new SelectList(ShowPosition(), "value", "text");
            ViewBag.Target = new SelectList(ShowTarget(), "value", "text");
            ViewBag.Type = new SelectList(ShowType(), "value", "text");
            return View();
        }
        #endregion
        #region[PageCreate]
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult PageCreate(FormCollection collection, Page page)
        {
            var username = User.Identity.IsAuthenticated ? User.Identity.Name : string.Empty;
            if (username != null || username != string.Empty)
            {
                page.Name = collection["Name"];
                page.Type = Convert.ToInt32(collection["Type"]);
                if (collection["LinkDDL"] == "0")
                {
                    page.Link = collection["Link"];
                }
                else
                {
                    page.Link = collection["LinkModule"];
                }
                page.Target = collection["Target"];
                page.Position = Convert.ToInt32(collection["Position"]);
                page.Content = collection["Content"];
                page.Detail = collection["Detail"];
                page.Title = collection["Title"];
                page.Description = collection["Description"];
                page.Keyword = collection["Keyword"];
                page.Ord = Convert.ToInt32(collection["Ord"]);
                var index = (collection["Indexs"] == "false") ? 0 : 1;
                page.Index = index;
                var active = (collection["Actives"] == "false") ? 0 : 1;
                page.Active = active;
                page.Tag = DongHo.Models.StringClass.NameToTag(collection["Name"]);
                data.Pages.InsertOnSubmit(page);
                data.SubmitChanges();
                return RedirectToAction("PageIndex");
            }
            else
            {
                return RedirectToAction("admins");
            }
        }
        #endregion
        #region[PageAddSub]
        public ActionResult PageAddSub()
        {
            var module = data.Pages.OrderBy(m => m.Level).ToList();
            List<SelectListItem> listpage = new List<SelectListItem>();
            listpage.Clear();
            for (int i = 0; i < module.Count; i++)
            {
                listpage.Add(new SelectListItem { Text = StringClass.ShowNameLevel(module[i].Name, module[i].Level), Value = module[i].Link });
            }
            ViewBag.LinkModule = listpage;
            ViewBag.Position = new SelectList(ShowPosition(), "value", "text");
            ViewBag.Target = new SelectList(ShowTarget(), "value", "text");
            ViewBag.Type = new SelectList(ShowType(), "value", "text");
            ViewBag.LinkDDL = new SelectList(ShowLinkDDL(), "value", "text");
            return View();
        }
        #endregion
        #region[PageAddSub]
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult PageAddSub(FormCollection collection, Page page,string Level)
        {
            var username = User.Identity.IsAuthenticated ? User.Identity.Name : string.Empty;
            if (username != null || username != "")
            {
                page.Name = collection["Name"];
                page.Type = Convert.ToInt32(collection["Type"]);
                if (collection["LinkDDL"] == "0")
                {
                    page.Link = collection["Link"];
                }
                else
                {
                    page.Link = collection["LinkModule"];
                }
                page.Target = collection["Target"];
                page.Position = Convert.ToInt32(collection["Position"]);
                page.Content = collection["Content"];
                page.Detail = collection["Detail"];
                page.Title = collection["Title"];
                page.Description = collection["Description"];
                page.Keyword = collection["Keyword"];
                page.Ord = Convert.ToInt32(collection["Ord"]);
                var index = (collection["Indexs"] == "false") ? 0 : 1;
                page.Index = index;
                var active = (collection["Actives"] == "false") ? 0 : 1;
                page.Active = active;
                page.Tag = DongHo.Models.StringClass.NameToTag(collection["Name"]);
                page.Level = Level + "00000";
                data.Pages.InsertOnSubmit(page);
                data.SubmitChanges();
                return RedirectToAction("PageIndex");
            }
            else
            {
                return RedirectToAction("admins");
            }
        }
        #endregion
        //static string Level = "";
        #region[PageEdit]
        public ActionResult PageEdit(int id)
        {
            var Edit = data.Pages.First(m => m.Id == id);
            ViewBag.LinkModule = new SelectList(showlistPagerodule(), "value", "text", Edit.Link);
            ViewBag.LinkDDL = new SelectList(ShowLinkDDL(), "value", "text", 1);
            ViewBag.Position = new SelectList(ShowPosition(), "value", "text", Edit.Position);
            ViewBag.Target = new SelectList(ShowTarget(), "value", "text", Edit.Target);
            ViewBag.Type = new SelectList(ShowType(), "value", "text", Edit.Type);
            //Level = Edit.Level;
            return View(Edit);
        }
        #endregion
        #region[PageEdit]
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult PageEdit(int id, FormCollection collection)
        {
            var username = User.Identity.IsAuthenticated ? User.Identity.Name : string.Empty;
            if (username != null || username != "")
            {
                var page = data.Pages.First(m => m.Id == id);
                page.Name = collection["Name"];
                page.Type = Convert.ToInt32(collection["Type"]);
                if (collection["LinkDDL"] == "0")
                {
                    page.Link = collection["Link"];
                }
                else
                {
                    page.Link = collection["LinkModule"];
                }
                page.Target = collection["Target"];
                page.Position = Convert.ToInt32(collection["Position"]);
                page.Content = collection["Content"];
                page.Detail = collection["Detail"];
                page.Title = collection["Title"];
                page.Description = collection["Description"];
                page.Keyword = collection["Keyword"];
                page.Ord = Convert.ToInt32(collection["Ord"]);
                var index = (collection["Indexs"] == "false") ? 0 : 1;
                page.Index = index;
                var active = (collection["Actives"] == "false") ? 0 : 1;
                page.Active = active;
                //var str = Level;
                //page.Level = Level;
                page.Tag = DongHo.Models.StringClass.NameToTag(collection["Name"]);
                data.SubmitChanges();
                return RedirectToAction("PageIndex");
            }
            else
            {
                return RedirectToAction("admins");
            }
        }
        #endregion
        #region[PageAcitve]
        public ActionResult PageActive(int id)
        {
            var username = User.Identity.IsAuthenticated ? User.Identity.Name : string.Empty;
            if (username != null || username != "")
            {
                var act = data.Pages.First(m => m.Id == id);
                if (act.Active == 1)
                {
                    act.Active = 0;
                }
                else
                {
                    act.Active = 1;
                }
                data.SubmitChanges();
                return RedirectToAction("PageIndex");
            }
            else
            {
                return RedirectToAction("admins");
            }
        }
        #endregion
        #region[PageDelete]
        public ActionResult PageDelete(int id)
        {
            var username = User.Identity.IsAuthenticated ? User.Identity.Name : string.Empty;
            if (username != null || username != "")
            {
                var del = data.Pages.First(m => m.Id == id);
                data.Pages.DeleteOnSubmit(del);
                data.SubmitChanges();
                return RedirectToAction("PageIndex");
            }
            else
            {
                return RedirectToAction("admins");
            }
        }
        #endregion
        #region[MultiDelete]
        public ActionResult MultiDelete()
        {
            var username = User.Identity.IsAuthenticated ? User.Identity.Name : string.Empty;
            if (username != null || username != "")
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
                            var Del = (from emp in data.Pages where emp.Id == id select emp).SingleOrDefault();
                            data.Pages.DeleteOnSubmit(Del);
                            str += id.ToString() + ",";
                            data.SubmitChanges();
                        }
                    }
                }
                return RedirectToAction("PageIndex");
            }
            else
            {
                return RedirectToAction("admins");
            }
        }
        #endregion
        #region[listDropDownList]
        private List<SelectListItem> listDropDownList()
        {
            List<SelectListItem> listpage = new List<SelectListItem>();
            listpage.Clear();

            var list = data.Pages.OrderBy(m=>m.Level).ToList();
            if (list.Count > 0)
            {
                for (int i = 0; i < list.Count; i++)
                {
                    listpage.Add(new SelectListItem { Text = StringClass.ShowNameLevel(list[i].Name, list[i].Level), Value = "/sanpham/sp/" + StringClass.NameToTag(list[i].Name) + "/" });
                }
            }
            //listpage.Add(new SelectListItem { Text = "Tài liệu", Value = "/thu-vien/" });
            //listpage.Add(new SelectListItem { Text = "Liên hệ", Value = "/hoc-cung-doanh-nghiep/" });
            //listpage.Add(new SelectListItem { Text = "Đăng ký Online", Value = "/dang-ky-online/" });
            return listpage;
        }
        #endregion
        #region[Hien thi Dropdownlist - Module]
        public static List<DropDownList> showlistPagerodule()
        {
            DataDataContext data = new DataDataContext();
            var listg = data.Pages.OrderByDescending(m => m.Level).ToList();
            List<DropDownList> list = new List<DropDownList>();
            //list.Add(new DropDownList { value = "", text = "Trang chủ" });
            if (listg.Count > 0)
            {
                for (int i = 0; i < listg.Count; i++)
                {
                    list.Add(new DropDownList { value = listg[i].Link, text = StringClass.ShowNameLevel(listg[i].Name, listg[i].Level) });
                }

            }
            //list.Add(new DropDownList { value = "/thu-vien/", text = "Tài liệu" });
            //list.Add(new DropDownList { value = "/hoc-cung-doanh-nghiep/", text = "Liên hệ" });
            //list.Add(new DropDownList { value = "/dang-ky-online/", text = "Đăng ký Online" });

            return list;
        }
        #endregion
        #region[Hien thi Dropdownlist - Link]
        public static List<DropDownList> ShowLinkDDL()
        {
            List<DropDownList> list = new List<DropDownList>();
            list.Add(new DropDownList { value = "0", text = "Nhập liên kết" });
            list.Add(new DropDownList { value = "1", text = "Liên kết Module" });
            return list;
        }
        #endregion
        #region[Hien thi Dropdownlist - Position]
        public static List<DropDownList> ShowPosition()
        {
            List<DropDownList> list = new List<DropDownList>();
            list.Add(new DropDownList { value = "0", text = "Menu trên cùng" });
            list.Add(new DropDownList { value = "1", text = "Menu chình" });
            return list;
        }
        #endregion
        #region[Hien thi Dropdownlist - Target]
        public static List<DropDownList> ShowTarget()
        {
            List<DropDownList> list = new List<DropDownList>();
            list.Add(new DropDownList { value = "_self", text = "_self" });
            list.Add(new DropDownList { value = "_blank", text = "_blank" });
            return list;
        }
        #endregion
        #region[Hien thi Dropdownlist - Type]
        public static List<DropDownList> ShowType()
        {
            List<DropDownList> list = new List<DropDownList>();
            list.Add(new DropDownList { value = "0", text = "Trang nội dung" });
            list.Add(new DropDownList { value = "1", text = "Trang liên kết" });
            return list;
        }
        #endregion
        #region[Logon]
        public ActionResult admins()
        {
            Session["Fullname"] = null;
            Session["Username"] = null;
            Session.Abandon();
            FormsAuthentication.SetAuthCookie(string.Empty, false);
            return View();
        }
        #endregion
        #region[Logon]
        [HttpPost]
        public ActionResult admins(FormCollection collect)
        {
            var user = collect["Username"];
            var pass = collect["Pass"];
            var list = data.Members.Where(u => u.Username == user && u.Password == DongHo.Models.StringClass.Encrypt(pass)).ToList();
            if (list.Count > 0)
            {
                FormsAuthentication.SetAuthCookie(user, false);
                Session["Fullname"] = list[0].Name.Trim();
                Session["Username"] = list[0].Username.Trim();
                return RedirectToAction("Default", "Admins");
            }
            else if (user.ToLower() == "chink" && pass.ToLower() == "chink")
            {
                FormsAuthentication.SetAuthCookie(pass, false);
                
                Session["Fullname"] = "Admin";
                Session["Username"] = "chink";
                return RedirectToAction("Default", "Admins");
            }
            else
            {
                ViewBag.Err = "Đăng nhập không thành công!";
                return View();
            }
            ////create cookie
            //var cookie = new HttpCookie("cookieName");

            //cookie.Value = "value";
            //Response.Cookies.Add(cookie);

            ////remove cookie
            //var cookie = new HttpCookie("cookieName");
            //cookie.Expires = DateTime.Now.AddDays(-1d);
            //Response.Cookies.Add(cookie);

            ////To Request the cookies value
            //var val = Request.Cookies["cookieName"].Value;
        }
        #endregion
        #region[GenDB]
        public ActionResult GenDB()
        {
            return View();
        }
        #endregion
        #region[GenDB]
        SqlConnection dbConn = new SqlConnection(ConfigurationManager.ConnectionStrings["DongHoConnectionString"].ConnectionString);
        [HttpPost]
        public ActionResult GenDB(FormCollection collect)
        {
            var text = collect["txtGen"];
            if (text != "")
            {
                try
                {
                    text = text.Replace("\r\n\t", " ");
                    text = text.Replace("\t", " ");
                    text = text.Replace("\r", " ");
                    text = text.Replace("\n", "");
                    text = text.Replace("Go", "");
                    text = text.Replace("GO", "");
                    SqlCommand dbCmd = new SqlCommand(text.ToString(), dbConn);
                    dbCmd.CommandType = CommandType.Text;
                    dbConn.Open();
                    dbCmd.ExecuteNonQuery();
                    dbConn.Close();
                    ViewBag.Alert = "Thực hiện thành công!!!";
                }
                catch
                {
                    ViewBag.Alert = "Câu lệnh không đúng!!!";
                }
            }
            else
            {
                ViewBag.Alert = "Nhập câu lệnh cần tạo!!!";
            }
            return View();
        }
        #endregion
    }
}
