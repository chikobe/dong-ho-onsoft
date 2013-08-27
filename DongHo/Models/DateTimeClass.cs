using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DongHo.Models
{
    public class DateTimeClass
    {
        /// <summary>
        /// Convert string Datetime
        /// </summary>
        /// <param name="DateTime">string Datetime to convert</param>
        /// <param name="DateFormat">string format date</param>
        /// <returns>string Datetime format as DateFormat</returns>
        public static string ConvertDateTime(string DateTime, string DateFormat)
        {
            if (DateTime.Length > 0)
            {
                DateTime dt = Convert.ToDateTime(DateTime);
                return dt.ToString(DateFormat);
            }
            else
            {
                return "";
            }
        }


        /// <summary>
        /// Convert Datetime
        /// </summary>
        /// <param name="DateTime">Datetime to convert</param>
        /// <param name="DateFormat">string format date</param>
        /// <returns>string Datetime format as DateFormat</returns>
        public static string ConvertDateTime(DateTime DateTime, string DateFormat)
        {
            if (DateTime != null)
            {
                return DateTime.ToString(DateFormat);
            }
            else
            {
                return "";
            }
        }

        
        /// <summary>
        /// Convert Datetime
        /// </summary>
        /// <param name="DateTime">Datetime to convert</param>
        /// <returns>Default dd/MM/yyyy HH:mm:ss</returns>
        ///         
        public static string ConvertDateTime(string DateTime)
        {
            return ConvertDateTime(DateTime, "dd/MM/yyyy hh:mm:ss tt");
        }
        public static string ConvertDateTime(DateTime DateTime)
        {
            return ConvertDateTime(DateTime, "dd/MM/yyyy HH:mm:ss tt");
        }

        public static string ConvertDateTimeddMMyyHHmm(DateTime DateTime)
        {
            return ConvertDateTime(DateTime, "dd/MM/yyyy HH:mm");
        }
        
        public static string ConvertDateTimeddMMyyyy(string DateTime)
        {
            return ConvertDateTime(DateTime, "dd/MM/yyyy");
        }

        public static string ConvertDateTimeMMddyyyy(string DateTime)
        {
            return ConvertDateTime(DateTime, "MM/dd/yyyy HH:mm:ss");
        }

        /// <summary>
        /// Convert string Date
        /// </summary>
        /// <param name="Date">string Date to convert</param>
        /// <returns>Default datetime format dd/MM/yyyy</returns>
        public static string ConvertDate(string Date)
        {
            return ConvertDate(Date, "dd/MM/yyyy");
        }
        /// <summary>
        /// Convert string Date
        /// </summary>
        /// <param name="Date">string Date to convert</param>
        /// <param name="DateFormat">string format date</param>
        /// <returns>string Date format as DateFormat</returns>
        public static string ConvertDate(string Date, string DateFormat)
        {
            if (Date.Length > 0)
            {
                DateTime dt = Convert.ToDateTime(Date);
                return dt.ToString(DateFormat);
            }
            else
            {
                return "";
            }
        }

        /// <summary>
        /// Convert Date
        /// </summary>
        /// <param name="Date">Date to convert</param>
        /// <returns>Default datetime format dd/MM/yyyy</returns>
        public static string ConvertDate(DateTime Date)
        {
            return ConvertDate(Date, "dd/MM/yyyy");
        }
        /// <summary>
        /// Convert Date
        /// </summary>
        /// <param name="Date">Date</param>
        /// <param name="DateFormat">string format date</param>
        /// <returns>string Date format as DateFormat</returns>
        public static string ConvertDate(DateTime Date, string DateFormat)
        {
            if (Date != null)
            {
                return Date.ToString(DateFormat);
            }
            else
            {
                return "";
            }
        }

        public static DateTime Convert2Date(string Date)
        {
            DateTime dt = Convert.ToDateTime(Date);
            return dt;
        }

       
        /// <summary>
        ///  Convert string Time
        /// </summary>
        /// <param name="Time">string Time to convert</param>
        /// <returns>Default HH:mm:ss</returns>
        public static string ConvertTime(string Time)
        {
            return ConvertTime(Time, "HH:mm:ss");
        }

        public static string ConvertTimett(string Time)
        {
            return ConvertTime(Time, "HH:mm tt");
        }

        /// <summary>
        ///  Convert string Time
        /// </summary>
        /// <param name="Time">string Time to convert</param>
        /// <param name="TimeFormat">string format time</param>
        /// <returns>string Time format as TimeFormat</returns>
        public static string ConvertTime(string Time, string TimeFormat)
        {
            if (Time.Length > 0)
            {
                DateTime dt = Convert.ToDateTime(Time);
                return dt.ToString(TimeFormat);
            }
            else
            {
                return "";
            }
        }

        /// <summary>
        ///  Convert Time
        /// </summary>
        /// <param name="Time">Time to convert</param>
        /// <returns>Default HH:mm:ss</returns>
        public static string ConvertTime(DateTime Time)
        {
            return ConvertTime(Time, "HH:mm:ss");
        }

        /// <summary>
        ///  Convert Time
        /// </summary>
        /// <param name="Time">Time to convert</param>
        /// <param name="TimeFormat">string format time</param>
        /// <returns>string Time format as TimeFormat</returns>
        public static string ConvertTime(DateTime Time, string TimeFormat)
        {
            if (Time != null)
            {
                return Time.ToString(TimeFormat);
            }
            else
            {
                return "";
            }
        }

        public static DateTime ConvertToDate1(string Date, string Format)
        {
            DateTime date = DateTime.MinValue;
            try
            {
                string[] s = Date.Split(new char[] { '/', '-' });
                int d = date.Day;
                int m = date.Month;
                int y = date.Year;
                if (Format == "dd/MM/yyyy" || Format == "dd-MM-yyyy")
                {
                    int.TryParse(s[0], out d);
                    int.TryParse(s[1], out m);
                }
                else if (Format == "MM/dd/yyyy" || Format == "MM-dd-yyyy")
                {
                    int.TryParse(s[0], out d);
                    int.TryParse(s[1], out m);
                }
                else
                {
                    int.TryParse(s[0], out m);
                    int.TryParse(s[1], out d);
                }
                int.TryParse(s[2], out y);

                date = new DateTime(y, m, d);
            }
            catch { }
            return date;
        }


        
    }
}