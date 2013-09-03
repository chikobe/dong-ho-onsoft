using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DongHo.Models;
using DongHo.ViewModels;

namespace DongHo.Controllers
{
    public class sanphamController : Controller
    {
        //
        // GET: /sanpham/
        DataDataContext db = new DataDataContext();
        #region[View chi tiet sp]
        public ActionResult chitiet()
        {
            string tag = "";
            string chuoi = "";
            string detail = "";
            int? count = 0;
            string sptt = "";
            string anh = "";
            string anhHuge = "";
            string addCart = "";
            if (RouteData.Values["id"] != null) tag = RouteData.Values["id"].ToString();
            if (tag != "")
            {
                var list = db.Products.Where(m => m.Tag == tag).FirstOrDefault();
                var brand = db.Brands.First(m => m.Id == list.BrandId);
                var pro = db.Products.Where(m => m.CatL2 == list.CatL2).OrderByDescending(m => m.Id).Take(10).ToList();
                var proimg = db.ProImages.Where(m => m.ProId == list.Id).ToList();
                for (int k = 0; k < proimg.Count; k++)
                {
                    var a = proimg[k].Image.IndexOf("_big");
                    var b = proimg[k].Image.IndexOf("_huge");
                    if (a > 0) anh = proimg[k].Image;
                    if (b > 0) anhHuge = proimg[k].Image;
                }
                var bill = db.tbBilldetails.Where(m => m.proid == list.Id && m.status == 0).ToList();
                var demSp = bill.Count;//lay tat ca hoa don chua giao hang cua sp tren
                int g = 0;
                int? n = 0;
                if (list.SpTon == null) n = 0;
                else n = list.SpTon;
                g = (demSp == n) ? 1 : 0;//Image5 dung de luu so sp con ton trong kho
                chuoi += "<input type=\"hidden\" id=\"item_id\" value=\"" + list.Id + "\" />";
                chuoi += "<div class=\"mainLeft\">";
                chuoi += "<div class=\"zoom-small-image\">";
		        chuoi += "<a href='"+ anhHuge +"' class = 'cloud-zoom' rel=\"position: 'inside' , showTitle: false\" id=\"zoom1\">";
                chuoi += "<img src=\"" + anh + "\" title=\"Ảnh sản phẩm\" alt=''/></a> ";
	            chuoi += "</div>";
                if (g != 0)
                {
                    chuoi += "<span class='outStock'></span>";
                }
                chuoi += "</div>";
                chuoi += "<div class=\"mainRight\">";
                chuoi += "<h2>" + list.Name + "</h2>";
                chuoi += "<p>Mã sản phẩm : <span>" + list.Id + "</span> - Thương hiệu: <span>" + brand.Name + "</span></p>";
                chuoi += "<div class=\"clear\"></div>";
                chuoi += "<div class=\"divInfo\">";
                if (g == 0)
                {
                    chuoi += "<p><span class=\"txtw140\">Giá bán</span><span class=\"stock\">Còn hàng</span></p>";
                }
                else
                {
                    chuoi += "<p><span class=\"txtw140\">Giá bán</span><span class=\"nostock\">Hết hàng</span></p>";
                }
                chuoi += "<p><span class=\"txtw140 price\">" + StringClass.Format_Price(list.Price.ToString()) + " VNĐ</span><span class=\"txtout\">" + StringClass.Format_Price(list.PiceOld.ToString()) + " VNĐ</span></p>";
                chuoi += "<span class=\"lblProduct\">&nbsp;</span>";
                chuoi += "</div>";
                chuoi += "<div class=\"clear\"></div>";
                chuoi += "<div class=\"divInfo\">";
                chuoi += "<p>";
                chuoi += "<span class=\"txtw140\">Số lượng mua:</span>";
                chuoi += "<span><input type=\"text\" style=\"width:20px; height:18px; text-align:center; float:left;\" value=\"1\" id=\"quantity\" name=\"quantity\" />/<span id=\"max-quantity\" style=\"float:right;\">10</span>";
                chuoi += "<span style=\"float:left\">";
                chuoi += "<a href=\"javascript:void(0);\" class=\"upNum\">&nbsp;</a>";
                chuoi += "<a href=\"javascript:void(0);\" class=\"downNum\">&nbsp;</a>";
                chuoi += "</span>";
                chuoi += "</span>";
                chuoi += "</p>";
                chuoi += "<div class=\"clear\"></div>";
                chuoi += "<p style=\"margin-top:10px; float:right;\">";
                if (g == 0)
                {
                    chuoi += "<a title=\"Cho vào giỏ hàng\" href=\"javascript:;\" class=\"btnBuyNow add-to-cart\"><span>Cho vào giỏ hàng</span></a>";
                    chuoi += "<a title=\"Mua ngay\" href=\"javascript:;\" class=\"btnAddCart\" rel=\"/sanpham/checkout\"><span>Mua ngay</span></a>";
                    addCart += "<a title=\"Mua ngay\" href=\"javascript:;\" class=\"btnAddCart mg10_45\" rel=\"/sanpham/checkout\"><span>Mua ngay</span></a>";
                }
                else
                {
                    chuoi += "<a title=\"Cháy hàng\" href=\"javascript:;\" class=\"btnBuy Disabled\" ><span>Cho vào giỏ hàng</span></a>";
                    chuoi += "<a title=\"Cháy hàng\" href=\"javascript:;\" class=\"btnAdd Disabled\"><span>Mua ngay</span></a>";
                    addCart += "<a title=\"Cháy hàng\" href=\"javascript:;\" class=\"btnAdd mg10_45 Disabled\" rel=\"/sanpham/checkout\"><span>Mua ngay</span></a>";
                }
                chuoi += "</p>";
                chuoi += "</div>";
                chuoi += "</div>";
                detail += list.Detail;
                count = list.Count;
                sptt += "<ul>";
                for (int i = 0; i < pro.Count; i++)
                {
                    sptt += "<li>";
                    sptt += "<h3><a href=\"/sanpham/chitiet/" + pro[i].Tag + "\">" + pro[i].Name + "</a></h3>";
                    sptt += "<a href=\"/sanpham/chitiet/" + pro[i].Tag + "\"><img src=\"" + pro[i].Image + "\" /></a>";
                    sptt += "<p>Model: <span>" + pro[i].Id + "</span></p>";
                    sptt += "<p>Giá: <span class=\"require\">" + StringClass.Format_Price(pro[i].Price.ToString()) + " VNĐ</span></p>";
                    sptt += "<p class=\"txtout\">" + StringClass.Format_Price(pro[i].PiceOld.ToString()) + " VNĐ</p>";
                    sptt += "</li>";
                }
                sptt += "</ul>";
                list.View = (list.View + 1);
                db.SubmitChanges();
                ViewBag.Content = detail;//thong tin chi tiet sp
                ViewBag.View = chuoi;
                ViewBag.Count = count;//so nguoi da mua sp
                ViewBag.sp = sptt;//sp tuong tu
                ViewBag.AddCart = addCart;
            }
            return View();
        }
        #endregion
        #region[View sp theo cat - catLevel2]
        public ActionResult sp()
        {
            string tag = "";
            string chuoi = "";
            if (RouteData.Values["id"] != null)
            {
                tag = RouteData.Values["id"].ToString();
            }
            if (tag != "")
            {
                var c = db.Categories.Where(m => m.Tag == tag).FirstOrDefault();
                var list = db.Products.Where(m => m.CatL2 == c.Id).ToList();
                var cat = db.Categories.Where(m => m.Tag == tag && m.Level.Length==5).ToList();
                if (list.Count > 0 && cat.Count == 0)
                {
                    for (int i = 0; i < list.Count; i++)
                    {
                        var proImg = db.ProImages.Where(m => m.ProId == list[i].Id).ToList();
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
                        chuoi += "<div class=\"div-pro\">";
                        chuoi += "<a href=\"/sanpham/chitiet/" + list[i].Tag + "\">";
                        chuoi += "<img src=\"" + k + "\" />";
                        chuoi += "<div class=\"titlePro\">";
                        chuoi += "<p>" + FormatContentNews(list[i].Name,50) + "</p>";
                        chuoi += "<span class=\"require\">" + StringClass.Format_Price(list[i].Price.ToString()) + " VNĐ</span>";
                        chuoi += "<span class=\"oldPrice\">" + StringClass.Format_Price(list[i].PiceOld) + " VNĐ</span>";
                        chuoi += "<div class=\"hideTitlePro\">";
                        chuoi += "<span class=\"require\">Giảm giá " + list[i].Codepro + " %</span>";
                        chuoi += "<div style=\"padding:8px;\">" + list[i].Content + "</div>";
                        chuoi += "</div>";
                        chuoi += "</div>";
                        chuoi += "</a>";
                        chuoi += "</div>";
                    }
                }
                else
                {
                    if (cat.Count > 0)
                    {
                        var listpro = db.Products.Where(m => m.CatId == cat[0].Id).ToList();
                        for (int i = 0; i < listpro.Count; i++)
                        {
                            var proImg = db.ProImages.Where(m => m.ProId == listpro[i].Id).ToList();
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
                            chuoi += "<div class=\"div-pro\">";
                            chuoi += "<a href=\"/sanpham/chitiet/" + listpro[i].Tag + "\">";
                            chuoi += "<img src=\"" + k + "\" />";
                            chuoi += "<div class=\"titlePro\">";
                            chuoi += "<p>" + FormatContentNews(listpro[i].Name,50) + "</p>";
                            chuoi += "<span class=\"require\">" + StringClass.Format_Price(listpro[i].Price.ToString()) + " VNĐ</span>";
                            chuoi += "<span class=\"oldPrice\">" + StringClass.Format_Price(listpro[i].PiceOld) + " VNĐ</span>";
                            chuoi += "<div class=\"hideTitlePro\">";
                            chuoi += "<span class=\"require\">Giảm giá " + listpro[i].Codepro + " %</span>";
                            chuoi += "<div style=\"padding:8px;\">" + listpro[i].Content + "</div>";
                            chuoi += "</div>";
                            chuoi += "</div>";
                            chuoi += "</a>";
                            chuoi += "</div>";
                        }
                    }
                }

            }
            ViewBag.View = chuoi;
            return View();
        }
        #endregion
        #region[Gio hang]
        public ActionResult checkout()
        {
            if (Session["ShoppingCart"] != null)
            {
                var cart = (ShoppingCartViewModel)Session["ShoppingCart"];
                return View(cart);
            }
            else
            {
                return Redirect("/Home/Index");
            }
        }
        #endregion
        #region[View gio hang Top]
        [HttpPost]
        public JsonResult command(string co)
        {
            string anh = "";
            var re = co.Replace("\":", ":").Replace(",\"", ":").Replace("{\"", ":").Replace("}", ":");
            var tr = re.Split(':');
            string chuoi = "";
            int count = 0;
            List<int> numCart = new List<int>();
            List<Product> obj = new List<Product>();
            chuoi += "<ul>";
            for (int i = 0; i < tr.Length; i++)
            {
                if (i == 0 || i == (tr.Length - 1))
                { }
                else if (i % 2 != 0)
                {
                    var list = db.Products.Where(m => m.Id == int.Parse(tr[i])).FirstOrDefault();
                    var proimg = db.ProImages.Where(m => m.ProId == list.Id).ToList();
                    for (int k = 0; k < proimg.Count; k++)
                    {
                        var a = proimg[k].Image.IndexOf("_small");
                        if (a > 0)
                        {
                            anh = proimg[k].Image;
                            break;
                        }
                    }
                    chuoi += "<li class='proCart clearfix'>";
                    chuoi += "<img src='" + anh + "'/>";
                    chuoi += "<a href='/sanpham/chitiet/" + list.Tag + "'>" + FormatContentNews(list.Name,50) + " <span>x " + tr[(i + 1)] + "</span></a>";
                    chuoi += "</li>";
                    obj.Add(list);
                    count += Convert.ToInt32(tr[(i + 1)]);
                    numCart.Add(Convert.ToInt32(tr[(i + 1)]));
                }
            }
            Session["proId"] = obj;
            chuoi += "<li><p class='pCartView'><a class='btn-view-cart' href='/sanpham/checkout'>Xem giỏ hàng (" + count + " sản phẩm)</a></p></li>";
            chuoi += "<ul>";
            Session["count"] = numCart;
            AddToCart(tr);
            return Json(new { success = chuoi, html=count });
        }
        #endregion
        //#region[Them moi sp vao gio hang]
        //int cartTotal;
        //ShoppingCartViewModel shoppCart;
        //public void AddToCart(String[] tr) {
        //    InitShoppingCartSession();
        //    shoppCart = (ShoppingCartViewModel)Session["ShoppingCart"];
        //    Cart cartItem;
        //    int sl = 0;
        //    var soluong = (List<int>)Session["count"];
        //    for (int i = 0; i < tr.Length; i++)
        //    {
        //        if (i == 0 || i == (tr.Length - 1))
        //        { continue; }
        //        else if(i % 2 != 0)
        //        {
        //            var obj = db.Products.Where(m => m.Id == int.Parse(tr[i])).FirstOrDefault();
        //            int gia = 0;
        //            gia = int.Parse(obj.Price.ToString());
        //            int tong = (soluong[sl] * gia);
        //            cartItem = new Cart
        //            {
        //                productId = obj.Id,
        //                productImage = obj.Image,
        //                productName = obj.Name,
        //                productTag = obj.Tag,
        //                price = obj.Price.ToString(),
        //                count = soluong[sl],
        //                total = tong
        //            };
        //            shoppCart.CartItems.Add(cartItem);
        //            sl++;
        //        }
        //    }
        //    for (int k = 0; k < shoppCart.CartItems.Count; k++)
        //    {
        //        cartTotal += shoppCart.CartItems[k].total;
        //    }
        //    shoppCart.CartTotal = cartTotal;
        //    Session["ShoppingCart"] = shoppCart;
        //}
        //#endregion
        #region[Them moi sp vao gio hang]
        int cartTotal;
        ShoppingCartViewModel shoppCart;
        public void AddToCart(Array tr)
        {
            var soluong = (List<int>)Session["count"];//so luong sp co trong gio hang tuong ung voi tung sp trong Session["proId"]
            var data = (List<Product>)Session["proId"];//luu thong tin sp co trong gio hang
            for (int i = 0; i < data.Count; i++)
            {
                Product obj = data[i];
                int gia = 0;
                gia = int.Parse(obj.Price.ToString());
                int tong = (soluong[i] * gia);
                int flag = -2;
                if (Session["ShoppingCart"] == null)
                {
                    InitShoppingCartSession();
                }
                shoppCart = (ShoppingCartViewModel)Session["ShoppingCart"];
                int dem = GetCartItem(shoppCart, obj.Id, soluong[i]);
                if (dem == flag)
                {
                    var proImg = db.ProImages.Where(m => m.ProId == obj.Id).ToList();
                    string k = "";
                    for (int l = 0; l < proImg.Count; l++)
                    {
                        var a = proImg[l].Image.IndexOf("_noz");
                        if (a > -1)
                        {
                            k = proImg[l].Image;
                            break;
                        }
                    }
                    Cart cartItem;
                    cartItem = new Cart
                    {
                        productId = obj.Id,
                        productImage = k,
                        productName = FormatContentNews(obj.Name,50),
                        productTag = obj.Tag,
                        price = obj.Price.ToString(),
                        count = soluong[i],
                        total = tong
                    };
                    shoppCart.CartItems.Add(cartItem);
                }
                else
                {
                    if (dem != -1)
                    {
                        shoppCart.CartItems[i].count = soluong[i];
                        shoppCart.CartItems[i].total = Convert.ToInt32(shoppCart.CartItems[i].price) * shoppCart.CartItems[i].count;
                    }
                }
            }
            if (shoppCart.CartItems.Count > 0)
            {
                for (int k = 0; k < shoppCart.CartItems.Count; k++)
                {
                    cartTotal += shoppCart.CartItems[k].total;
                }
            }
            shoppCart.CartTotal = cartTotal;
            Session["ShoppingCart"] = shoppCart;
        }
        #endregion
        #region[Tao moi gio hang]
        public void InitShoppingCartSession()
        {
            var shoppingCart = new ShoppingCartViewModel();
            Session["ShoppingCart"] = shoppingCart;
        }
        #endregion
        #region[Kiem tra sp ton tai trong gio hang]
        private int GetCartItem(ShoppingCartViewModel cart, int key, int count)
        {
            int found = -2;
            if (cart.CartItems.Count > 0)
            {
                for (int i = 0; i < cart.CartItems.Count; i++)
                {
                    if (cart.CartItems[i].productId == key && cart.CartItems[i].count == count)
                    {
                        found = -1;
                    }
                    else if (cart.CartItems[i].productId == key && cart.CartItems[i].count != count)
                    {
                        found = 0;
                    }
                }
            }
            return found;
        }
        #endregion
        #region[Xoa sp khoi gio hang]
        [HttpPost]
        public void RemoveFromCart(int id)
        {
            ShoppingCartViewModel shoppCart = (ShoppingCartViewModel)Session["ShoppingCart"];
            for (int i = 0; i < shoppCart.CartItems.Count; i++)
            {
                if (shoppCart.CartItems[i].productId == id)
                {
                    shoppCart.CartItems.RemoveAt(i);
                    break;
                }
            }
            if (shoppCart.CartItems.Count > 0)
            {
                for (int j = 0; j < shoppCart.CartItems.Count; j++)
                {
                    cartTotal += shoppCart.CartItems[j].total;
                }
                shoppCart.CartTotal = cartTotal;
            }
            else
            {
                shoppCart.CartTotal = 0;
            }

            Session["ShoppingCart"] = shoppCart;
        }
        #endregion
        #region[Cap nhat so luong trong gio hang]
        [HttpPost]
        public void UpdateCartCountItem(int id, int cartCount)
        {
            ShoppingCartViewModel shoppCart = (ShoppingCartViewModel)Session["ShoppingCart"];
            for (int i = 0; i < shoppCart.CartItems.Count; i++)
            {
                if (shoppCart.CartItems[i].productId == id)
                {
                    shoppCart.CartItems[i].count = cartCount;
                    shoppCart.CartItems[i].total = Convert.ToInt32(shoppCart.CartItems[i].price) * cartCount;
                    break;
                }
            }
            for (int j = 0; j < shoppCart.CartItems.Count; j++)
            {
                cartTotal += shoppCart.CartItems[j].total;
            }
            shoppCart.CartTotal = cartTotal;
            Session["ShoppingCart"] = shoppCart;
        }
        #endregion
        #region[Them vao gio hang Top]
        [HttpPost]
        public ActionResult UpdateTopCart(string co, int type)
        {
            string anh = "";
            var re = co.Replace("\":", ":").Replace(",\"", ":").Replace("{\"", ":").Replace("}", ":");
            var tr = re.Split(':');
            string chuoi = "";
            int count = 0;
            List<int> numCart = new List<int>();
            List<Product> obj = new List<Product>();
            chuoi += "<ul>";
            for (int i = 0; i < tr.Length; i++)
            {
                if (i == 0 || i == (tr.Length - 1))
                { }
                else if(i % 2 != 0)
                {
                    var list = db.Products.Where(m => m.Id == int.Parse(tr[i])).FirstOrDefault();
                    var proimg = db.ProImages.Where(m => m.ProId == list.Id).ToList();
                    for (int k = 0; k < proimg.Count; k++)
                    {
                        var a = proimg[k].Image.IndexOf("_small");
                        if (a > 0)
                        {
                            anh = proimg[k].Image;
                            break;
                        }
                    }
                    chuoi += "<li class='proCart clearfix'>";
                    chuoi += "<img src='" + anh + "'/>";
                    chuoi += "<a href='/sanpham/chitiet/" + list.Tag + "'>" + list.Name + " <span>x " + tr[(i+1)] + "</span></a>";
                    chuoi += "</li>";
                    obj.Add(list);
                    count += Convert.ToInt32(tr[(i + 1)]);
                    numCart.Add(Convert.ToInt32(tr[(i + 1)]));
                }
            }
            chuoi += "</ul>";
            Session["proId"] = obj;
            chuoi += "<p class='pCartView'><a class='btn-view-cart' href='/sanpham/checkout'>Xem giỏ hàng (" + count + " sản phẩm)</a></p>";
            Session["count"] = numCart;
            AddToCart(tr);
            if (type == 0)
            {
                return Json(new { success = chuoi, html = count });
            }
            else
            {
                return RedirectToAction("checkout");
            }
        }
        #endregion
        #region[_siteMap]
        public ActionResult _siteMap()
        {
            string pathway = "";
            var tag = RouteData.Values["id"];
            if (tag != null)
            {
                pathway += "<div class=\"pathway\">";
                var pro = db.Products.Where(m => m.Tag == tag.ToString()).FirstOrDefault();
                if (pro != null)
                {
                    var Cat = db.Categories.Where(m => m.Id == pro.CatId).FirstOrDefault();
                    var CatL2 = db.Categories.Where(m => m.Id == pro.CatL2).FirstOrDefault();
                    if (CatL2 != null)
                    {
                        pathway += "<a href=\"/Home/Index\">Trang chủ</a><a href=\"/sanpham/sp/" + Cat.Tag + "\">" + Cat.Name + "</a><a href=\"/sanpham/sp/" + CatL2.Tag + "\">" + CatL2.Name + "</a>";
                    }
                    else
                    {
                        pathway += "<a href=\"/Home/Index\">Trang chủ</a><a href=\"/sanpham/sp/" + Cat.Tag + "\">" + Cat.Name + "</a>";
                    }
                }
                else
                {
                    var Cat = db.Categories.Where(m => m.Tag == tag.ToString()).FirstOrDefault();
                    if (Cat != null)
                    {
                        if (Cat.Level.Length > 5)
                        {
                            var CatL1 = db.Categories.Where(m => m.Level.Length == 5 && m.Level == Cat.Level.Substring(0, 5)).FirstOrDefault();
                            pathway += "<a href=\"/Home/Index\">Trang chủ</a><a href=\"/sanpham/sp/" + CatL1.Tag + "\">" + CatL1.Name + "</a><a href=\"/sanpham/sp/" + Cat.Tag + "\">" + Cat.Name + "</a>";
                        }
                        else
                        {
                            pathway += "<a href=\"/Home/Index\">Trang chủ</a><a href=\"/sanpham/sp/" + Cat.Tag + "\">" + Cat.Name + "</a>";
                        }
                    }
                }
                pathway += "</div>";
            }
            ViewBag.View = pathway;
            return PartialView();
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
    }
}
