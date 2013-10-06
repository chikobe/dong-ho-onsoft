using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Collections;
using System.Web.UI;
using DongHo.ViewModels;

namespace DongHo.Models
{
    public class StringClass
    {
        /// <summary>
        /// Ma hoa chuoi ky tu (MD5)
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string Encrypt(string value)
        {
            if (string.IsNullOrEmpty(value))
                return string.Empty;
            var md5 = new MD5CryptoServiceProvider();
            byte[] valueArray = Encoding.ASCII.GetBytes(value);
            valueArray = md5.ComputeHash(valueArray);
            var sb = new StringBuilder();
            for (int i = 0; i < valueArray.Length; i++)
                sb.Append(valueArray[i].ToString("x2").ToLower());
            return sb.ToString();
        }
        /// <summary>
        /// Tao chuoi dung cho rewrite url
        /// </summary>
        /// <param name="strName"></param>
        /// <returns></returns>
        #region Name To Tag
        public static string NameToTag(string strName)
        {
            string strReturn = "";
            Regex regex = new Regex("\\p{IsCombiningDiacriticalMarks}+");
            strReturn = Regex.Replace(strName, "[^\\w\\s]", string.Empty).Replace(" ", "-").ToLower();
            string strFormD = strReturn.Normalize(System.Text.NormalizationForm.FormD);
            return regex.Replace(strFormD, string.Empty).Replace("đ", "d");
        }
        #endregion
        /// <summary>
        /// Xoa ky tu unicode tu chuoi
        /// </summary>
        /// <param name="strString"></param>
        /// <returns></returns>
        #region Remove Unicode
        public static string RemoveUnicode(string strString)
        {
            Regex regex = new Regex("\\p{IsCombiningDiacriticalMarks}+");
            string strFormD = strString.Normalize(NormalizationForm.FormD);
            return regex.Replace(strFormD, string.Empty).Replace("đ", "d");
        }
        #endregion
        #region[ShowNameLevel]
        public static string ShowNameLevel(string Name, string Level)
        {
            int strLevel = (Level.Length / 5);
            string strReturn = "";
            for (int i = 1; i < strLevel; i++)
            {
                strReturn = strReturn + ".....";
            }
            strReturn += Name;
            return strReturn;
        }
        #endregion
        /// <summary>
        /// Tao mot chuoi ngau nghien
        /// </summary>
        /// <param name="size"></param>
        /// <returns></returns>
        #region Random String
        public static string RandomString(int size)
        {
            Random rnd = new Random();
            string srds = "";
            string[] str = { "a", "b", "c", "d", "e", "f", "g", "h", "i", "j", "k", "l", "m", "n", "o", "p", "q", "r", "s", "t", "u", "v", "w", "x", "y", "z", "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "O", "P", "Q", "R", "S", "T", "U", "V", "W", "X", "Y", "Z", "1", "2", "3", "4", "5", "6", "7", "8", "9", "0" };
            for (int i = 0; i < size; i++)
            {
                srds = srds + str[rnd.Next(0, 61)];
            }
            return srds;
        }
        #endregion

        public static string ShowPageType(string ActiveCode)
        {
            return ActiveCode == "1" ? "Trang liên kết" : "Trang nội dung";
        }
        public static string ShowSupportType(string type)
        {
            string strString = "";
            string[] myArr = new string[] { "0,yahoo", "1,skype","2,hotline" };
            char[] splitter = { ',', ';' };
            for (int i = 0; i < myArr.Length; i++)
            {
                string[] arr = myArr[i].Split(splitter);
                if (arr[0].Equals(type))
                {
                    strString = arr[1];
                    break;
                }
            }
            return strString;
        }
        #region[Xem kieu thanh vien]
        public static string ShowTypeMember(string type)
        {
            string strString = "";
            string[] myArr = new string[] { "0,Tài khoản thường", "1,Tài khoản Vip" };
            char[] splitter = { ',', ';' };
            for (int i = 0; i < myArr.Length; i++)
            {
                string[] arr = myArr[i].Split(splitter);
                if (arr[0].Equals(type))
                {
                    strString = arr[1];
                    break;
                }
            }
            return strString;
        }
        #endregion
        #region[Xem hinh thuc thanh toan]
        public static string ShowTypePay(string type)
        {
            string strString = "";
            string[] myArr = new string[] { "1,Thanh toán tại nhà", "2,Thanh toán tại cửa hàng" };
            char[] splitter = { ',', ';' };
            for (int i = 0; i < myArr.Length; i++)
            {
                string[] arr = myArr[i].Split(splitter);
                if (arr[0].Equals(type))
                {
                    strString = arr[1];
                    break;
                }
            }
            return strString;
        }
        #endregion
        #region[Xem kieu tinh trang don hang]
        public static string ShowStateBill(string state)
        {
            string strString = "";
            string[] myArr = new string[] { "0,Chưa giao hàng", "1,Đã giao hàng", "2,Đã hủy" };
            char[] splitter = { ',', ';' };
            for (int i = 0; i < myArr.Length; i++)
            {
                string[] arr = myArr[i].Split(splitter);
                if (arr[0].Equals(state))
                {
                    strString = arr[1];
                    break;
                }
            }
            return strString;
        }
        #endregion
        #region [Format_Price]
        public static string Format_Price(string Price)
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
        public static List<DropDownList> DDLTypeAdvertise()
        {
            List<DropDownList> list = new List<DropDownList>();
            list.Add(new DropDownList { value = "1", text = "Logo" });
            list.Add(new DropDownList { value = "2", text = "Quảng cáo 2 bên - Bên phải" });
            list.Add(new DropDownList { value = "3", text = "Quảng cáo 2 bên - Bên trái" });
            return list;
        }
    }
}